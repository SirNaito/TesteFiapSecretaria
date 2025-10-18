using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaFiapFluxo.Api.DTOs;
using SecretariaFiapFluxo.Domain.Entities;
using SecretariaFiapFluxo.Infrastructure.Models;

namespace SecretariaFiapFluxo.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ControllerBase
    {
        private readonly SecretariaContext _context;

        public MatriculaController(SecretariaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Matricular([FromBody] InserirMatricula dto)
        {
            if (!await _context.Alunos.AnyAsync(a => a.Id == dto.AlunoId))
                return BadRequest(new { message = "Aluno não encontrado." });

            if (!await _context.Turmas.AnyAsync(t => t.Id == dto.TurmaId))
                return BadRequest(new { message = "Turma não encontrada." });

            if (await _context.Matriculas.AnyAsync(m => m.AlunoId == dto.AlunoId && m.TurmaId == dto.TurmaId))
                return BadRequest(new { message = "Aluno já matriculado nesta turma." });

            var matricula = new Matricula
            {
                AlunoId = dto.AlunoId,
                TurmaId = dto.TurmaId
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ListarAlunosPorTurma), new { turmaId = dto.TurmaId }, matricula);
        }

        [HttpGet("turma/{turmaId}")]
        public async Task<IActionResult> ListarAlunosPorTurma(int turmaId)
        {
            var turma = await _context.Turmas
                                      .Include(t => t.Matriculas)
                                      .ThenInclude(m => m.Aluno)
                                      .FirstOrDefaultAsync(t => t.Id == turmaId);
            if (turma == null) return NotFound();

            var alunos = turma.Matriculas.Select(m => new
            {
                m.Aluno.Id,
                m.Aluno.Nome,
                m.Aluno.Email,
                m.Aluno.CPF,
                m.Aluno.DataNascimento
            });

            return Ok(alunos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desmatricular(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null) return NotFound();

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
