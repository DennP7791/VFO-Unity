using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

public class EncryptVideo {


    public void EncryptFile(string srcPath, string destPath, string key, byte[] salt)
    {
        try
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt.ToString()));

            using (SymmetricAlgorithm aes = new RijndaelManaged())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = rgb.GetBytes(aes.KeySize >> 3);
                aes.IV = rgb.GetBytes(aes.BlockSize >> 3);
                aes.Mode = CipherMode.CBC;

                using (FileStream fsCrypt = new FileStream(destPath, FileMode.Create))
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (FileStream fsIn = new FileStream(srcPath, FileMode.Open))
                            {
                                int data;
                                while ((data = fsIn.ReadByte()) != -1)
                                {
                                    cs.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // failed to encrypt file
            Console.Write(ex);
        }
    }

    public void DecryptFile(string srcPath, string destPath, string key, byte[] salt)
    {
        try
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt.ToString()));
            Console.WriteLine("");
            using (SymmetricAlgorithm aes = new RijndaelManaged())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = rgb.GetBytes(aes.KeySize >> 3);
                aes.IV = rgb.GetBytes(aes.BlockSize >> 3);
                aes.Mode = CipherMode.CBC;

                using (FileStream fsCrypt = new FileStream(srcPath, FileMode.Open))
                {
                    using (FileStream fsOut = new FileStream(destPath, FileMode.Create))
                    {
                        using (ICryptoTransform decryptor = aes.CreateDecryptor())
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOut.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // failed to decrypt file
            Console.Write(ex);
        }
    }

}
