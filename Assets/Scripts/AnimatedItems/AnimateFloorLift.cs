using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateFloorLift : MonoBehaviour
{
    // callback from the remote
    public void MoveUp()
    {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "FloorLiftUp");
    }

    public void MoveDown()
    {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "FloorLiftDown");
    }

    public void MoveOut()
    {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "FloorLiftOut");
    }

    public void MoveIn()
    {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "FloorLiftIn");
    }

    public class CAnimate
	{
		AnimationState s;
		float animFps;
		bool additive = false;
		float animFadeTime;
		string callname;
        float additiveWeight;
		
		public CAnimate(string name, string animName, int layer, AnimationBlendMode blendMode, float weight, float fps, float fadeTime)
		{
			s = AnimateFloorLift.Instance.GetComponent<Animation>()[animName];
			
			s.wrapMode = WrapMode.ClampForever;
			s.blendMode = blendMode;
            s.layer = layer;
			
			animFadeTime = fadeTime;			
			animFps = fps;
			
			callname = name;

            if (blendMode == AnimationBlendMode.Additive)
            {
                additive = true;
                additiveWeight = weight;
                s.weight = 0.0f;
            }
            else
            {
                s.weight = weight;
            }
		}
		
		public int Layer
		{
			get { return s.layer; }
		}
		
		public void StartAnim()
		{
			if(additive) {
                AnimateFloorLift.Instance.GetComponent<Animation>().Blend(s.name, additiveWeight, animFadeTime);
			}
			else {
				AnimateFloorLift.Instance.GetComponent<Animation>().CrossFade(s.name, animFadeTime);
			}
		}
	}
	
	private static AnimateFloorLift _instance;
	public static AnimateFloorLift Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateFloorLift)GameObject.FindObjectOfType(typeof(AnimateFloorLift));

                if (!_instance)
                {
                    Debug.LogError("AnimateFloorLift instance could not be found, make sure to add a bed to the scene, and add the AnimateFloorLift script to it");
                } 
            }

            return _instance;
        }
    }
	
	private List<string> 	animationName = new List<string>();
	private List<CAnimate>	cAnimation = new List<CAnimate>();
    private List<string> animStates = new List<string>();
    private List<float> animDelays = new List<float>();

    private string idleAnim = "";
    private string currentState = "";
	
	public void SetIdle(string animName)
	{
		GetComponent<Animation>()[animName].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play(animName);
		if(idleAnim != animName)
		{
			GetComponent<Animation>().Stop(idleAnim);
			idleAnim = animName;
		}
	}

    public void AddAnimation(string[] statearr, string name, string animName, float delay, bool additive, int layer)
    {
        AddAnimation(statearr, name, animName, delay, additive, layer, GetComponent<Animation>()[animName].weight, 25.0f, 2.0f);
    }

    public void AddAnimation(string[] statearr, string name, string animName, float delay, bool addtive, int layer, float weight)
    {
        AddAnimation(statearr, name, animName, delay, addtive, layer, weight, 25.0f, 2.0f);
    }

    public void AddAnimation(string[] statearr, string name, string animName, float delay, bool additive, int layer, float weight, float fadeTime)
    {
        if (weight == -1.0f)
            weight = GetComponent<Animation>()[animName].weight;
        AddAnimation(statearr, name, animName, delay, additive, layer, weight, 25.0f, fadeTime);
    }

    public void AddAnimation(string[] statearr, string name, string animName, float delay, bool additive, int layer, float weight, float fps, float fadeTime)
    {
        for (int i = 0; i < statearr.Length; ++i)
        {
            CAnimate c = new CAnimate(name, animName, layer, additive ? AnimationBlendMode.Additive : AnimationBlendMode.Blend, weight, fps, fadeTime);
            animationName.Add(name);
            cAnimation.Add(c);
            animStates.Add(statearr[i]);
            animDelays.Add(delay);
        }
    }

    public void UpdateAnimation(string state)
    {
        if (state != currentState)
        {
            currentState = state;

            for (int i = 0; i < animStates.Count; ++i)
            {
                if (animStates[i] == state)
                {
                    string n = animationName[i];
                    float d = animDelays[i];
                    StartAnimation(n, d);
                }
            }
        }
    }
	
	public void AddAnimation(string name, string animName, bool additive, int layer)
	{
		AddAnimation(name, animName, additive, layer, GetComponent<Animation>()[animName].weight, 25.0f, 2.0f);
	}
		
	public void AddAnimation(string name, string animName, bool addtive, int layer, float weight)
	{
		AddAnimation(name, animName, addtive, layer, weight, 25.0f, 2.0f);
	}
	
	public void AddAnimation(string name, string animName, bool additive, int layer, float weight, float fadeTime)
	{
		if(weight == -1.0f)
			weight = GetComponent<Animation>()[animName].weight;
		AddAnimation(name, animName, additive, layer, weight, 25.0f, fadeTime);
	}
	
	public void AddAnimation(string name, string animName, bool additive, int layer, float weight, float fps, float fadeTime)
	{
		CAnimate c = new CAnimate(name, animName, layer, additive ? AnimationBlendMode.Additive : AnimationBlendMode.Blend, weight, fps, fadeTime);
		animationName.Add(name);
		cAnimation.Add(c);
	}
	
	public void StartAnimation(string name)
	{
		StartCoroutine(StartAnimationTimed(name, 0.0f));	
	}
	
	public void StartAnimation(string name, float delay)
	{
		StartCoroutine(StartAnimationTimed(name, delay));
	}
	
	public void StartIdle(string animName)
	{
		GetComponent<Animation>()[animName].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play(animName);
	}
	
	private IEnumerator StartAnimationTimed(string name, float delay)
	{
		yield return new WaitForSeconds(delay);
		
		int pos = animationName.IndexOf(name);
		if(pos != -1)
		{
			cAnimation[pos].StartAnim();
		}
		else
		{
			Debug.LogError("Animation with the name: " + name + " could not be started");
		}
	}
	
	// Use this for initialization
	void Start () 
	{
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
