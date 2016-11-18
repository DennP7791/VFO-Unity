using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

public class EncryptVideo {

    private string key = "HR$2pIjHR$2pIj12jh3adTaF3bi23u9n7a";
    private bool _isDencrpyting = true;
    public event EventHandler<EventArgs> IsDecryptingChanged;

    public bool IsDecrypting
    {
        get { return _isDencrpyting; }
        set
        {
            _isDencrpyting = value;
            OnIsDecryptingChanged(null);
        }
    }

    protected virtual void OnIsDecryptingChanged(EventArgs e)
    {
        if (IsDecryptingChanged != null)
            IsDecryptingChanged(this, e);
    }

    public void EncryptFile(string srcPath)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        string file = Path.GetFileNameWithoutExtension(srcPath);
        string destPath = srcPath.Replace(file, file + "-Encrypted");

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
                                    //yield return cs;
                                }
                                //IsEncrypting = true;
                            }
                        }
                    }
                }
                OverwriteFile(srcPath, destPath);
            }
        }
        catch (Exception ex)
        {
            // failed to encrypt file
            Debug.Log(ex);
        }
    }

    public void DecryptFile(string srcPath)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        string file = Path.GetFileNameWithoutExtension(srcPath);
        string destPath = srcPath.Replace(file, file + "-Encrypted");

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
                OverwriteFile(srcPath, destPath);
            }
            IsDecrypting = false;
        }
        catch (Exception ex)
        {
            // failed to decrypt file
            Console.Write(ex);
        }
    }

    void OverwriteFile(string originalPath, string encryptedPath)
    {
        if (File.Exists(originalPath))
        {
            Debug.Log("File exists");
            File.Delete(originalPath);
        }
        File.Move(encryptedPath, originalPath);
    }
}
