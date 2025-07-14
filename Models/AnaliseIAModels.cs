namespace AgroTechSafra.API.Models
{
    public class AnaliseIAResponse
    {
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }
        public IdentificacaoPraga? Praga { get; set; }
        public List<RecomendacaoManejo> Recomendacoes { get; set; } = new();
        public AnaliseRisco? Risco { get; set; }
        public string AnaliseCompleta { get; set; } = string.Empty;
        public MetricasProcessamento? Metricas { get; set; }
    }

    public class IdentificacaoPraga
    {
        public string NomePopular { get; set; } = string.Empty;
        public string NomeCientifico { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string TipoDano { get; set; } = string.Empty;
        public List<string> SintomasCaracteristicos { get; set; } = new();
        public decimal ConfiancaIdentificacao { get; set; } // 0-100%
    }

    public class RecomendacaoManejo
    {
        public string Tipo { get; set; } = string.Empty; // Químico, Biológico, Cultural, Integrado
        public string Descricao { get; set; } = string.Empty;
        public string Urgencia { get; set; } = string.Empty; // Baixa, Média, Alta, Crítica
        public List<string> ProdutosSugeridos { get; set; } = new();
        public string Dosagem { get; set; } = string.Empty;
        public string MelhorEpocaAplicacao { get; set; } = string.Empty;
    }

    public class AnaliseRisco
    {
        public string NivelInfestacao { get; set; } = string.Empty; // Baixo, Médio, Alto, Crítico
        public decimal PercentualRisco { get; set; } // 0-100%
        public string ImpactoEconomico { get; set; } = string.Empty;
        public List<string> FatoresRisco { get; set; } = new();
        public string TempoReacao { get; set; } = string.Empty; // Imediato, 7 dias, 15 dias, etc.
    }

    public class MetricasProcessamento
    {
        public int TotalRegistrosProcessados { get; set; }
        public TimeSpan TempoProcessamento { get; set; }
        public int TokensUtilizados { get; set; }
        public string ModeloIA { get; set; } = string.Empty;
        public DateTime ProcessadoEm { get; set; } = DateTime.UtcNow;
    }

    public class RequestAnalise
    {
        public IFormFile Arquivo { get; set; } = null!;
        public string? InstrucoesEspecificas { get; set; }
        public bool IncluirRecomendacoes { get; set; } = true;
        public bool IncluirAnaliseRisco { get; set; } = true;
        public int MaximoRegistros { get; set; } = 1000;
    }
}