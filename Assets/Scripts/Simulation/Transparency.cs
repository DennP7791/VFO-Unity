using UnityEngine;
using System.Collections;

public class Transparency : MonoBehaviour 
{
	private static Transparency _instance; //singleton
    private bool startFade = false;
    private bool fadeIn = false;
    private Color dc;

    public static Transparency Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Transparency)GameObject.FindObjectOfType(typeof(Transparency));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Transparency";
                    _instance = (Transparency)container.AddComponent(typeof(Transparency));
                }
            }

            return _instance;
        }	
    }

    void OnDestroy()
    {
        _instance = null;
    }
	
	public void StartFade()
	{
        fadeIn = false;

        if (gameObject.GetComponent<Renderer>().material.color.a < 1.0f)
            fadeIn = true;

        startFade = true;
        
	}

    public void SetAlpha(float a)
    {
        dc = new Color(1.0f, 1.0f, 1.0f, a);
        
        gameObject.GetComponent<Renderer>().material.color = dc;
        foreach (Transform child in transform) {
            if(child.GetComponent<Renderer>())
                child.GetComponent<Renderer>().material.color = dc;
        }
    }
	
	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (startFade)
        {
            if (fadeIn && dc.a < 1.0f)
            {
                dc.a += 1.0f * (Time.deltaTime);
                Debug.Log(dc.a);
            }
            else if(fadeIn)
            {
                dc.a = 1.0f;
                startFade = false;
            }

            gameObject.GetComponent<Renderer>().material.color = dc;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                    child.GetComponent<Renderer>().material.color = dc;
            }
        }
	}
}
