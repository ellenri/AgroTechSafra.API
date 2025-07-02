using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroTechSafra.API.Data;
using AgroTechSafra.API.Models;

namespace AgroTechSafra.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmostragemController : ControllerBase
    {
        private readonly AgroTechSafraContext _context;
        private readonly ILogger<AmostragemController> _logger;

        public AmostragemController(AgroTechSafraContext context, ILogger<AmostragemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retorna estatísticas gerais das amostras
        /// </summary>
        [HttpGet("estatisticas")]
        public async Task<ActionResult> GetEstatisticas()
        {
            try
            {
                var avaliacoes = await _context.AvaliacoesPragas
                    .Include(a => a.DetalhesPragas)
                    .Include(a => a.InimigosNaturais)
                    .ToListAsync();

                if (!avaliacoes.Any())
                {
                    return Ok(new { mensagem = "Nenhuma avaliação encontrada" });
                }

                var estatisticas = new
                {
                    TotalAvaliacoes = avaliacoes.Count,
                    PorCultura = avaliacoes.GroupBy(a => a.NomeCultura)
                        .Select(g => new { Cultura = g.Key, Quantidade = g.Count() })
                        .OrderByDescending(x => x.Quantidade),
                    
                    MediaTemperatura = Math.Round(avaliacoes.Average(a => (double)a.TemperaturaGraus), 1),
                    MediaUmidade = Math.Round(avaliacoes.Average(a => (double)a.UmidadePercentual), 1),
                    
                    PeriodoAvaliacao = new
                    {
                        DataInicio = avaliacoes.Min(a => a.DataAvaliacao).ToString("dd/MM/yyyy"),
                        DataFim = avaliacoes.Max(a => a.DataAvaliacao).ToString("dd/MM/yyyy")
                    }
                };

                return Ok(estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar estatísticas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Retorna avaliações filtradas por cultura
        /// </summary>
        [HttpGet("cultura/{cultura}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoPraga>>> GetByCultura(string cultura)
        {
            try
            {
                var avaliacoes = await _context.AvaliacoesPragas
                    .Include(a => a.DetalhesPragas)
                    .Include(a => a.InimigosNaturais)
                    .Where(a => a.NomeCultura.ToLower().Contains(cultura.ToLower()))
                    .ToListAsync();

                if (!avaliacoes.Any())
                {
                    return NotFound($"Nenhuma avaliação encontrada para a cultura: {cultura}");
                }

                return Ok(avaliacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar avaliações por cultura: {cultura}");
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}