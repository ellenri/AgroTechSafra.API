namespace AgroTechSafra.API.Models
{
    public class RecomendacaoControle
    {
        public int Id { get; set; }
        public int AvaliacaoPragaId { get; set; }
        public string NivelInfestacao { get; set; } = string.Empty; // Baixo, Médio, Alto, Crítico
        public bool NecessidadeControle { get; set; }
        public string MetodoControleRecomendado { get; set; } = string.Empty;
        public DateTime? PrazoReavaliacao { get; set; }
        public string ObservacoesGerais { get; set; } = string.Empty;
        public string TipoControle { get; set; } = string.Empty; // Químico, Biológico, Cultural, Integrado
        public string ProdutoRecomendado { get; set; } = string.Empty;
        public decimal? DosagemRecomendada { get; set; }

        // Navegação
        public AvaliacaoPraga? AvaliacaoPraga { get; set; }
    }
}