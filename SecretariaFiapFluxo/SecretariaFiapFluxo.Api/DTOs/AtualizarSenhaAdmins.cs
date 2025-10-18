using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class AtualizarSenhaAdmins
    {
        [Required]
        public string SenhaAtual { get; set; } = null!;

        [Required, MinLength(8)]
        public string NovaSenha { get; set; } = null!;
    }
}
