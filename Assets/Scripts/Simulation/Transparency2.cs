using UnityEngine;
using System.Collections;

public class Transparency2 : MonoBehaviour 
{
	private static Transparency2 _instance; //singleton
    private bool startFade = false;
    private bool fadeIn = false;
    private Color dc;

    public static Transparency2 Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Transparency2)GameObject.FindObjectOfType(typeof(Transparency2));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Transparency2";
                    _instance = (Transparency2)container.AddComponent(typeof(Transparency2));
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
            if (fadeIn && dc.a > 0.0f)
            {
                dc.a -= 1.0f * (Time.deltaTime);
            }
            else if(fadeIn)
            {
                dc.a = 0.0f;
                startFade = false;
                gameObject.GetComponent<Renderer>().enabled = false;
                foreach (Transform child in transform)
                {
                    child.GetComponent<Renderer>().enabled = false;
                }
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
