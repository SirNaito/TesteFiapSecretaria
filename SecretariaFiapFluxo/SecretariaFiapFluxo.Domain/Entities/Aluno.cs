using System;
using System.Collections.Generic;

namespace SecretariaFiapFluxo.Domain.Entities
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;

        // Relacionamentos
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
