using System.ComponentModel.DataAnnotations;

namespace SecretariaFiapFluxo.Api.Validators
{
    public static class EmailValidator
    {
        public static bool Validar(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var emailAttr = new EmailAddressAttribute();
            return emailAttr.IsValid(email);
        }
    }
}
