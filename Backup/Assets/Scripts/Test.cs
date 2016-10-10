using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void Awake()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Interface");
        foreach(GameObject g in go)
        {
            Debug.Log(g.name);
            if (g.name == "Background")
            {
                g.active = false;
            }
            DontDestroyOnLoad(g);
        }
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
