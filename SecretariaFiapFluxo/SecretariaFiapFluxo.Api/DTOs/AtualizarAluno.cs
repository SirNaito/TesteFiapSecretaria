using System;
using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class AtualizarAluno
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [Required, DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string CPF { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
    }
}
