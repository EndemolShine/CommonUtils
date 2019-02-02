using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonUtilsX64
{
    public class Password
    {
        public static string Random(int passwordLength)
        {
            string password = string.Empty;
            char[] upperCase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] lowerCase = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] specialchars = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+' };
            var rRandom = new Random();

            for (var i = 0; i < passwordLength; i++)
            {
                switch (rRandom.Next(4))
                {
                    case 0:
                        password += upperCase[rRandom.Next(0, 25)];
                        break;
                    case 1:
                        password += lowerCase[rRandom.Next(0, 25)];
                        break;
                    case 2:
                        password += rRandom.Next(0, 9);
                        break;
                    case 3:
                        password += specialchars[rRandom.Next(0, 5)];
                        break;
                }
            }

            return password;
        }

        public static string Hash(string pPassword)
        {
            var hashTool = new SHA512Managed();
            var phraseAsByte = Encoding.UTF8.GetBytes(string.Concat(pPassword));
            var encryptedBytes = hashTool.ComputeHash(phraseAsByte);
            hashTool.Clear();
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
