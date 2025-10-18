using System.Text.RegularExpressions;

namespace SecretariaFiapFluxo.Api.Validators
{
    public static class SenhaValidator
    {
        public static bool Validar(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
                return false;

            // Pelo menos 1 letra maiúscula, 1 minúscula, 1 número e 1 símbolo especial
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$");
            return regex.IsMatch(senha);
        }
    }
}
