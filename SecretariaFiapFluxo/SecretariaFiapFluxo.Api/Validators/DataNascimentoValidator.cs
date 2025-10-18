using System;

namespace SecretariaFiapFluxo.Api.Validators
{
    public static class DataNascimentoValidator
    {
        public static bool Validar(DateTime dataNascimento)
        {
            var hoje = DateTime.UtcNow.Date;

            // Data Futura
            if (dataNascimento >= hoje) return false;

            // Calculo de Idade
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento > hoje.AddYears(-idade)) idade--;

            // Idade entre 17 e 100 anos
            if (idade < 17 || idade > 100) return false;

            return true;
        }
    }
}
