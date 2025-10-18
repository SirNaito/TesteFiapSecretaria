using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class InserirAdmin
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(8)]
        public string Senha { get; set; } = null!;
    }
}
