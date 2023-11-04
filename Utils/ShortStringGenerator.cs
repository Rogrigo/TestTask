using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace TestTask.Utils
{
    public class ShortStringGenerator
    {
        public static string GenerateShortUrl()
        {
            string url;
            // TODO add checking do we have a short url in the DB or not
            // do
            // {
                url = GenerateRandomAlphanumericString();
            // } while (Models.Urls.Any(u => u.ShortUrl == url));

            return url;
        }

    private static string GenerateRandomAlphanumericString()
    {
        const int length = 8;
        const string alphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder sb = new StringBuilder();

        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(alphanumericChars.Length);
            sb.Append(alphanumericChars[index]);
        }

        return sb.ToString();
    }
    }
}
