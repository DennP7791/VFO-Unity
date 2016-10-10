using UnityEngine;
using System.Collections;

public class AnimateFemaleHelper : MonoBehaviour 
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
			anim = AnimateFemaleHelper.Instance.GetComponent<Animation>()[animationName];
			bakedAnim = baked;

			if(baked)
			{
				anim.layer = AnimateFemaleHelper.Instance.layer;
				anim.blendMode = AnimationBlendMode.Additive;
				AnimateFemaleHelper.Instance.layer++;
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
					normalizedTime = normalizedTime >= 1.0f ? 1.0f : normalizedTime + (1.0f / (float)AnimateFemaleHelper.Instance.MoveAmounts);
					moveSpeed = animSpeed;
					if(normalizedTime < 1.0f) AnimateFemaleHelper.Instance.GetComponent<Animation>().Play(anim.name);
				}
				else {
					normalizedTime = normalizedTime <= 0.0f ? 0.0f : normalizedTime - (1.0f / (float)AnimateFemaleHelper.Instance.MoveAmounts);
					moveSpeed = -animSpeed;
					if(normalizedTime > 0.0f) AnimateFemaleHelper.Instance.GetComponent<Animation>().Play(anim.name);
				};
			}
			else
			{
				AnimateFemaleHelper.Instance.GetComponent<Animation>().CrossFade(anim.name, 2.0f, PlayMode.StopSameLayer);
			}
		}
		
		public void StartAnimation(bool u, float s, float d)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimateFemaleHelper.Instance.StartCoroutine(StartAnimTimed(AnimateFemaleHelper.Instance.MoveAmounts));
			
		}
		
		public void StartAnimation(bool u, float s, float d, int moveSteps)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimateFemaleHelper.Instance.StartCoroutine(StartAnimTimed(moveSteps));
			
		}
		
		public AnimationState AnimState
		{
			get { return anim; }
			set { anim = value; }
		}
	}
	
	private static AnimateFemaleHelper _instance; //singleton
	
	private int MoveAmounts = 2; // Numbers of steps the bed can move
	
	private int layer = 11;
	
	public CAnimate AssistLegs;
	public CAnimate SlideMatRight;
	public CAnimate SlideMatLeft;
	public CAnimate BendKnees;
	public CAnimate HandOnKnees;
	public CAnimate TurnPatient;
	public CAnimate AntiSlideMatFeetB;
	public CAnimate SlideMatLeftShoulderB;
	public CAnimate SlideMatRightShoulderB;
	public CAnimate SittingMoveForwardC;
	public CAnimate StandUpC;
	public CAnimate HandsOnHips;
	public CAnimate WalkWithPatient;
		
	public static AnimateFemaleHelper Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateFemaleHelper)GameObject.FindObjectOfType(typeof(AnimateFemaleHelper));

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
		/*
		animation["idle_100f"].wrapMode = WrapMode.Loop;
		animation.Play("idle_100f");
		
		AssistLegs 				= new CAnimate("C_assist_legs_263f", false);
		SlideMatRight			= new CAnimate("cloth_under_left_shoulder_106f", false);
		SlideMatLeft			= new CAnimate("cloth_under_right_shoulder_101f", false);
		BendKnees				= new CAnimate("bend_knees_96f", false);
		HandOnKnees				= new CAnimate("hand_knee_hip_31f", false);
		TurnPatient				= new CAnimate("lturn_citizen_91f", false);
		AntiSlideMatFeetB		= new CAnimate("coach_push_sheet_under_patient_feets_116f", false);
		SlideMatLeftShoulderB	= new CAnimate("coach_push_sheet_under_patient_shoulderl_91f", false);
		SlideMatRightShoulderB	= new CAnimate("coach_push_sheet_under_patient_shoulderr_76f_2013", false);
		SittingMoveForwardC		= new CAnimate("C_bed_Sitting_Move_forward_1_131F", false);
		StandUpC				= new CAnimate("C_bed_Standup_151F", false);
		HandsOnHips				= new CAnimate("200_20", false);
		WalkWithPatient			= new CAnimate("200_30", false);
		*/
	}
	
	public void StandOnLeftSide() {
		GetComponent<Animation>()["idle_leftside_1f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("idle_leftside_1f");
		GetComponent<Animation>().Stop("idle_100f");
	}
	
	public void StandingWalk() {
		GetComponent<Animation>()["200_10"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("200_10");
		GetComponent<Animation>().Stop("idle_100f");
	}
	
	// Update is called once per frame
	void Update () 
	{					
	}
}
