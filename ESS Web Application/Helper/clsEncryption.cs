using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ESS_Web_Application.Helper
{
    public class clsEncryption
    {
        public clsEncryption()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private static byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

        public static string EncryptData(string inName)
        {
            byte[] result = null;

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            UTF8Encoding m_utf8 = new UTF8Encoding();
            byte[] input = m_utf8.GetBytes(inName);

            MemoryStream memStream = new MemoryStream();

            CryptoStream cryptStream = new CryptoStream(memStream, tdes.CreateEncryptor(key, iv), CryptoStreamMode.Write);

            // transform the bytes as requested

            cryptStream.Write(input, 0, input.Length);

            cryptStream.FlushFinalBlock();

            // Read the memory stream and

            // convert it back into byte array

            memStream.Position = 0;

            //Dim result as byte() = memStream.ToArray()

            result = memStream.ToArray();

            // close and release the streams

            memStream.Close();

            cryptStream.Close();

            // hand back the encrypted buffer

            return Convert.ToBase64String(result);
        }

        public static string DecryptData(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData))
                return "";

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            UTF8Encoding m_utf8 = new UTF8Encoding();

            byte[] input = Convert.FromBase64String(encryptedData);

            MemoryStream memStream = new MemoryStream();

            CryptoStream cryptStream = new CryptoStream(memStream, tdes.CreateDecryptor(key, iv), CryptoStreamMode.Write);

            // transform the bytes as requested

            cryptStream.Write(input, 0, input.Length);

            cryptStream.FlushFinalBlock();

            // Read the memory stream and

            // convert it back into byte array

            memStream.Position = 0;

            byte[] result = memStream.ToArray();

            // close and release the streams

            memStream.Close();

            cryptStream.Close();

            // hand back the encrypted buffer

            return m_utf8.GetString(result);

        }
    }
}