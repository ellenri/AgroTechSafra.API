namespace AgroTechSafra.API.Models
{
    public class DetalhePraga
    {
        public int Id { get; set; }
        public int AvaliacaoPragaId { get; set; }
        public string NomeCientifico { get; set; } = string.Empty;
        public string NomeComum { get; set; } = string.Empty;
        public string EstadioDesenvolvimento { get; set; } = string.Empty;
        public int NumeroIndividuosPorPlanta { get; set; }
        public int PlantasAtacadasPorPonto { get; set; }
        public decimal PercentualPlantasInfestadas { get; set; }
        public int NivelDano { get; set; } // Escala 1-5
        public decimal PercentualDano { get; set; }
        public string DistribuicaoNaPlanta { get; set; } = string.Empty;
        public string TipoDano { get; set; } = string.Empty;
        public decimal SeveridadeDano { get; set; }
        public decimal ImpactoProducaoEstimado { get; set; }
        public string SintomasSecundarios { get; set; } = string.Empty;

        // Navegação
        public AvaliacaoPraga? AvaliacaoPraga { get; set; }
    }
}