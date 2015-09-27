﻿using System.IO;
using System.Security.Cryptography;

namespace FTPES
{
	internal class TripleTCrypto // TommyTwoToesCrypto ^___^
	{
		private string pass;

		//TODO: create system to automagically create IV/Key based on user input, right now its autogenerated

		public TripleTCrypto(string passPhrase)
		{
			pass = passPhrase;
		}

		/// <summary>
		/// Method for getting your string encrypted based on the key
		/// from the constructor of this object.
		/// </summary>
		/// <param name="plainText">The string you want encrypted</param>
		/// <returns></returns>
		public string ApplyEncryption(string plainText)
		{
			return GetString(EncryptString(plainText,GetBytes(pass))); //encrypts and returns it as a string
		}

		/// <summary>
		/// Method for getting your string returned to plain text
		/// </summary>
		/// <param name="cipherData">Your encrypted data in string format</param>
		/// <returns></returns>
		public string ApplyDecryption(string cipherData)
		{
			return GetString(DecryptData(GetBytes(cipherData),GetBytes(pass)));
		}


		private byte[] GetBytes(string input)
		{
			byte[] bytes = new byte[input.Length * sizeof (char)];
			System.Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof (char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		private byte[] DecryptData(byte[] cipherData, byte[] Key)
		{
			SymmetricAlgorithm cryptoAlgorythm = SymmetricAlgorithm.Create();
			cryptoAlgorythm.GenerateIV();
			ICryptoTransform decryptor = cryptoAlgorythm.CreateDecryptor(Key, cryptoAlgorythm.IV);
			byte[] plainData;
			//byte[] plainData = decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length);
			 using (MemoryStream msDecrypt = new MemoryStream(cipherData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plainData = GetBytes(srDecrypt.ReadToEnd());
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

		private byte[] EncryptString(string plainData, byte[] key)
		{
			SymmetricAlgorithm cryptoAlgorythm = SymmetricAlgorithm.Create();
			cryptoAlgorythm.GenerateIV();
			ICryptoTransform encryptor = cryptoAlgorythm.CreateEncryptor(key, cryptoAlgorythm.IV);
			byte[] cipherData = new byte[0];

			using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						{
							StreamWriter swEncrypt = new StreamWriter(csEncrypt);
							swEncrypt.Write(plainData);
							swEncrypt.Close();
							csEncrypt.Clear();

							cipherData = msEncrypt.ToArray();
						}
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
	}
}
