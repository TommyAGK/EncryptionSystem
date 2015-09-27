using System;
using System.IO;
using System.Security.Cryptography;

namespace FTPES
{
    internal class TripleTCrypto : IDisposable // TommyTwoToesCrypto ^___^
    {
        private SymmetricAlgorithm algo;

        public TripleTCrypto(SymmetricAlgorithm algorithm, string passPhrase) //constructor
        {
            initialiseAlgo(algorithm, passPhrase);
        }
        /// <summary>
		/// Method for getting your string encrypted based on the key
		/// from the constructor of this object.
		/// </summary>
		/// <param name="plainText">The string you want encrypted</param>
		/// <returns></returns>
		public byte[] ApplyEncryption(string plainText)
        {
            return EncryptString(plainText); //encrypts and returns it as a string
        }

        /// <summary>
        /// Method for getting your string returned to plain text
        /// </summary>
        /// <param name="cipherData">Your encrypted data in string format</param>
        /// <returns></returns>
        public string ApplyDecryption(byte[] cipherData)
        {
            return DecryptData(cipherData);
        }

        private string DecryptData(byte[] cipherData)
        {
            string plainData;
            //byte[] plainData = decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length);
            using (MemoryStream msDecrypt = new MemoryStream(cipherData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, algo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plainData = srDecrypt.ReadLine();
                    }
                }
            }

            return plainData;
        } //gonna use when iv/key has been created properly

        //private byte[] DecryptData(byte[] cipherData)
        //{
        //	SymmetricAlgorithm cryptoAlgorythm = SymmetricAlgorithm.Create();
        //	ICryptoTransform decryptor = cryptoAlgorythm.CreateDecryptor();

        //	byte[] plainData = decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length);
        //	return plainData;
        //}

        private byte[] EncryptString(string plainData)
        {

            byte[] cipherData;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.WriteLine(plainData);
                    }
                }

                cipherData = ms.ToArray();
            }
            return cipherData;
        }

        //private byte[] EncryptString(string plainData)
        //{
        //	SymmetricAlgorithm cryptoAlgorythm = SymmetricAlgorithm.Create();
        //	ICryptoTransform encryptor = cryptoAlgorythm.CreateEncryptor();
        //	byte[] cipherData = new byte[0];

        //	using (MemoryStream msEncrypt = new MemoryStream())
        //		{
        //			using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //				{
        //					StreamWriter swEncrypt = new StreamWriter(csEncrypt);
        //					swEncrypt.Write(plainData);
        //					swEncrypt.Close();
        //					csEncrypt.Clear();

        //					cipherData = msEncrypt.ToArray();
        //				}
        //		}
        //	return cipherData;
        //}

        private void initialiseAlgo(SymmetricAlgorithm algorithm, string password)
        {
            const int keyBitLength = 64; //We want our key to be 64 bits
            const int iterations = 427; //arbitrary number... 

            var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; //not secure.
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] nyerh = rfc2898DeriveBytes.GetBytes(keyBitLength / 8);
                //bytes = bits / 8 so...
                algorithm.Key = rfc2898DeriveBytes.GetBytes(keyBitLength / 8);
                algorithm.IV = rfc2898DeriveBytes.GetBytes(algorithm.BlockSize / 8);
                algo = algorithm; //Set the class's algorithm to this initialised algorithm.
            }
        }

        public void Dispose()
        {
            algo.Dispose();
        }
    }
}
