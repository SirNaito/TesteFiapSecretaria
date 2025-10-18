using System.Collections.Generic;

namespace SecretariaFiapFluxo.Domain.Entities
{
    public class Turma
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;

        // Relacionamentos
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
