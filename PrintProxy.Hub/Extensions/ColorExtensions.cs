using System.Security.Cryptography;
using System.Text;

namespace PrintProxy.Hub.Extensions
{
    public static class ColorExtensions
    {
        public static string GenerateHexColor(this string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);

            using SHA256 sha = SHA256.Create();

            var HashBytes = sha.ComputeHash(bytes);

            byte r = HashBytes[1];
            byte g = HashBytes[6];
            byte b = HashBytes[10];

            return $"#{r.ToString("X2")}{g.ToString("X2")}{b.ToString("X2")}";
        }

    }
}
