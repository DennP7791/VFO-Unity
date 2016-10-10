using UnityEngine;
using System.Collections;

public class AnimatePatient : MonoBehaviour 
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
			anim = AnimatePatient.Instance.GetComponent<Animation>()[animationName];
			if(!anim)
				Debug.LogError("Couldn't find animation: " + animationName);
			
			bakedAnim = baked;

			if(baked)
			{
				anim.layer = AnimatePatient.Instance.layer;
				anim.blendMode = AnimationBlendMode.Additive;
				AnimatePatient.Instance.layer++;
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
					normalizedTime = normalizedTime >= 1.0f ? 1.0f : normalizedTime + (1.0f / (float)AnimatePatient.Instance.MoveAmounts);
					moveSpeed = animSpeed;
					if(normalizedTime < 1.0f) AnimatePatient.Instance.GetComponent<Animation>().Play(anim.name);
				}
				else {
					normalizedTime = normalizedTime <= 0.0f ? 0.0f : normalizedTime - (1.0f / (float)AnimatePatient.Instance.MoveAmounts);
					moveSpeed = -animSpeed;
					if(normalizedTime > 0.0f) AnimatePatient.Instance.GetComponent<Animation>().Play(anim.name);
				};
			}
			else
			{
				AnimatePatient.Instance.GetComponent<Animation>().CrossFade(anim.name, 2.0f, PlayMode.StopSameLayer);
			}
		}
		
		public void StartAnimation(bool u, float s, float d)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimatePatient.Instance.StartCoroutine(StartAnimTimed(AnimatePatient.Instance.MoveAmounts));
			
		}
		
		public void StartAnimation(bool u, float s, float d, int moveSteps)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimatePatient.Instance.StartCoroutine(StartAnimTimed(moveSteps));
			
		}
		
		public AnimationState AnimState
		{
			get { return anim; }
			set { anim = value; }
		}
	}
	
	private static AnimatePatient _instance; //singleton
	
	private int MoveAmounts = 2; // Numbers of steps the bed can move
	
	private int layer = 11;
		
	public CAnimate MoveBedEnd;
	public CAnimate MoveBedHead;
	public CAnimate MoveHead;
	public CAnimate BendLeftLeg;
	public CAnimate BendRightLeg;
	public CAnimate SideBendLegs;
	public CAnimate SidePlaceArms;
	public CAnimate LyingToSitting;
	public CAnimate LyingToSittingB;
	public CAnimate SittingMovingForward;
	public CAnimate SittingMoveLegs;
	public CAnimate SittingLeanForward;
	public CAnimate SittingStandUp;
	public CAnimate SittingMoveBack;
	public CAnimate SittingDownOnArm;
	public CAnimate DownLiftingLegs;
	public CAnimate OnBackLiftingLegs;
	public CAnimate BackCrawl;
	public CAnimate BackInChair;
	public CAnimate BendKnees;
	public CAnimate BendKneesB;
	public CAnimate BendKneesC;
	public CAnimate LookRight;
	public CAnimate ArmRight;
	public CAnimate TurnRight;
	public CAnimate TurnHeadRight;
	public CAnimate TurnSide;
	public CAnimate TurnSideC_I;
	public CAnimate SlideMatRight;
	public CAnimate SlideMatLeft;
	public CAnimate GrapCot;
	public CAnimate PushUpB;
	public CAnimate LiftLeftShoulderB;
	public CAnimate LiftRightShoulderB;
	public CAnimate MoveForwardB;
	public CAnimate PlaceLegsB;
	public CAnimate StandUpB1;
	public CAnimate StandUpB2;
	public CAnimate StandUpB3;
	public CAnimate SittingToStandingAdd;
	public CAnimate MoveForwardC;
	public CAnimate PlaceLegsC;
	public CAnimate StandUpC;
	public CAnimate WalkA;
	public CAnimate WalkB;	
	public CAnimate CounterBedUp;
	public CAnimate SitHandsUpC;
	public CAnimate LiftLegsAdditive;

	public static AnimatePatient Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimatePatient)GameObject.FindObjectOfType(typeof(AnimatePatient));

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
		
		GetComponent<Animation>()["Idle_back_100f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("Idle_back_100f");
		
		MoveBedEnd	 			= new CAnimate("Bed_lift_legs_2f", true);
		MoveBedHead 			= new CAnimate("Bed_lift_torso_2f", true);
		MoveHead 				= new CAnimate("No_pillow_2f", true); 
		BendLeftLeg 			= new CAnimate("Bend_left_leg_43f", false);
		BendRightLeg 			= new CAnimate("Bend_Right_leg_43f", false);
		SideBendLegs			= new CAnimate("Side_bend_legs_55f", false);
		SidePlaceArms			= new CAnimate("Side_Place_arms_51f", false);
		LyingToSitting			= new CAnimate("A_bed_Lying_to_Sitting_154f", false);
		SittingMovingForward	= new CAnimate("a_bed_Sitting_move_forward_131f", false);
		SittingMoveLegs			= new CAnimate("a_bed_Sitting_place_legs_01_76f", false);
		SittingLeanForward		= new CAnimate("a_bed_Sitting_place_legs_02_41f", false);
		SittingStandUp			= new CAnimate("a_bed_Sitting_Standup_71f", false);
		SittingMoveBack			= new CAnimate("A_bed_sitting_moving_back_106f", false);
		SittingDownOnArm		= new CAnimate("A_bed_lying_Down_on_arm_64f", false);
		DownLiftingLegs			= new CAnimate("A_bed_lying_Down_on_Back_91f", false);	
		OnBackLiftingLegs		= new CAnimate("on_back_lifting_legs_91f", false);
		BackCrawl				= new CAnimate("back_crawl_238f", false);
		BackInChair				= new CAnimate("A_tilbage_i_stol_82f", false);
		BendKnees				= new CAnimate("bend_knees_51f", false);
		LookRight				= new CAnimate("look_right_41f", false);
		ArmRight				= new CAnimate("arm_right_71f", false);
		TurnRight				= new CAnimate("turn_side_81f", false);
		LyingToSittingB			= new CAnimate("B_bed_Lying_to_Sitting_263f", false);
		BendKneesB				= new CAnimate("bend_knees_61f", false);
		TurnHeadRight			= new CAnimate("turn_head_right_56f", false);
		TurnSide				= new CAnimate("turn_side_81f", false);
		SlideMatRight			= new CAnimate("cloth_under_left_shoulder_106f", false);
		SlideMatLeft			= new CAnimate("cloth_under_right_shoulder_101f", false);
		GrapCot					= new CAnimate("grab_cot_side_61f_2011", false);
		BendKneesC				= new CAnimate("bend_knees_111f", false);
		TurnSideC_I				= new CAnimate("turn_right", false);
		PushUpB					= new CAnimate("laying_push_head_to_pillow_100f", false);
		LiftLeftShoulderB		= new CAnimate("lift_shoulder_91f", false);
		LiftRightShoulderB		= new CAnimate("lift_shoulderr_101f_2013", false);
		MoveForwardB			= new CAnimate("B_bed_Sitting_Move_forward_131f", false);
		PlaceLegsB				= new CAnimate("B_bed_Sitting_place_legs_76f", false);
		StandUpB1				= new CAnimate("B_bed_Sitting_Standup_01_131f", false);
		StandUpB2				= new CAnimate("B_bed_Sitting_Standup_02_42f", false);
		StandUpB3				= new CAnimate("B_bed_Sitting_Standup_03_107f", false);
		SittingToStandingAdd	= new CAnimate("B_bed_Sitting_Standup_03_additive", true);
		MoveForwardC			= new CAnimate("C_bed_Sitting_Move_forward_1_131f", false);
		PlaceLegsC				= new CAnimate("C_bed_Sitting_Place_Legs_71f", false);
		StandUpC				= new CAnimate("C_bed_Sitting_Standup_151f", false);
		WalkA					= new CAnimate("190_10", false);
		WalkB					= new CAnimate("200_30", false);
		CounterBedUp			= new CAnimate("counter_bed_up", true);
		SitHandsUpC				= new CAnimate("C_bed_Sitting_hands_up_1_131f", false);
		LiftLegsAdditive		= new CAnimate("lift_legs_higher_in_bed_b", false);
	}
	
	// Update is called once per frame
	void Update () 
	{					
		MoveBedEnd.update();
		MoveBedHead.update();
		MoveHead.update();
		//LiftLegsAdditive.update();
	}
	
	public void LayOnSide()
	{
		GetComponent<Animation>()["Idle_Side_100f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("Idle_Side_100f");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
	
	public void SitUp()
	{
		GetComponent<Animation>()["Idle_Sitting_100f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("Idle_Sitting_100f");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
	
	public void SitOnChair()
	{
		GetComponent<Animation>()["A_idle_chair_1f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("A_idle_chair_1f");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
	
	public void LowLay()
	{
		GetComponent<Animation>()["Idle_back__low_in_bed_100f"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("Idle_back__low_in_bed_100f");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
	
	public void Standing()
	{
		GetComponent<Animation>()["190_5"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("190_5");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
	
	public void StandingB()
	{
		GetComponent<Animation>()["200_10"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("200_10");	
		GetComponent<Animation>().Stop("Idle_back_100f");
	}
}
