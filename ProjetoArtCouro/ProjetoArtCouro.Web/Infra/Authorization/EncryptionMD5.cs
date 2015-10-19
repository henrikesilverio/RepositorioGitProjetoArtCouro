using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoArtCouro.Web.Infra.Authorization
{
    public static class EncryptionMD5
    {
        private const string CryptoKey = "cryptoKey";

        // The Initialization Vector for the DES encryption routine
        private static readonly byte[] Iv = { 240, 3, 45, 29, 0, 76, 173, 59 };

        /// <summary>
        /// Encrypts provided string parameter
        /// </summary>
        public static string Encrypt(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var buffer = Encoding.ASCII.GetBytes(s);
            var des = new TripleDESCryptoServiceProvider();
            var md5 = new MD5CryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(CryptoKey));
            des.IV = Iv;
            var result = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }

        /// <summary>
        /// Decrypts provided string parameter
        /// </summary>
        public static string Decrypt(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var buffer = Convert.FromBase64String(s);
            var des = new TripleDESCryptoServiceProvider();
            var md5 = new MD5CryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(CryptoKey));
            des.IV = Iv;
            var result = Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }
    }
}