using UnityEngine;
using System.Collections;

public class AnimateBed : MonoBehaviour 
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
			anim = AnimateBed.Instance.GetComponent<Animation>()[animationName];
			bakedAnim = baked;

			anim.layer = AnimateBed.Instance.layer;
			AnimateBed.Instance.layer++;
			anim.wrapMode = WrapMode.ClampForever;
			anim.blendMode = AnimationBlendMode.Additive;
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
				
				if(normalizedTime >= 0.99f) {
					if(!States.Instance.GetStateValueB(anim.name + "_down"))
					{
						States.Instance.PushState(anim.name + "_down", "yes");
						States.Instance.PushState(anim.name + "_middle", "no");
						States.Instance.PushState(anim.name + "_up", "no");
						GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
						if(go) go.SendMessage("SimCallback", anim.name + "_down");
					}
				} else if(normalizedTime < 0.99f && normalizedTime > 0.1f) {
					if(!States.Instance.GetStateValueB(anim.name + "_middle"))
					{
						States.Instance.PushState(anim.name + "_middle", "yes");
						States.Instance.PushState(anim.name + "_down", "no");
						States.Instance.PushState(anim.name + "_up", "no");
						GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
						if(go) go.SendMessage("SimCallback", anim.name + "_middle");
					}
				} else {
					if(!States.Instance.GetStateValueB(anim.name + "_up"))
					{
						States.Instance.PushState(anim.name + "_down", "no");
						States.Instance.PushState(anim.name + "_middle", "no");
						States.Instance.PushState(anim.name + "_up", "yes");
						GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
						if(go) go.SendMessage("SimCallback", anim.name + "_up");
					}
				}
			}
			else
			{
				if(anim.enabled)
				{
					if(moveSpeed > 0.0f && anim.normalizedTime < 1.0f) normalizedTime += (Time.deltaTime / 10) * animSpeed;
					else if(moveSpeed < 0.0f && anim.normalizedTime > 0.0f) normalizedTime -= (Time.deltaTime / 10) * animSpeed;
				}
			}
		}
		
		private IEnumerator StartAnimTimed()
		{
			yield return new WaitForSeconds(delay);
			
			if(bakedAnim)
			{
				if(upDown) {
					normalizedTime = normalizedTime >= 1.0f ? 1.0f : normalizedTime + (1.0f / (float)AnimateBed.Instance.MoveAmounts);
					moveSpeed = animSpeed;
					if(normalizedTime < 1.0f) AnimateBed.Instance.GetComponent<Animation>().Play(anim.name);
				}
				else {
					normalizedTime = normalizedTime <= 0.0f ? 0.0f : normalizedTime - (1.0f / (float)AnimateBed.Instance.MoveAmounts);
					moveSpeed = -animSpeed;
					if(normalizedTime > 0.0f) AnimateBed.Instance.GetComponent<Animation>().Play(anim.name);
				};
			}
			else
			{
				moveSpeed = upDown ? 1.0f : -1.0f;
				if(!anim.enabled) anim.enabled = true;
			}
		}
		
		public void StartAnimation(bool u, float s, float d)
		{
			upDown = u;
			animSpeed = s;
			delay = d;
			AnimateBed.Instance.StartCoroutine(StartAnimTimed());
		}
		
		public AnimationState AnimState
		{
			get { return anim; }
			set { anim = value; }
		}
	}

	private static AnimateBed _instance; //singleton
		
	private int MoveAmounts 				= 2; // Numbers of steps the bed can move
	
	private int layer = 11;
	
	public CAnimate MoveBedEnd;
	public CAnimate MoveBedHead;
	public CAnimate MoveBedHeight;
	//public CAnimate MoveBedRailing;
			
	public static AnimateBed Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateBed)GameObject.FindObjectOfType(typeof(AnimateBed));

                if (!_instance)
                {
                    Debug.LogError("Bed instance could not be found, make sure to add a bed to the scene, and add the AnimateBed script to it");
                } 
            }

            return _instance;
        }
    }
		
	// Use this for initialization
	void Start () {
		
		GetComponent<Animation>()["base"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>().Play("base");
		
		MoveBedEnd = new CAnimate("foot_end_pos_high", true);
		MoveBedHead = new CAnimate("head_end_pos_high", true);
		MoveBedHeight = new CAnimate("pos_low", true);
		//MoveBedRailing = new CAnimate("rail_down", true);
	}
	
	// Update is called once per frame
	void Update () 
	{			
		MoveBedEnd.update();
		MoveBedHead.update();
		MoveBedHeight.update();
//		MoveBedRailing.update();
	}	
}
