using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.DTOs
{
    public class AtualizarMatricula
    {
        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int TurmaId { get; set; }
    }
}
