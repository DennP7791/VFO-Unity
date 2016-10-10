using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateLiftReceiver : MonoBehaviour {
	
	public AnimationState AnimBedHead;
	
	private List<AnimationState> anim = new List<AnimationState>();
	private List<string>		animName = new List<string>();
	private List<float>			animTime = new List<float>();
	
	private int layer = 10;
	
	// callback from the remote
	public void MoveLiftUp()
	{
		//animation.Blend(GetAnimationName("BedHead"), 1.0f, GetAnimationTime("BedHead"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "MoveLiftUp");
	}
	
	public void MoveLiftDown()
	{
		//animation.Blend(GetAnimationName("BedHead"), 0.0f, GetAnimationTime("BedHead"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "MoveLiftDown");
	}
	
	public void MoveLiftLegsOut()
	{
		//animation.Blend(GetAnimationName("BedEnd"), 1.0f, GetAnimationTime("BedEnd"));	
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "MoveLiftLegsOut");
	}
	
	public void MoveLiftLegsIn()
	{
		//animation.Blend(GetAnimationName("BedEnd"), 0.0f, GetAnimationTime("BedEnd"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "MoveLiftLegsIn");
	}
	
	/*public void MoveBedDown()
	{
		animation.Blend(GetAnimationName("BedHeight"), 1.0f, GetAnimationTime("BedHeight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeightDown");
	}
	
	public void MoveBedUp()
	{
		animation.Blend(GetAnimationName("BedHeight"), 0.0f, GetAnimationTime("BedHeight"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedHeightUp");
	}
	
	public void MoveBedSittingUp()
	{
		animation.Blend(GetAnimationName("BedSitting"), 1.0f, GetAnimationTime("BedSitting"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingUp");
	}
	
	public void MoveBedSittingDown()
	{
		animation.Blend(GetAnimationName("BedSitting"), 0.0f, GetAnimationTime("BedSitting"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingDown");
	}
	
	public void MoveBedSittingLegsUp()
	{
		animation.Blend(GetAnimationName("BedSittingLegs"), 1.0f, GetAnimationTime("BedSittingLegs"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingLegsUp");
	}
	
	public void MoveBedSittingLegsDown()
	{
		animation.Blend(GetAnimationName("BedSittingLegs"), 0.0f, GetAnimationTime("BedSittingLegs"));
		GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
		if(go) go.SendMessage("SimCallback", "BedSittingLegsDown");
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
		for(int i = 0; i < animName.Count; ++i)
		{
			if(animName[i] == name)
			{
				pos = i;
			}
		}
		
		if(pos != -1)
		{
			AnimationState s = animation[animationName];
			s.weight = 0.0f;
			s.blendMode = AnimationBlendMode.Additive;
			s.wrapMode = WrapMode.ClampForever;
			s.layer = anim[pos].layer;
			
			anim[pos] = s;
			animTime[pos] = blendTime;
		}
	}
	
	private string GetAnimationName(string name)
	{
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
	
	private float GetAnimationTime(string name)
	{
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
	
	private void AddAnimation(string name, string animationName, float time)
	{
		AnimationState s = animation[animationName];
		s.weight = 0.0f;
		s.blendMode = AnimationBlendMode.Additive;
		s.wrapMode = WrapMode.ClampForever;
		s.layer = layer;
		
		anim.Add(s);
		animName.Add(name);
		animTime.Add(time);
		
		layer++;
	}*/
	
	private static AnimateLiftReceiver _instance; //singleton
	public static AnimateLiftReceiver Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateLiftReceiver)GameObject.FindObjectOfType(typeof(AnimateLiftReceiver));

                if (!_instance)
                {
                    Debug.LogError("AnimateLiftReceiver instance could not be found, make sure to add a bed to the scene, and add the AnimateLiftReceiver script to it");
                } 
            }

            return _instance;
        }
    }
	
	// Use this for initialization
	void Awake () 
	{
		/*animation["base"].wrapMode = WrapMode.Loop;
		animation.Play("base");
		
		AddAnimation("BedHead", "head_end_pos_high", 2.0f);
		AddAnimation("BedEnd", "foot_end_pos_high", 2.0f);*/
	}	
	
	// Update is called once per frame
	void Update () 
	{
	}
}
