using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Redbox.HAL.Component.Model.Services
{
    public sealed class TripleDesEncryptionService : IEncryptionService
    {
        private readonly byte[] m_keyValue = new Guid("{776DA6AF-3033-43ee-B379-2D4F28B5F1FC}").ToByteArray();
        private readonly byte[] m_initialVector = new Guid("{F375D7E0-4572-4518-9C2F-E8F022F42AA7}").ToByteArray();

        public byte[] Encrypt(byte[] inputArray)
        {
            using (TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider())
                return this.DoTransform(cryptoServiceProvider.CreateEncryptor(this.m_keyValue, this.m_initialVector), inputArray);
        }

        public byte[] Decrypt(byte[] inputArray)
        {
            using (TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider())
                return this.DoTransform(cryptoServiceProvider.CreateDecryptor(this.m_keyValue, this.m_initialVector), inputArray);
        }

        public string EncryptToBase64(string source)
        {
            return Convert.ToBase64String(this.Encrypt(Encoding.UTF8.GetBytes(source)));
        }

        public string DecryptFromBase64(string source)
        {
            return Encoding.UTF8.GetString(this.Decrypt(Convert.FromBase64String(source)));
        }

        public string HashFile(string fullFilePath)
        {
            using (SHA1CryptoServiceProvider algorithm = new SHA1CryptoServiceProvider())
                return this.HashFile((HashAlgorithm)algorithm, fullFilePath);
        }

        public string HashFile(HashAlgorithm algorithm, string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
                throw new ArgumentException("fullFilePath doesn't exist.");
            byte[] numArray = (byte[])null;
            using (FileStream inputStream = new FileStream(fullFilePath, FileMode.Open))
                numArray = algorithm.ComputeHash((Stream)inputStream);
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < numArray.Length; ++index)
                stringBuilder.Append(numArray[index].ToString("x2"));
            return stringBuilder.ToString();
        }

        private byte[] DoTransform(ICryptoTransform transform, byte[] inputArray)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(inputArray, 0, inputArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
}
