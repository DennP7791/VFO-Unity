using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateBed3 : MonoBehaviour
{

    // callback from the remote
    public void MoveBedHeadUp() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedHeadUp");
    }

    public void MoveBedHeadDown() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedHeadDown");
    }

    public void MoveBedEndUp() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedEndUp");
    }

    public void MoveBedEndDown() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedEndDown");
    }

    public void MoveBedDown() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedHeightDown");
    }

    public void MoveBedUp() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedHeightUp");
    }

    public void MoveBedSittingUp() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedSittingUp");
    }

    public void MoveBedSittingDown() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedSittingDown");
    }

    public void MoveBedSittingLegsUp() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedSittingLegsUp");
    }

    public void MoveBedSittingLegsDown() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedSittingLegsDown");
    }

    public void MoveBedRailingDownL() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedRailingLeft");
    }

    public void MoveBedRailingUpL() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedRailingLeft");
    }

    public void MoveBedRailingDownR() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedRailingRight");
    }

    public void MoveBedRailingUpR() {
        GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
        if (go) go.SendMessage("SimCallback", "BedRailingRight");
    }

    private static AnimateBed3 _instance;
    public static AnimateBed3 Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateBed3)GameObject.FindObjectOfType(typeof(AnimateBed3));

                if (!_instance)
                {
                    Debug.LogError("AnimateBed3 instance could not be found, make sure to add a bed to the scene, and add the AnimateBed3 script to it");
                }
            }

            return _instance;
        }
    }

    private List<AnimationState> anim = new List<AnimationState>();
    private List<string> animName = new List<string>();
    private List<float> animTime = new List<float>();
    private List<string> animStates = new List<string>();
    private List<float> animDelays = new List<float>();
    private List<float> animWeight = new List<float>();

    private int layer = 10;
    
    private string idleAnim = "";
    private string currentState = "";

    public void SetIdle(string an)
    {
        GetComponent<Animation>()[an].wrapMode = WrapMode.Loop;
        GetComponent<Animation>().Play(an);
        if (idleAnim != an)
        {
            GetComponent<Animation>().Stop(idleAnim);
            idleAnim = an;
        }
    }

    public void AddAnimation(string[] statearr, string name, float weight, float blendtime)
    {
        for (int i = 0; i < statearr.Length; ++i)
        {
            AnimationState s = GetComponent<Animation>()[name];
            s.weight = 0.0f;
            s.blendMode = AnimationBlendMode.Additive;
            s.wrapMode = WrapMode.ClampForever;
            s.layer = layer;

            anim.Add(s);

            layer++;

            animStates.Add(statearr[i]);
            animDelays.Add(blendtime);
            animWeight.Add(weight);
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
                    GetComponent<Animation>().Blend(anim[i].name, animWeight[i], animDelays[i]);
                }
            }
        }
    }
}
