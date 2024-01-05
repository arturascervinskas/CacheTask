using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public static class EncryptionService
{
    public static readonly byte[] IV = new byte[]
    {
        254, 194, 102, 235,
        16, 169, 70, 249,
        35, 59, 69, 171,
        223, 150, 27, 136
    };

    public static readonly byte[] Key = new byte[]
    {
        125, 23, 252, 174,
        208, 199, 47, 32,
        13, 246, 16, 131,
        235, 193, 208, 184,
        118, 249, 31, 136,
        238, 51, 165, 91,
        118, 236, 101, 43,
        96, 139, 164, 55
    };


    public static byte[]? EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new ())
            {
                using (CryptoStream csEncrypt = new (msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new (csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    public static string? DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        // Declare the string used to hold
        // the decrypted text.
        string? plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new (cipherText))
            {
                using (CryptoStream csDecrypt = new (msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new (csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}
