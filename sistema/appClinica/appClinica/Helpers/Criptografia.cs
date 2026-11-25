using System.Security.Cryptography;
using System.Text;

namespace appClinica.Helpers
{
    public static class Criptografia
    {
        public static string HashSha256(string valor)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(valor));
            return Convert.ToHexString(bytes);
        }
    }
}