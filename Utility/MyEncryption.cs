using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public static class MyEncryption
    {
        public static string Encryption_sha256(string text)
        {
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] source = Encoding.Default.GetBytes(text);
                byte[] crypto = sha256.ComputeHash(source);
                return Convert.ToBase64String(crypto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Utiliry.MyEncryption Encryption_sha256 Is Error:${ex.Message}");
            }
        }
    }
}
