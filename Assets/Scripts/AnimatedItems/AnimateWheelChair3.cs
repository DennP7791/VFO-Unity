using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateWheelChair3 : MonoBehaviour
{
    public class CAnimate
    {
        AnimationState s;
        float animFps;
        bool additive = false;
        float animFadeTime;
        string callname;

        public CAnimate(string name, string animName, int layer, AnimationBlendMode blendMode, float weight, float fps, float fadeTime)
        {
            s = AnimateWheelChair3.Instance.GetComponent<Animation>()[animName];

            s.wrapMode = WrapMode.ClampForever;
            s.blendMode = blendMode;
            s.weight = weight;
            s.layer = layer;

            animFadeTime = fadeTime;
            animFps = fps;

            callname = name;

            if (blendMode == AnimationBlendMode.Additive)
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
            if (additive && !s.enabled)
            {
                s.enabled = true;
            }
            else
            {
                AnimateWheelChair3.Instance.GetComponent<Animation>().CrossFade(s.name, animFadeTime);
            }
        }

        public void Update()
        {
            if (additive && s.enabled)
            {
                s.time += Time.deltaTime / animFps;
            }
        }
    }

    private static AnimateWheelChair3 _instance;
    public static AnimateWheelChair3 Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateWheelChair3)GameObject.FindObjectOfType(typeof(AnimateWheelChair3));

                if (!_instance)
                {
                    Debug.LogError("AnimateWheelChair3 instance could not be found, make sure to add a walker to the scene, and add the AnimateWheelChair3 script to it");
                }
            }

            return _instance;
        }
    }

    private List<string> animationName = new List<string>();
    private List<CAnimate> cAnimation = new List<CAnimate>();
    private List<string> animStates = new List<string>();
    private List<float> animDelays = new List<float>();
    private List<string> resetStates = new List<string>();
    private string idleAnim = "";
    private string currentState = "";

    public void SetIdle(string animName)
    {
        GetComponent<Animation>()[animName].wrapMode = WrapMode.Loop;
        GetComponent<Animation>().Play(animName);
        if (idleAnim != animName)
        {
            GetComponent<Animation>().Stop(idleAnim);
            idleAnim = animName;
        }
    }

    public void AddAnimation(string[] statearr, string name, string animName, float delay, bool additive, int layer)
    {
        try
        {
            AddAnimation(statearr, name, animName, delay, additive, layer, GetComponent<Animation>()[animName].weight, 25.0f, 2.0f);
        }
        catch (System.Exception e)
        {
            Debug.LogError("AnimateWheelChair3 - Couldn't find animation named: " + animName);
        }
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

    public void AddResetState(string[] statearr)
    {
        for (int i = 0; i < statearr.Length; ++i)
            resetStates.Add(statearr[i]);
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

            int pos2 = resetStates.IndexOf(state);
            if (pos2 != -1)
            {
                ResetPositionAndRotation();
                /*int _p = state.IndexOf("#");
                if (_p != -1)
                {
                    string _objName = state.Substring(0, _p);
                    string _idxs = state.Substring(_p + 1, 1);
                    GameObject go = GameObject.Find(_objName);
                    if (go)
                    {
                        go.GetComponent<HUD>().Buttons[int.Parse(_idxs)].Correct = false;
                        go.GetComponent<HUD>().Buttons[int.Parse(_idxs)].Disabled = true;
                    }
                }*/ 
            }
        }
    }

    public void StartAnimation(string name)
    {
        StartCoroutine(StartAnimationTimed(name, 0.0f));
    }

    public void StartAnimation(string name, float delay)
    {
        StartCoroutine(StartAnimationTimed(name, delay));
    }

    public void ResetPositionAndRotation()
    {
        transform.position = Vector3.zero;
       // transform.rotation = Quaternion.identity;
    }

    private IEnumerator StartAnimationTimed(string name, float delay)
    {
        yield return new WaitForSeconds(delay);

        int pos = animationName.IndexOf(name);
        if (pos != -1)
        {
            cAnimation[pos].StartAnim();
        }
        else
        {
            Debug.LogError("Animation with the name: " + name + " could not be started");
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CAnimate c in cAnimation)
        {
            c.Update();
        }
    }
}
