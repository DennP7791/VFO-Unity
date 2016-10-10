using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Global : MonoBehaviour {

    private static Global _instance; //singleton

    private Dictionary<string,GameObject> guiObjects; //list of the 
    // Singleton
    public static Global Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Global)GameObject.FindObjectOfType(typeof(Global));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Global";
                    _instance = (Global)container.AddComponent(typeof(Global));
                }
            }

            return _instance;
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        if(_instance)
            DontDestroyOnLoad(_instance);
    }

    public void ToggleInfoWin()
    {
        InfoWindowScript infWinScript = (InfoWindowScript)GameObject.FindObjectOfType(typeof(InfoWindowScript));
        if (infWinScript)
        {
            infWinScript.Visible = !infWinScript.Visible;
            LinkMenu linkMenu = (LinkMenu)GameObject.FindObjectOfType(typeof(LinkMenu));
            if (linkMenu)
                linkMenu.Enabled = !infWinScript.Visible;
            return;
        }

        Object infWin = Resources.Load("InfoWindow");
        if (infWin)
        {
            GameObject instance = (GameObject)Object.Instantiate(infWin);
            infWinScript = (InfoWindowScript)GameObject.FindObjectOfType(typeof(InfoWindowScript));
            if (infWinScript)
            {
                infWinScript.Visible = true;
                LinkMenu linkMenu = (LinkMenu)GameObject.FindObjectOfType(typeof(LinkMenu));
                if (linkMenu)
                    linkMenu.Enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("InfoWindow is not an existent resource.");
        }
    }

}
