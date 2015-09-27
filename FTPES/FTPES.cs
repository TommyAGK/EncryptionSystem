

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FTPES
{
	public class Ftpes
	{
		//todo: Read file input

		private static TripleTCrypto tttCrypto;
		

		private static void Main()
		{
			// entrypoint for testing
			Console.WriteLine("Please enter text you want encrypted.");
			string readLine = Console.ReadLine();
			Console.WriteLine("Enter your password");
			string passPhrase = Console.ReadLine();
			Console.Clear();
			Console.WriteLine("Initializating encryption protocol");

			//set byte arrays, IV and KEY
			tttCrypto = new TripleTCrypto(passPhrase); // first passphrase

			Console.WriteLine(Environment.NewLine);
			//Console.WriteLine("Encrypted data: " + Encoding.Default.GetString(tttCrypto.ApplyEncryption(readLine)));
			string encryptedString = tttCrypto.ApplyEncryption(readLine);
			Console.WriteLine("Encrypted data: " + encryptedString);

			Console.WriteLine("__________________");
			Console.WriteLine("Decrypting...");

			//byte[] plainBytes = DecryptData(cryptoArr, IV, Key);
			if (passPhrase.Equals(Console.ReadLine()))
				{
					Console.WriteLine("Decrypted data: " + tttCrypto.ApplyDecryption(encryptedString));

				}
			else
				{
					goto WrongPass;
				}
			Console.ReadKey(true);
			
			WrongPass:
			Console.Clear();
			Console.WriteLine("You did not give the correct password, data is discarded as punishment!");


			

			}

		
	}
}
