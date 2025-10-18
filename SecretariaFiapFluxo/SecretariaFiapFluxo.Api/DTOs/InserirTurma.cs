using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class InserirTurma
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(250, MinimumLength = 10)]
        public string Descricao { get; set; } = null!;
    }
}
