

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


        private static void Main()
        {
            // entrypoint for testing
            Console.WriteLine("Please enter text you want encrypted.");
            string readLine = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string passPhrase = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Initializating encryption protocol");

            //Can be DES, AES, Rijndael, etc.
            using (TripleTCrypto tttCrypto = new TripleTCrypto(DES.Create(), passPhrase))
            {
                Console.WriteLine(Environment.NewLine);
                //Console.WriteLine("Encrypted data: " + Encoding.Default.GetString(tttCrypto.ApplyEncryption(readLine)));
                byte[] encryptedBytes = tttCrypto.ApplyEncryption(readLine);
                Console.WriteLine("Encrypted data: " + Encoding.UTF8.GetString(encryptedBytes)); //not all characters will be valid UTF-8 - expect some '?'s

                Console.WriteLine("__________________");
                Console.WriteLine("Decrypting...");

                //byte[] plainBytes = DecryptData(cryptoArr, IV, Key);
                if (passPhrase.Equals(Console.ReadLine()))
                {
                    Console.WriteLine("Decrypted data: " + tttCrypto.ApplyDecryption(encryptedBytes));

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You did not give the correct password, data is discarded as punishment!");
                }
            }
                
            Console.ReadKey(true);





        }


    }
}
