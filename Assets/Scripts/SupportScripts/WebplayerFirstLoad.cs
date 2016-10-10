using UnityEngine;
using System.Collections;

public class WebplayerFirstLoad : MonoBehaviour {

    private bool load = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.CanStreamedLevelBeLoaded("Login") && !load)
        {
            load = true;
            Application.LoadLevel("Login");
        }
	}
}
