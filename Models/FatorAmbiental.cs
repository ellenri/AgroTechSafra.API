namespace AgroTechSafra.API.Models
{
    public class FatorAmbiental
    {
        public int Id { get; set; }
        public int AvaliacaoPragaId { get; set; }
        public string TipoIrrigacao { get; set; } = string.Empty;
        public string FrequenciaIrrigacao { get; set; } = string.Empty;
        public DateTime? UltimaAdubacao { get; set; }
        public string TipoAdubo { get; set; } = string.Empty;
        public string PlantasDaninhasPresentes { get; set; } = string.Empty;
        public string CulturasVizinhas { get; set; } = string.Empty;
        public string BarreirasNaturais { get; set; } = string.Empty;
        public string OutrosFatores { get; set; } = string.Empty;

        // Navegação
        public AvaliacaoPraga? AvaliacaoPraga { get; set; }
    }
}