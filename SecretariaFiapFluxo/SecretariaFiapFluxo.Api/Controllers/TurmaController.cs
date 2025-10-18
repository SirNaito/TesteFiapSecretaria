using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaFiapFluxo.Api.DTOs;
using SecretariaFiapFluxo.Api.Helpers;
using SecretariaFiapFluxo.Api.Validators;
using SecretariaFiapFluxo.Domain.Entities;
using SecretariaFiapFluxo.Infrastructure.Models;

namespace SecretariaFiapFluxo.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TurmaController : ControllerBase
    {
        private readonly SecretariaContext _context;

        public TurmaController(SecretariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1)
        {
            var query = _context.Turmas
                                .Include(t => t.Matriculas)
                                .OrderBy(t => t.Nome)
                                .AsQueryable();

            var turmas = await PaginationHelper.Paginar(query, pagina).Select(t => new
            {
                t.Id,
                t.Nome,
                t.Descricao,
                QuantidadeAlunos = t.Matriculas.Count
            }).ToListAsync();

            return Ok(turmas);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] InserirTurma dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || dto.Nome.Length < 3 || dto.Nome.Length > 100)
                return BadRequest(new { message = "Nome inválido." });

            if (string.IsNullOrWhiteSpace(dto.Descricao) || dto.Descricao.Length < 10 || dto.Descricao.Length > 250)
                return BadRequest(new { message = "Descrição inválida." });

            if (await _context.Turmas.AnyAsync(t => t.Nome == dto.Nome))
                return BadRequest(new { message = "Turma já existe." });

            var turma = new Turma
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao
            };

            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Listar), new { id = turma.Id }, turma);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTurma dto)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Nome) || dto.Nome.Length < 3 || dto.Nome.Length > 100)
                return BadRequest(new { message = "Nome inválido." });

            if (string.IsNullOrWhiteSpace(dto.Descricao) || dto.Descricao.Length < 10 || dto.Descricao.Length > 250)
                return BadRequest(new { message = "Descrição inválida." });

            turma.Nome = dto.Nome;
            turma.Descricao = dto.Descricao;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null) return NotFound();

            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
