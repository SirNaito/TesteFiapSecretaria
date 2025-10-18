using System.Text.RegularExpressions;

namespace SecretariaFiapFluxo.Api.Validators
{
    public static class CpfValidator
    {
        public static bool Validar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Deve ter 11 dígitos
            if (cpf.Length != 11) return false;

            return true;
        }
    }
}
