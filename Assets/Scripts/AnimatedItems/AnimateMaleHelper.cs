using UnityEngine;
using System.Collections;

public class AnimateMaleHelper : MonoBehaviour 
{
	public class CAnimate
	{
		private AnimationState anim;
		private float normalizedTime	= 0.0f;
		private float moveSpeed			= 1.0f;
		private float animSpeed			= 1.0f;
		private float delay				= 0.0f;
		private bool  bakedAnim			= true;
		private bool  upDown			= true;
		
		public CAnimate(string animationName, bool baked)
		{
			anim = AnimateMaleHelper.Instance.GetComponent<Animation>()[animationName];
			if(!anim)
				Debug.LogError("wrong anim: " + animationName);
			
			bakedAnim = baked;

			if(baked)
			{
				anim.layer = AnimateMaleHelper.Instance.layer;
				anim.blendMode = AnimationBlendMode.Additive;
				AnimateMaleHelper.Instance.layer++;
			}
			else
			{
				anim.layer = 100;
				anim.blendMode = AnimationBlendMode.Blend;
			}
			anim.wrapMode = WrapMode.ClampForever;
			
			if(baked)
				anim.weight = 1.0f;
			
			anim.enabled = false;		
		}
		
		public void update()
		{
			if(bakedAnim)
			{
				if(moveSpeed > 0.0f)
				{
					anim.normalizedSpeed = anim.normalizedTime >= normalizedTime ? 0.0f : moveSpeed;
				}
				else if(moveSpeed < 0.0f)
				{
					anim.normalizedSpeed = anim.normalizedTime >= normalizedTime ? moveSpeed : 0.0f;
				}
				if(normalizedTime <= 0.0f) normalizedTime = 0.0f;
			}
		}
		
		private IEnumerator StartAnimTimed(int moveSteps)
		{
			yield return new WaitForSeconds(delay);
			
			if(bakedAnim)
			{
				if(upDown) {
					normalizedTime = normalizedTime >= 1.0f ? 1.0f : normalizedTime + (1.0f / (float)AnimateMaleHelper.Instance.MoveAmounts);
					moveSpeed = animSpeed;
					if(normalizedTime < 1.0f) AnimateMaleHelper.Instance.GetComponent<Animation>().Play(anim.name);
				}
				else {
					normalizedTime = normalizedTime <= 0.0f ? 0.0f : normalizedTime - (1.0f / (float)AnimateMaleHelper.Instance.MoveAmounts);
					moveSpeed = -animSpeed;
					if(normalizedTime > 0.0f) AnimateMaleHelper.Instance.GetComponent<Animation>().Play(anim.name);
				};
			}
			else
			{
				AnimateMaleHelper._instance.transform.rotation = Quaternion.identity;
				AnimateMaleHelper.Instance.transform.position = Vector3.zero;
				AnimateMaleHelper.Instance.GetComponent<Animation>().CrossFade(anim.name, 2.0f, PlayMode.StopSameLayer);
			}
		}
		
		public void StartAnimation(bool u, float s, float d)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimateMaleHelper.Instance.StartCoroutine(StartAnimTimed(AnimateMaleHelper.Instance.MoveAmounts));
		
		}
		
		public void StartAnimation(bool u, float s, float d, int moveSteps)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimateMaleHelper.Instance.StartCoroutine(StartAnimTimed(moveSteps));
			
		}
		
		public AnimationState AnimState
		{
			get { return anim; }
			set { anim = value; }
		}
	}
	
	private static AnimateMaleHelper _instance; //singleton
	
	private int MoveAmounts = 2; // Numbers of steps the bed can move
	
	private int layer = 11;
	
	public CAnimate BendKnees;
	public CAnimate HandUnderButt;
	public CAnimate TurnPatient;
	public CAnimate MoveForwardSittingC;
	public CAnimate StandUpC;
	public CAnimate WalkWithWheelChair;
	
	public static AnimateMaleHelper Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateMaleHelper)GameObject.FindObjectOfType(typeof(AnimateMaleHelper));

                if (!_instance)
                {
                    Debug.LogError("Bed instance could not be found, make sure to add a bed to the scene, and add the AnimateBed script to it");
                } 
            }

            return _instance;
        }
    }
	
		
		
	// Use this for initialization
	void Awake () {
		
		GetComponent<Animation>()["idle_100f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("idle_100f");
		
		BendKnees 				= new CAnimate("bend_knees_111f", false);	
		HandUnderButt 			= new CAnimate("hands_under_bottom_41f", false);
		TurnPatient 			= new CAnimate("turn_citizen_80f", false);
		MoveForwardSittingC 	= new CAnimate("C_bed_Sitting_Move_forward_1_131F", false);
		StandUpC				= new CAnimate("C_bed_Sitting_Standup_151F", false);
		WalkWithWheelChair		= new CAnimate("210_30", false);
	}
	
	public void StandUpBorgerC()
	{
		AnimateMaleHelper.Instance.transform.rotation = Quaternion.identity;
		AnimateMaleHelper.Instance.transform.position = Vector3.zero;
		GetComponent<Animation>()["stand_up_borger_c_Assistant_Idle"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("stand_up_borger_c_Assistant_Idle");
		GetComponent<Animation>().Stop("idle_100f");
	}
	
	public void TurnToSideBorgerC()
	{
		AnimateMaleHelper.Instance.transform.rotation = Quaternion.identity;
		AnimateMaleHelper.Instance.transform.position = Vector3.zero;
		GetComponent<Animation>()["c_turn_to_side_Idle"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("c_turn_to_side_Idle");
		GetComponent<Animation>().Stop("idle_100f");
	}
	
	public void WalkWithWheelChairC()
	{
		AnimateMaleHelper.Instance.transform.rotation = Quaternion.identity;
		AnimateMaleHelper.Instance.transform.position = Vector3.zero;
		GetComponent<Animation>()["helper@210_25"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("helper@210_25");
		GetComponent<Animation>().Stop("idle_100f");
	}
	
	// Update is called once per frame
	void Update () 
	{					
	}
}
