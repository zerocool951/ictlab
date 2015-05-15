using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*
 * Based on: https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
 */

namespace Server_Data {
    internal static class EncryptionHelper {
        private const short keysize = 256;

        //Must be 32 Bytes, 16 chars
        private static readonly byte[] salt = Encoding.ASCII.GetBytes("b9ekyysopf7t0cam");

        public static string Encrypt(string text, string key) {
            byte[] textToBytes = Encoding.UTF8.GetBytes(text);
            using (var password = new PasswordDeriveBytes(key, null)) {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged rijndael = new RijndaelManaged()) {
                    rijndael.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = rijndael.CreateEncryptor(keyBytes, salt)) {
                        using (MemoryStream ms = new MemoryStream()) {
                            using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                                cryptoStream.Write(textToBytes, 0, textToBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] encryptedBytes = ms.ToArray();
                                return Convert.ToBase64String(encryptedBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string text, string key) {
            byte[] textToBytes = Convert.FromBase64String(text);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(key, null)) {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged rijndael = new RijndaelManaged()) {
                    rijndael.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = rijndael.CreateDecryptor(keyBytes, salt)) {
                        using (MemoryStream ms = new MemoryStream(textToBytes)) {
                            using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                                byte[] plainTextBytes = new byte[textToBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}