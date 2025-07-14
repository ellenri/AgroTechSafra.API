using OpenAI.Chat;
using AgroTechSafra.API.Models;
using System.Diagnostics;
using System.Text.Json;

namespace AgroTechSafra.API.Services
{
    public interface IAnaliseIAService
    {
        Task<AnaliseIAResponse> AnalisarPragasAsync(List<AmostragemPragasCsvModel> dados, string? instrucoesEspecificas = null);
    }

    public class AnaliseIAService : IAnaliseIAService
    {
        private readonly ChatClient _chatClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AnaliseIAService> _logger;

        public AnaliseIAService(IConfiguration configuration, ILogger<AnaliseIAService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var apiKey = _configuration["OpenAI:ApiKey"];
            var modelo = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";

            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("Chave da API OpenAI não configurada no appsettings.json");

            _chatClient = new ChatClient(modelo, apiKey);
        }

        public async Task<AnaliseIAResponse> AnalisarPragasAsync(List<AmostragemPragasCsvModel> dados, string? instrucoesEspecificas = null)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation($"Iniciando análise de IA para {dados.Count} registros");

                // Construir prompt especializado
                var prompt = ConstruirPromptEspecializado(dados, instrucoesEspecificas);

                var mensagens = new List<ChatMessage>
                {
                    new SystemChatMessage(@"Você é um especialista em entomologia agrícola e manejo integrado de pragas. 
                        Analise os dados de amostragem fornecidos e identifique as pragas presentes, avalie riscos e forneça recomendações práticas.
                        Sempre responda em português brasileiro e seja específico nas recomendações."),
                    new UserChatMessage(prompt)
                };

                var opcoes = new ChatCompletionOptions
                {
                    MaxOutputTokenCount = _configuration.GetValue<int>("OpenAI:MaxTokens", 4000),
                    Temperature = (float)_configuration.GetValue<double>("OpenAI:Temperature", 0.7)
                };

                var resposta = await _chatClient.CompleteChatAsync(mensagens, opcoes);
                var conteudo = resposta.Value.Content[0].Text;

                stopwatch.Stop();

                // Processar resposta da IA
                var analiseProcessada = ProcessarRespostaIA(conteudo);
                
                // Adicionar métricas
                analiseProcessada.Metricas = new MetricasProcessamento
                {
                    TotalRegistrosProcessados = dados.Count,
                    TempoProcessamento = stopwatch.Elapsed,
                    TokensUtilizados = resposta.Value.Usage.TotalTokenCount,
                    ModeloIA = _configuration["OpenAI:Model"] ?? "gpt-4o-mini"
                };

                analiseProcessada.Sucesso = true;
                analiseProcessada.AnaliseCompleta = conteudo;

                _logger.LogInformation($"Análise concluída em {stopwatch.ElapsedMilliseconds}ms. Tokens: {resposta.Value.Usage.TotalTokenCount}");

                return analiseProcessada;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Erro ao analisar dados com IA");

                return new AnaliseIAResponse
                {
                    Sucesso = false,
                    MensagemErro = $"Erro na análise: {ex.Message}",
                    Metricas = new MetricasProcessamento
                    {
                        TotalRegistrosProcessados = dados.Count,
                        TempoProcessamento = stopwatch.Elapsed,
                        ModeloIA = _configuration["OpenAI:Model"] ?? "gpt-4o-mini"
                    }
                };
            }
        }        private string ConstruirPromptEspecializado(List<AmostragemPragasCsvModel> dados, string? instrucoesEspecificas)
        {
            var amostra = dados.Take(5).ToList();
            var resumoGeral = dados.GroupBy(d => d.NomeCultura)
                .Select(g => new { Cultura = g.Key, Registros = g.Count() })
                .ToList();

            var prompt = $@"
# ANÁLISE DE AMOSTRAGEM DE PRAGAS AGRÍCOLAS

## DADOS GERAIS:
- **Total de registros:** {dados.Count:N0}
- **Culturas analisadas:** {string.Join(", ", resumoGeral.Select(r => $"{r.Cultura} ({r.Registros} registros)"))}
- **Período:** {dados.Min(d => d.DataAvaliacao):dd/MM/yyyy} a {dados.Max(d => d.DataAvaliacao):dd/MM/yyyy}

## AMOSTRA DETALHADA DOS DADOS:
{FormatarAmostraDados(amostra)}

## ESTATÍSTICAS RESUMIDAS:
- **Nível de dano médio:** {dados.Average(d => d.NivelDano):F1} (escala 1-5)
- **Percentual de plantas infestadas médio:** {dados.Average(d => d.PercentualPlantasInfestadas):F1}%
- **Impacto na produção estimado médio:** {dados.Average(d => d.ImpactoProducaoEstimado):F1}%

{(!string.IsNullOrEmpty(instrucoesEspecificas) ? $@"
## INSTRUÇÕES ESPECÍFICAS:
{instrucoesEspecificas}
" : "")}

## SOLICITAÇÃO DE ANÁLISE:

Por favor, analise estes dados de amostragem e forneça:

### 1. IDENTIFICAÇÃO DA PRAGA PRINCIPAL
- Nome popular e científico
- Descrição dos danos observados
- Confiança na identificação (0-100%)

### 2. AVALIAÇÃO DE RISCO
- Nível de infestação (Baixo/Médio/Alto/Crítico)
- Percentual de risco para a produção
- Fatores que contribuem para o risco

### 3. RECOMENDAÇÕES DE MANEJO
- Métodos de controle recomendados (químico, biológico, cultural)
- Produtos específicos sugeridos
- Urgência das ações
- Melhor época para aplicação

### 4. INSIGHTS ADICIONAIS
- Padrões observados nos dados
- Correlações importantes
- Recomendações preventivas

**FORMATO DE RESPOSTA:** 
Estruture sua resposta de forma clara e prática, priorizando informações acionáveis para o produtor rural.
";

            return prompt;
        }

        private string FormatarAmostraDados(List<AmostragemPragasCsvModel> amostra)
        {
            var resultado = new System.Text.StringBuilder();
            
            foreach (var item in amostra)
            {
                resultado.AppendLine($"**Registro {item.Id}:**");
                resultado.AppendLine($"- Cultura: {item.NomeCultura} ({item.VariedadeHibrido})");
                resultado.AppendLine($"- Local: {item.Propriedade} - {item.Talhao}");
                resultado.AppendLine($"- Data: {item.DataAvaliacao:dd/MM/yyyy}");
                resultado.AppendLine($"- Condições: {item.CondicoesClimaticas}, {item.TemperaturaGraus}°C, {item.UmidadePercentual}% umidade");
                resultado.AppendLine($"- Praga identificada: {item.NomeComum} ({item.NomeCientifico})");
                resultado.AppendLine($"- Estádio: {item.EstadioDesenvolvimento}");
                resultado.AppendLine($"- Nível de dano: {item.NivelDano}/5");
                resultado.AppendLine($"- Plantas infestadas: {item.PercentualPlantasInfestadas}%");
                resultado.AppendLine($"- Impacto estimado na produção: {item.ImpactoProducaoEstimado}%");
                resultado.AppendLine($"- Sintomas: {item.SintomasSecundarios}");
                resultado.AppendLine();
            }
            
            return resultado.ToString();
        }

        private AnaliseIAResponse ProcessarRespostaIA(string conteudo)
        {
            var resposta = new AnaliseIAResponse();

            try
            {
                // Extrair informações estruturadas do texto
                resposta.Praga = ExtrairIdentificacaoPraga(conteudo);
                resposta.Recomendacoes = ExtrairRecomendacoes(conteudo);
                resposta.Risco = ExtrairAnaliseRisco(conteudo);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Erro ao processar resposta estruturada: {ex.Message}");
                // Continua com análise em texto mesmo se a extração estruturada falhar
            }

            return resposta;
        }

        private IdentificacaoPraga? ExtrairIdentificacaoPraga(string conteudo)
        {
            try
            {
                var praga = new IdentificacaoPraga();
                
                // Buscar padrões no texto para extrair informações
                var linhas = conteudo.Split('\n');
                
                foreach (var linha in linhas)
                {
                    var linhaLower = linha.ToLower();
                    
                    if (linhaLower.Contains("nome popular") || linhaLower.Contains("praga principal"))
                    {
                        praga.NomePopular = ExtrairValorDaLinha(linha);
                    }
                    else if (linhaLower.Contains("nome científico") || linhaLower.Contains("científico"))
                    {
                        praga.NomeCientifico = ExtrairValorDaLinha(linha);
                    }
                    else if (linhaLower.Contains("confiança") && linhaLower.Contains("%"))
                    {
                        var percentualStr = System.Text.RegularExpressions.Regex.Match(linha, @"(\d+)%").Groups[1].Value;
                        if (decimal.TryParse(percentualStr, out decimal confianca))
                        {
                            praga.ConfiancaIdentificacao = confianca;
                        }
                    }
                }

                return string.IsNullOrEmpty(praga.NomePopular) ? null : praga;
            }
            catch
            {
                return null;
            }
        }

        private List<RecomendacaoManejo> ExtrairRecomendacoes(string conteudo)
        {
            var recomendacoes = new List<RecomendacaoManejo>();
            
            try
            {
                // Buscar seções de recomendações
                var linhas = conteudo.Split('\n');
                bool dentroRecomendacoes = false;
                
                foreach (var linha in linhas)
                {
                    if (linha.ToLower().Contains("recomenda") || linha.ToLower().Contains("manejo"))
                    {
                        dentroRecomendacoes = true;
                        continue;
                    }
                    
                    if (dentroRecomendacoes && linha.StartsWith("-") || linha.StartsWith("•"))
                    {
                        var rec = new RecomendacaoManejo
                        {
                            Descricao = linha.TrimStart('-', '•', ' '),
                            Tipo = DeterminarTipoRecomendacao(linha)
                        };
                        recomendacoes.Add(rec);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Erro ao extrair recomendações: {ex.Message}");
            }
            
            return recomendacoes;
        }

        private AnaliseRisco? ExtrairAnaliseRisco(string conteudo)
        {
            try
            {
                var risco = new AnaliseRisco();
                
                // Buscar nível de infestação
                var linhas = conteudo.Split('\n');
                foreach (var linha in linhas)
                {
                    var linhaLower = linha.ToLower();
                    
                    if (linhaLower.Contains("nível") && (linhaLower.Contains("infestação") || linhaLower.Contains("risco")))
                    {
                        if (linhaLower.Contains("baixo")) risco.NivelInfestacao = "Baixo";
                        else if (linhaLower.Contains("médio")) risco.NivelInfestacao = "Médio";
                        else if (linhaLower.Contains("alto")) risco.NivelInfestacao = "Alto";
                        else if (linhaLower.Contains("crítico")) risco.NivelInfestacao = "Crítico";
                    }
                }
                
                return risco;
            }
            catch
            {
                return null;
            }
        }

        private string ExtrairValorDaLinha(string linha)
        {
            var partes = linha.Split(':', '-');
            return partes.Length > 1 ? partes[1].Trim() : string.Empty;
        }

        private string DeterminarTipoRecomendacao(string texto)
        {
            var textoLower = texto.ToLower();
            
            if (textoLower.Contains("inseticida") || textoLower.Contains("químico"))
                return "Químico";
            if (textoLower.Contains("biológico") || textoLower.Contains("predador"))
                return "Biológico";
            if (textoLower.Contains("cultural") || textoLower.Contains("plantio"))
                return "Cultural";
            
            return "Integrado";
        }
    }
}