namespace AgroTechSafra.API.Models
{
    public class InimigoNatural
    {
        public int Id { get; set; }
        public int AvaliacaoPragaId { get; set; }
        public string TipoInimigo { get; set; } = string.Empty; // Predador, Parasitoide
        public string NomeEspecie { get; set; } = string.Empty;
        public int QuantidadeEncontrada { get; set; }
        public bool SinaisParasitismo { get; set; }
        public decimal PercentualControleNatural { get; set; }
        public string Observacoes { get; set; } = string.Empty;

        // Navegação
        public AvaliacaoPraga? AvaliacaoPraga { get; set; }
    }
}