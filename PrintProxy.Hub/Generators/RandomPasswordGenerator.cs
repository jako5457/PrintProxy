using System.Security.Cryptography;
using System.Text;

namespace PrintProxy.Hub.Generators
{
    public static class RandomPasswordGenerator
    {

        private static readonly char[] Alpha = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '@', '#', '*' };

        public static string GeneratePassword(int length)
        {
            StringBuilder builder = new();

            for (int i = 0; i < length; i++)
            {
                int index = RandomNumberGenerator.GetInt32(Alpha.Length);

                builder.Append(Alpha[index]);
            }

            return builder.ToString();
        }
    }
}
