using Microsoft.AspNetCore.Identity;

namespace SecretariaFiapFluxo.Api.Helpers
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<object> _hasher = new();

        public static string HashPassword(string senha)
        {
            return _hasher.HashPassword(null!, senha);
        }

        public static bool VerifyPassword(string senha, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null!, hash, senha);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
