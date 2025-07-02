using CsvHelper.Configuration.Attributes;

namespace AgroTechSafra.API.Models
{
    public class AmostragemPragasCsvModel
    {
        [Name("Id")]
        public int Id { get; set; }

        [Name("AvaliacaoPragaId")]
        public int AvaliacaoPragaId { get; set; }

        [Name("DataAvaliacao")]
        public DateTime DataAvaliacao { get; set; }

        [Name("Propriedade")]
        public string Propriedade { get; set; } = string.Empty;

        [Name("Talhao")]
        public string Talhao { get; set; } = string.Empty;

        [Name("Latitude")]
        public double? Latitude { get; set; }

        [Name("Longitude")]
        public double? Longitude { get; set; }

        [Name("ResponsavelColeta")]
        public string ResponsavelColeta { get; set; } = string.Empty;

        [Name("CondicoesClimaticas")]
        public string CondicoesClimaticas { get; set; } = string.Empty;
        [Name("TemperaturaGraus")]
        public decimal TemperaturaGraus { get; set; }

        [Name("UmidadePercentual")]
        public decimal UmidadePercentual { get; set; }

        [Name("ChuvaUltimas24h")]
        public bool ChuvaUltimas24h { get; set; }

        [Name("NomeCultura")]
        public string NomeCultura { get; set; } = string.Empty;

        [Name("VariedadeHibrido")]
        public string VariedadeHibrido { get; set; } = string.Empty;

        [Name("DataPlantio")]
        public DateTime DataPlantio { get; set; }

        [Name("EstadioFenologico")]
        public string EstadioFenologico { get; set; } = string.Empty;

        [Name("DensidadePlantio")]
        public decimal DensidadePlantio { get; set; }

        [Name("SistemaManejo")]
        public string SistemaManejo { get; set; } = string.Empty;

        [Name("UltimasAplicacoes")]
        public string UltimasAplicacoes { get; set; } = string.Empty;

        [Name("MetodoAmostragem")]
        public string MetodoAmostragem { get; set; } = string.Empty;

        [Name("NumeroPontosAmostrados")]
        public int NumeroPontosAmostrados { get; set; }
        [Name("DistanciaEntrePontos")]
        public decimal DistanciaEntrePontos { get; set; }

        [Name("AreaTotalAvaliada")]
        public decimal AreaTotalAvaliada { get; set; }

        [Name("PlantasPorPonto")]
        public int PlantasPorPonto { get; set; }

        [Name("PartesDaPlanta")]
        public string PartesDaPlanta { get; set; } = string.Empty;

        [Name("DetalhePragaId")]
        public int DetalhePragaId { get; set; }

        [Name("NomeCientifico")]
        public string NomeCientifico { get; set; } = string.Empty;

        [Name("NomeComum")]
        public string NomeComum { get; set; } = string.Empty;

        [Name("EstadioDesenvolvimento")]
        public string EstadioDesenvolvimento { get; set; } = string.Empty;

        [Name("NumeroIndividuosPorPlanta")]
        public int NumeroIndividuosPorPlanta { get; set; }

        [Name("PlantasAtacadasPorPonto")]
        public int PlantasAtacadasPorPonto { get; set; }

        [Name("PercentualPlantasInfestadas")]
        public decimal PercentualPlantasInfestadas { get; set; }

        [Name("NivelDano")]
        public int NivelDano { get; set; }
        [Name("PercentualDano")]
        public decimal PercentualDano { get; set; }

        [Name("DistribuicaoNaPlanta")]
        public string DistribuicaoNaPlanta { get; set; } = string.Empty;

        [Name("TipoDano")]
        public string TipoDano { get; set; } = string.Empty;

        [Name("SeveridadeDano")]
        public decimal SeveridadeDano { get; set; }

        [Name("ImpactoProducaoEstimado")]
        public decimal ImpactoProducaoEstimado { get; set; }

        [Name("SintomasSecundarios")]
        public string SintomasSecundarios { get; set; } = string.Empty;
    }
}
