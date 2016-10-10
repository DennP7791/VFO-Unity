using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MoveAndFadeCamera : MonoBehaviour 
{	
	[System.Serializable]
	public class CameraPos
	{
		public Vector3 pos;
		public Vector3 rot;
		
		public CameraPos(Vector3 p, Vector3 r)
		{
			pos = p;
			rot = r;
		}
	}
	
	private static MoveAndFadeCamera _instance; //singleton
	public static MoveAndFadeCamera Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (MoveAndFadeCamera)GameObject.FindObjectOfType(typeof(MoveAndFadeCamera));

                if (!_instance)
                {
                    Debug.LogError("Make sure you attach the moveandfadecamera.cs script to the main camera");
                }  
            }

            return _instance;
        }
    }
	
	public List<CameraPos> CameraPosition = new List<CameraPos>();
	
//	private CameraPos OriginalCameraPosition;
	public Texture2D fadeTexture;
	
	public bool startFade = false;
	
	private float fadeTime = 1.0f;
	private float fadeDelay = 1.0f;
	private float delayTime = 0.0f;
	
	private Color fadeColor;
	private bool changeCamera = false;
	
	private int currentCamera = 0;
	private int newCamera = 0;
	
	private List<string> fadeStates = new List<string>();
	private List<int> fadeNumbers = new List<int>();
	private List<float> fadeDelays = new List<float>();
	private List<float> fadeTimes = new List<float>();
	private string currentState = "";
	
	public void AddCameraFade(string[] statearr, int fn, float fd, float ft)
	{
		for(int i = 0; i < statearr.Length; ++i)
		{
			fadeStates.Add(statearr[i]);
			fadeNumbers.Add(fn);
			fadeDelays.Add(fd);
			fadeTimes.Add(ft);
		}
	}
	
	public void UpdateCameraFade(string s)
    {
        if (s != currentState)
        {
            currentState = s;

            int pos = fadeStates.IndexOf(s);
            if (pos != -1)
            {
				StartFade(fadeNumbers[pos], fadeDelays[pos], fadeTimes[pos]);	
			}
		}
	}
	
	public void StartFade(int fadeToCamera)
	{
		StartFade(fadeToCamera, 0.0f, 1.0f);	
	}
	
	public void StartFade(int fadeToCamera, float delay)
	{
		StartFade(fadeToCamera, delay, 1.0f);	
	}
	
	public void StartFade(int fadeToCamera, float delay, float fadetime)
	{
		if(CameraPosition.Count > fadeToCamera)
		{
			if(fadeToCamera != currentCamera)
			{
				newCamera = fadeToCamera;
				fadeTime = fadetime;
				fadeDelay = delay;
				startFade = true;
			}
		}
		else
		{
			Debug.Log("Trying to fade to a camera position that does not exist");
		}
	}
	
	public void ForceStopFade()
	{
		startFade = false;
	}
	
	
	// Use this for initialization
	void Start () {
//		OriginalCameraPosition = new CameraPos(transform.position, transform.rotation.eulerAngles);
		fadeColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{ 
		if(startFade)
		{
			if(!changeCamera)
			{
				fadeColor.a += Time.deltaTime / fadeTime;
				if(fadeColor.a >= 1.0f)
				{
					fadeColor.a = 1.0f;
					transform.position = CameraPosition[newCamera].pos;
					transform.rotation = Quaternion.Euler(CameraPosition[newCamera].rot);
					changeCamera = true;
					currentCamera = newCamera;
				}
			}
			else
			{
				delayTime += Time.deltaTime;
				if(delayTime >= fadeDelay)
				{
					fadeColor.a -= Time.deltaTime / fadeTime;
					if(fadeColor.a <= 0.0f)
					{
						startFade = false;
						changeCamera = false;
						fadeDelay = 0.0f;
						delayTime = 0.0f;
						fadeColor.a = 0.0f;
					}
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(startFade)
		{
			GUI.color = fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		}
	}
}
