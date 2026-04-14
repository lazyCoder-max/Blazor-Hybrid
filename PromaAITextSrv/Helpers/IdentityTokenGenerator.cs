using System.Security.Cryptography;
using System.Text;

namespace PromaAITextSrv.Helpers
{
    public static class IdentityTokenGenerator
    {
        public static string Generate(string identitySecret)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var keyBytes = Encoding.UTF8.GetBytes(identitySecret);
            var messageBytes = Encoding.UTF8.GetBytes(timestamp);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            var hashHex = Convert.ToHexString(hashBytes).ToLower();

            return $"{timestamp}.{hashHex}";
        }
    }
}