using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class AtualizarAdmin
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
    }
}
