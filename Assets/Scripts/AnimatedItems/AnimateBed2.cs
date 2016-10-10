using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateBed2 : MonoBehaviour {
	
	public AnimationState AnimBedHead;
	
	private List<AnimationState> anim = new List<AnimationState>();
	private List<string>		animName = new List<string>();
	private List<float>			animTime = new List<float>();
	
	private int layer = 10;
	
	// callback from the remote
	public void MoveBedHeadUp()
	{
		if(!AllowBedHead)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedHead"), 1.0f, GetAnimationTime("BedHead"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeadUp");
	}
	
	public void MoveBedHeadDown()
	{
		if(!AllowBedHead)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedHead"), 0.0f, GetAnimationTime("BedHead"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeadDown");
	}
	
	public void MoveBedEndUp()
	{
		if(!AllowBedEnd)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedEnd"), 1.0f, GetAnimationTime("BedEnd"));	
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedEndUp");
	}
	
	public void MoveBedEndDown()
	{
		if(!AllowBedEnd)
			return; 
		
		GetComponent<Animation>().Blend(GetAnimationName("BedEnd"), 0.0f, GetAnimationTime("BedEnd"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedEndDown");
	}
	
	public void MoveBedDown()
	{
		if(!AllowBedHeight)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedHeight"), 1.0f, GetAnimationTime("BedHeight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeightDown");
	}
	
	public void MoveBedUp()
	{
		if(!AllowBedHeight)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedHeight"), 0.0f, GetAnimationTime("BedHeight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeightUp");
	}
	
	public void MoveBedSittingUp()
	{
		if(!AllowBedSitting)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedSitting"), 1.0f, GetAnimationTime("BedSitting"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingUp");
	}
	
	public void MoveBedSittingDown()
	{
		if(!AllowBedSitting)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedSitting"), 0.0f, GetAnimationTime("BedSitting"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingDown");
	}
	
	public void MoveBedSittingLegsUp()
	{
		if(!AllowBedSittingLegs)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedSittingLegs"), 1.0f, GetAnimationTime("BedSittingLegs"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingLegsUp");
	}
	
	public void MoveBedSittingLegsDown()
	{
		if(!AllowBedSittingLegs)
			return;
		
		GetComponent<Animation>().Blend(GetAnimationName("BedSittingLegs"), 0.0f, GetAnimationTime("BedSittingLegs"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingLegsDown");
	}
	
	public void MoveBedRailingDownL()
	{
		GetComponent<Animation>().Blend(GetAnimationName("BedRailingLeft"), 1.0f, GetAnimationTime("BedRailingLeft"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedRailingLeft");
	}
	
	public void MoveBedRailingUpL()
	{
		GetComponent<Animation>().Blend(GetAnimationName("BedRailingLeft"), 0.0f, GetAnimationTime("BedRailingLeft"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedRailingLeft");
	}
	
	public void MoveBedRailingDownR()
	{
		GetComponent<Animation>().Blend(GetAnimationName("BedRailingRight"), 1.0f, GetAnimationTime("BedRailingRight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedRailingRight");
	}
	
	public void MoveBedRailingUpR()
	{
		GetComponent<Animation>().Blend(GetAnimationName("BedRailingRight"), 0.0f, GetAnimationTime("BedRailingRight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedRailingRight");
	}
	
	// to only change the blend time of an animation
	public void ChangeAnimationBlendTime(string name, float blendTime)
	{
		for(int i = 0; i < animName.Count; ++i)
		{
			if(animName[i] == name)
			{
				animTime[i] = blendTime;
			}
		}
	}
	
	// to change the animation for the bed inside the simulation
	public void ChangeAnimation(string name, string animationName, float blendTime)
	{
		int pos = -1;
		for(int i = 0; i < animName.Count; ++i) {
			if(animName[i] == name)
			{
				pos = i;
			}
		}
		
		if(pos != -1) {
			AnimationState s = GetComponent<Animation>()[animationName];
			s.weight = 0.0f;
			s.blendMode = AnimationBlendMode.Additive;
			s.wrapMode = WrapMode.ClampForever;
			s.layer = anim[pos].layer;
			
			anim[pos] = s;
			animTime[pos] = blendTime;
		}
	}
	
	private string GetAnimationName(string name) {
		string n = "";
		for(int i = 0; i < animName.Count; ++i)
		{
			if(animName[i] == name)
			{
				n = anim[i].name;
			}
		}
		
		return n;
	}
	
	private float GetAnimationTime(string name) {
		float t = -1.0f;
		for(int i = 0; i < animName.Count; ++i)
		{
			if(animName[i] == name)
			{
				t = animTime[i];
			}
		}
		
		return t;
	}
	
	private void AddAnimation(string name, string animationName, float time) {
		AnimationState s = GetComponent<Animation>()[animationName];
		s.weight = 0.0f;
		s.blendMode = AnimationBlendMode.Additive;
		s.wrapMode = WrapMode.ClampForever;
		s.layer = layer;
		
		anim.Add(s);
		animName.Add(name);
		animTime.Add(time);
		
		layer++;
	}
	
		private static AnimateBed2 _instance; //singleton
		public static AnimateBed2 Instance
	    {
	        get
	        {
	            if (!_instance)
	            {
	                _instance = (AnimateBed2)GameObject.FindObjectOfType(typeof(AnimateBed2));
	
	                if (!_instance)
	                {
	                    Debug.LogError("Bed instance could not be found, make sure to add a bed to the scene, and add the AnimateBed2 script to it");
	                } 
	            }
	
	            return _instance;
	        }
	    }
	
	public bool AllowBedHeight 		= false;
	public bool AllowBedEnd 		= false;
	public bool AllowBedHead		= false;
	public bool AllowBedSitting		= false;
	public bool AllowBedSittingLegs = false;
	
	// Use this for initialization
	void Awake () 
	{
		GetComponent<Animation>()["base"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("base");
		
		AddAnimation("BedHead", "head_end_pos_high", 2.0f);
		AddAnimation("BedEnd", "foot_end_pos_high", 2.0f);
		AddAnimation("BedHeight", "pos_low", 2.0f);
		AddAnimation("BedSitting", "sitting_pos", 2.0f);
		AddAnimation("BedSittingLegs", "sitting_pos_legs", 2.0f);
		AddAnimation("BedRailingLeft", "rail_down_left", 2.0f);
		AddAnimation("BedRailingRight", "rail_down", 2.0f);
	}	
	
	// Update is called once per frame
	void Update () 
	{
	}
}
