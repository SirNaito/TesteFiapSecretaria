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
    public class AdminController : ControllerBase
    {
        private readonly SecretariaContext _context;

        public AdminController(SecretariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1)
        {
            var query = _context.Admins.OrderBy(a => a.Nome).AsQueryable();
            var admins = await PaginationHelper.Paginar(query, pagina).ToListAsync();
            return Ok(admins);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] InserirAdmin dto)
        {
            if (!EmailValidator.Validar(dto.Email))
                return BadRequest(new { message = "Email inválido." });
            if (!SenhaValidator.Validar(dto.Senha))
                return BadRequest(new { message = "Senha fora dos critérios." });

            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email))
                return BadRequest(new { message = "Admin já cadastrado." });

            var admin = new Admin
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = PasswordHelper.HashPassword(dto.Senha)
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Listar), new { id = admin.Id }, admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarAdmin dto)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            if (!EmailValidator.Validar(dto.Email))
                return BadRequest(new { message = "Email inválido." });

            admin.Nome = dto.Nome;
            admin.Email = dto.Email;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/senha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] AtualizarSenhaAdmins dto)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            if (!PasswordHelper.VerifyPassword(dto.SenhaAtual, admin.Senha))
                return BadRequest(new { message = "Senha atual incorreta." });

            if (!SenhaValidator.Validar(dto.NovaSenha))
                return BadRequest(new { message = "Nova senha fora dos critérios." });

            admin.Senha = PasswordHelper.HashPassword(dto.NovaSenha);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
