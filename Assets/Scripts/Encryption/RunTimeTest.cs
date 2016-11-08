using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class RunTimeTest : MonoBehaviour
{

    EncryptVideo ev = new EncryptVideo();

    // Use this for initialization
    void Start()
    {
        RUNTHISSHIT();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void RUNTHISSHIT()
    {
        string encryptFile = "C:/Users/Tasin/Desktop/First5mb.mp4";
        string newEncryptFile = "C:/Users/Tasin/Desktop/encryptetFile.mp4";
        string newDecryptFile = "C:/Users/Tasin/Desktop/decryptetFile.mp4";
        string getFile = "C:/Users/Tasin/AppData/LocalLow/Welfare Denmark/Virtuel Forflytning/Video.ogv";
        string name = "EncryptedFile";
        string key = "HR$2pIjHR$2pIj12jh3adTaF3bi23u9n7a";
        string password = "thisisatestforme123";
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        //
        //--------------------Encrypt/Decrypt Video-----------------------
        //
        Debug.Log("Encryption video Started");
        Debug.Log("Encrypt video");
        ev.EncryptFile(encryptFile, newEncryptFile, key, salt);
        Debug.Log("Setting video to Blob(putBlob)");
        AzureManager.PutBlob(newEncryptFile, name);
        Debug.Log("Getting video from blob(GetBlob)");
        AzureManager.GetBlob(name);
        Debug.Log("Decryption video Started");
        Debug.Log("Decrypt video");
        ev.DecryptFile(getFile, newDecryptFile, key, salt);
        Debug.Log("Decryption video Finished");
    }
}
