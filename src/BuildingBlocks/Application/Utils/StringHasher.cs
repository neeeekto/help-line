using System.Security.Cryptography;
using System.Text;

namespace HelpLine.BuildingBlocks.Application.Utils
{
    public static class StringHasher
    {
        private static byte[] GetHash(string inputString)
        {
            using var algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string Make(string inputString)
        {
            var sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
