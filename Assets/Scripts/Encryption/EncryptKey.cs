using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class EncryptKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string Hash(string password, byte[] hashBytes)
    {
        var bytes = new UTF8Encoding().GetBytes(password);
        using (var algorithm = new System.Security.Cryptography.SHA512Managed())
        {
            hashBytes = algorithm.ComputeHash(bytes);
        }
        return Convert.ToBase64String(hashBytes);
    }
}
