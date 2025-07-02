namespace AgroTechSafra.API.Models
{
    public class AvaliacaoPraga
    {
        public int Id { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string Propriedade { get; set; } = string.Empty;
        public string Talhao { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ResponsavelColeta { get; set; } = string.Empty;
        public string CondicoesClimaticas { get; set; } = string.Empty;
        public decimal TemperaturaGraus { get; set; }
        public decimal UmidadePercentual { get; set; }
        public bool ChuvaUltimas24h { get; set; }

        // Dados da Cultura
        public string NomeCultura { get; set; } = string.Empty;
        public string VariedadeHibrido { get; set; } = string.Empty;
        public DateTime DataPlantio { get; set; }
        public string EstadioFenologico { get; set; } = string.Empty;
        public decimal DensidadePlantio { get; set; }
        public string SistemaManejo { get; set; } = string.Empty;
        public string UltimasAplicacoes { get; set; } = string.Empty;

        // Metodologia
        public string MetodoAmostragem { get; set; } = string.Empty;
        public int NumeroPontosAmostrados { get; set; }
        public decimal DistanciaEntrePontos { get; set; }
        public decimal AreaTotalAvaliada { get; set; }
        public int PlantasPorPonto { get; set; }
        public string PartesDaPlanta { get; set; } = string.Empty;

        // Navegação
        public List<DetalhePraga> DetalhesPragas { get; set; } = new List<DetalhePraga>();
        public List<InimigoNatural> InimigosNaturais { get; set; } = new List<InimigoNatural>();
    }
}