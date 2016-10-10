using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimateAntiSlideMat : MonoBehaviour
{
    public class CAnimate
    {
        AnimationState s;
        float animFps;
        bool additive = false;
        float animFadeTime;
        string callname;
        float additiveWeight = 1.0f;

        public CAnimate(string name, string animName, int layer, AnimationBlendMode blendMode, float weight, float fps, float fadeTime)
        {
            s = AnimateAntiSlideMat.Instance.GetComponent<Animation>()[animName];

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
                additiveWeight = weight;
            }
        }

        public int Layer
        {
            get { return s.layer; }
        }

        public void StartAnim()
        {
            if (additive)
            {
                AnimateAntiSlideMat.Instance.GetComponent<Animation>().Blend(s.name, additiveWeight, animFadeTime);
            }
            else
            {
                AnimateAntiSlideMat.Instance.GetComponent<Animation>().CrossFade(s.name, animFadeTime);
            }
        }

        public void Update()
        {
          
        }
    }

    private static AnimateAntiSlideMat _instance;
    public static AnimateAntiSlideMat Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (AnimateAntiSlideMat)GameObject.FindObjectOfType(typeof(AnimateAntiSlideMat));

                if (!_instance)
                {
                    Debug.LogError("AnimateAntiSlideMat instance could not be found, make sure to add a bed to the scene, and add the AnimateAntiSlideMat script to it");
                }
            }

            return _instance;
        }
    }

    private List<string> animationName = new List<string>();
    private List<CAnimate> cAnimation = new List<CAnimate>();
    private List<string> animStates = new List<string>();
    private List<float> animDelays = new List<float>();
    private List<string> removeStates = new List<string>();
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

    public void AddRemoveState(string[] statearr)
    {
        for (int i = 0; i < statearr.Length; ++i)
        {
            removeStates.Add(statearr[i]);
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

            int pos = animStates.IndexOf(state);
            if (pos != -1)
            {
                string n = animationName[pos];
                float d = animDelays[pos];
                StartAnimation(n, d);
            }

            int pos2 = removeStates.IndexOf(state);
            if (pos2 != -1)
            {
                GetComponent<Animation>().Stop();
                gameObject.transform.position =  new Vector3(10000.0f, 0.0f, 0.0f);
            }

            int pos3 = resetStates.IndexOf(state);
            if (pos3 != -1)
            {
                ResetPositionAndRotation();
                int _p = state.IndexOf("#");
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
                }
            }
        }
    }

    // Depriciated
    public void AddAnimation(string name, string animName, bool additive, int layer)
    {
        AddAnimation(name, animName, additive, layer, GetComponent<Animation>()[animName].weight, 25.0f, 2.0f);
    }

    // Depriciated
    public void AddAnimation(string name, string animName, bool addtive, int layer, float weight)
    {
        AddAnimation(name, animName, addtive, layer, weight, 25.0f, 2.0f);
    }

    // Depriciated
    public void AddAnimation(string name, string animName, bool additive, int layer, float weight, float fadeTime)
    {
        if (weight == -1.0f)
            weight = GetComponent<Animation>()[animName].weight;
        AddAnimation(name, animName, additive, layer, weight, 25.0f, fadeTime);
    }

    // Depriciated
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

    public void ResetPositionAndRotation()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
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
