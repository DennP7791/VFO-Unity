using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateDrawSheet : MonoBehaviour {
	
	AnimationState a1;
	AnimationState a2;
	AnimationState a3;
	AnimationState b1;
	
	public class CAnimate
	{
		AnimationState s;
		float animFps;
		bool additive = false;
		float animFadeTime;
		string callname;
		
		public CAnimate(string name, string animName, int layer, AnimationBlendMode blendMode, float weight, float fps, float fadeTime)
		{
			s = AnimateDrawSheet.Instance.GetComponent<Animation>()[animName];
			
			s.wrapMode = WrapMode.ClampForever;
			s.blendMode = blendMode;
			s.weight = weight;
			s.layer = layer;
			
			animFadeTime = fadeTime;			
			animFps = fps;
			
			callname = name;
			
			if(blendMode == AnimationBlendMode.Additive)
			{
				additive = true;
				s.enabled = false;
			}
		}
		
		public int Layer
		{
			get { return s.layer; }
		}
		
		public void StartAnim()
		{
			if(additive && !s.enabled) {
				s.enabled = true;
			}
			else {
				AnimateDrawSheet.Instance.GetComponent<Animation>().CrossFade(s.name, animFadeTime);
			}
		}
		
		public void Update()
		{
			if(additive && s.enabled) {
				s.time += Time.deltaTime / animFps;
			}
		}
	}
	
	private static AnimateDrawSheet _instance;
	public static AnimateDrawSheet Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateDrawSheet)GameObject.FindObjectOfType(typeof(AnimateDrawSheet));

                if (!_instance)
                {
                    Debug.LogError("AnimateDrawSheet instance could not be found, make sure to add a bed to the scene, and add the AnimateDrawSheet script to it");
                } 
            }

            return _instance;
        }
    }
	
	private List<string> 	animationName = new List<string>();
	private List<CAnimate>	cAnimation = new List<CAnimate>();
	string idleAnim = "";
	
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
	
	public void AddAnimation(string name, string animName, bool additive, int layer)
	{
		AddAnimation(name, animName, additive, layer, GetComponent<Animation>()[animName].weight, 25.0f, 2.0f);
	}
		
	public void AddAnimation(string name, string animName, bool addtive, int layer, float weight)
	{
		AddAnimation(name, animName, addtive, layer, weight, 25.0f, 2.0f);
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
		foreach(CAnimate c in cAnimation)
		{
			c.Update();
		}
	}
}
