using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroTechSafra.API.Data;
using AgroTechSafra.API.Models;

namespace AgroTechSafra.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacoesPragasController : ControllerBase
    {
        private readonly AgroTechSafraContext _context;

        public AvaliacoesPragasController(AgroTechSafraContext context)
        {
            _context = context;
        }

        // GET: api/AvaliacoesPragas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvaliacaoPraga>>> GetAvaliacoesPragas()
        {
            return await _context.AvaliacoesPragas
                .Include(a => a.DetalhesPragas)
                .Include(a => a.InimigosNaturais)
                .ToListAsync();
        }

        // GET: api/AvaliacoesPragas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AvaliacaoPraga>> GetAvaliacaoPraga(int id)
        {
            var avaliacaoPraga = await _context.AvaliacoesPragas
                .Include(a => a.DetalhesPragas)
                .Include(a => a.InimigosNaturais)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacaoPraga == null)
            {
                return NotFound();
            }

            return avaliacaoPraga;
        }        // POST: api/AvaliacoesPragas
        [HttpPost]
        public async Task<ActionResult<AvaliacaoPraga>> PostAvaliacaoPraga(AvaliacaoPraga avaliacaoPraga)
        {
            _context.AvaliacoesPragas.Add(avaliacaoPraga);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAvaliacaoPraga", new { id = avaliacaoPraga.Id }, avaliacaoPraga);
        }

        // PUT: api/AvaliacoesPragas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacaoPraga(int id, AvaliacaoPraga avaliacaoPraga)
        {
            if (id != avaliacaoPraga.Id)
            {
                return BadRequest();
            }

            _context.Entry(avaliacaoPraga).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoPragaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/AvaliacoesPragas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvaliacaoPraga(int id)
        {
            var avaliacaoPraga = await _context.AvaliacoesPragas.FindAsync(id);
            if (avaliacaoPraga == null)
            {
                return NotFound();
            }

            _context.AvaliacoesPragas.Remove(avaliacaoPraga);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AvaliacaoPragaExists(int id)
        {
            return _context.AvaliacoesPragas.Any(e => e.Id == id);
        }
    }
}