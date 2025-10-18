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
    public class AlunoController : ControllerBase
    {
        private readonly SecretariaContext _context;

        public AlunoController(SecretariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1)
        {
            var alunos = _context.Alunos
                                 .OrderBy(a => a.Nome)
                                 .AsQueryable();

            var paginado = PaginationHelper.Paginar(alunos, pagina);
            var result = await paginado.ToListAsync();

            return Ok(result);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? nome, [FromQuery] string? cpf)
        {
            var query = _context.Alunos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(a => a.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(cpf))
                query = query.Where(a => a.CPF == cpf);

            var result = await query.OrderBy(a => a.Nome).ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] InserirAluno dto)
        {
            if (!CpfValidator.Validar(dto.CPF))
                return BadRequest(new { message = "CPF inválido." });
            if (!EmailValidator.Validar(dto.Email))
                return BadRequest(new { message = "Email inválido." });
            if (!SenhaValidator.Validar(dto.Senha))
                return BadRequest(new { message = "Senha fora dos critérios." });
            if (!DataNascimentoValidator.Validar(dto.DataNascimento))
                return BadRequest(new { message = "Data de nascimento inválida." });

            if (await _context.Alunos.AnyAsync(a => a.CPF == dto.CPF || a.Email == dto.Email))
                return BadRequest(new { message = "Aluno já cadastrado." });

            var aluno = new Aluno
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Email = dto.Email,
                DataNascimento = dto.DataNascimento,
                Senha = PasswordHelper.HashPassword(dto.Senha)
            };

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Listar), new { id = aluno.Id }, aluno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarAluno dto)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            if (!EmailValidator.Validar(dto.Email))
                return BadRequest(new { message = "Email inválido." });

            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            aluno.DataNascimento = dto.DataNascimento;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/senha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] AtualizarSenhaAluno dto)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            if (!PasswordHelper.VerifyPassword(dto.SenhaAtual, aluno.Senha))
                return BadRequest(new { message = "Senha atual incorreta." });

            if (!SenhaValidator.Validar(dto.NovaSenha))
                return BadRequest(new { message = "Nova senha fora dos critérios." });

            aluno.Senha = PasswordHelper.HashPassword(dto.NovaSenha);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
