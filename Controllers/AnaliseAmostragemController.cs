using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using AgroTechSafra.API.Models;
using AgroTechSafra.API.Services;

namespace AgroTechSafra.API.Controllers
{
    [ApiController]
    [Route("api/analiseamostragem")]
    public class AnaliseAmostragemController : ControllerBase
    {
        private readonly IAnaliseIAService _analiseIAService;
        private readonly ILogger<AnaliseAmostragemController> _logger;

        public AnaliseAmostragemController(
            IAnaliseIAService analiseIAService,
            ILogger<AnaliseAmostragemController> logger)
        {
            _analiseIAService = analiseIAService;
            _logger = logger;
        }

        /// <summary>
        /// Analisa arquivo CSV de amostragem de pragas usando IA
        /// </summary>
        /// <param name="request">Dados da requisição incluindo arquivo CSV</param>
        /// <returns>Análise detalhada da praga identificada</returns>
        [HttpPost("analisar")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10MB máximo
        public async Task<ActionResult<AnaliseIAResponse>> AnalisarPragas([FromForm] RequestAnalise request)
        {
            try
            {
                // Validações
                if (request.Arquivo == null || request.Arquivo.Length == 0)
                {
                    return BadRequest(new { erro = "Arquivo CSV é obrigatório" });
                }

                if (!request.Arquivo.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { erro = "Apenas arquivos CSV são aceitos" });
                }

                _logger.LogInformation($"Iniciando análise do arquivo: {request.Arquivo.FileName}");

                // Processar arquivo CSV
                var dadosCSV = await ProcessarArquivoCSV(request.Arquivo, request.MaximoRegistros);

                if (!dadosCSV.Any())
                {
                    return BadRequest(new { erro = "Arquivo CSV vazio ou sem dados válidos" });
                }

                // Enviar para análise de IA
                var resultado = await _analiseIAService.AnalisarPragasAsync(dadosCSV, request.InstrucoesEspecificas);

                if (!resultado.Sucesso)
                {
                    return StatusCode(500, new { 
                        erro = "Erro ao processar com IA", 
                        detalhes = resultado.MensagemErro 
                    });
                }

                _logger.LogInformation($"Análise concluída para: {request.Arquivo.FileName}");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao analisar CSV: {request.Arquivo?.FileName}");
                return StatusCode(500, new { 
                    erro = "Erro interno do servidor", 
                    detalhes = ex.Message 
                });
            }
        }

        /// <summary>
        /// Endpoint legado para compatibilidade (mantém funcionamento anterior)
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var request = new RequestAnalise 
            { 
                Arquivo = file,
                MaximoRegistros = 1000
            };

            var resultado = await AnalisarPragas(request);
            if (resultado.Result != null)
                return resultado.Result;
            return Ok(resultado.Value!);
        }

        /// <summary>
        /// Processa apenas o CSV sem enviar para IA (para debug)
        /// </summary>
        [HttpPost("processar-csv")]
        public async Task<ActionResult> ProcessarCSV(IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return BadRequest("Arquivo é obrigatório");

                var dados = await ProcessarArquivoCSV(arquivo, 100);
                
                return Ok(new
                {
                    nomeArquivo = arquivo.FileName,
                    totalRegistros = dados.Count,
                    amostra = dados.Take(3).Select(d => new {
                        cultura = d.NomeCultura,
                        praga = d.NomeComum,
                        nivelDano = d.NivelDano,
                        percentualInfestacao = d.PercentualPlantasInfestadas
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Testa a conexão com OpenAI
        /// </summary>
        [HttpGet("testar-ia")]
        public async Task<ActionResult> TestarIA()
        {
            try
            {
                var dadosTeste = new List<AmostragemPragasCsvModel>
                {
                    new AmostragemPragasCsvModel
                    {
                        Id = 1,
                        NomeCultura = "Soja",
                        NomeComum = "Lagarta-da-soja",
                        NomeCientifico = "Anticarsia gemmatalis",
                        NivelDano = 3,
                        PercentualPlantasInfestadas = 25,
                        DataAvaliacao = DateTime.Now
                    }
                };

                var resultado = await _analiseIAService.AnalisarPragasAsync(dadosTeste, "Teste de conexão");

                return Ok(new
                {
                    conectado = resultado.Sucesso,
                    modelo = resultado.Metricas?.ModeloIA,
                    tokens = resultado.Metricas?.TokensUtilizados,
                    tempo = resultado.Metricas?.TempoProcessamento,
                    erro = resultado.MensagemErro
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    conectado = false,
                    erro = ex.Message
                });
            }
        }

        private async Task<List<AmostragemPragasCsvModel>> ProcessarArquivoCSV(IFormFile arquivo, int maxRegistros)
        {
            var dados = new List<AmostragemPragasCsvModel>();

            using var reader = new StreamReader(arquivo.OpenReadStream(), Encoding.UTF8);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            });

            var records = csv.GetRecordsAsync<AmostragemPragasCsvModel>();
            var contador = 0;

            await foreach (var record in records)
            {
                if (contador >= maxRegistros) break;
                
                dados.Add(record);
                contador++;
            }

            return dados;
        }
    }
}