using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretariaFiapFluxo.Api.DTOs;
using SecretariaFiapFluxo.Api.Helpers;
using SecretariaFiapFluxo.Infrastructure.Models;

namespace SecretariaFiapFluxo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SecretariaContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SecretariaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AutenticarUser dto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email);
            if (admin == null || !PasswordHelper.VerifyPassword(dto.Senha, admin.Senha))
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = JwtHelper.GenerateToken(admin.Id, admin.Email, _configuration["Jwt:SecretKey"]!);
            return Ok(new { token });
        }
    }
}
