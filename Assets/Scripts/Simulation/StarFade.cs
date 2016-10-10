using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StarFade : BaseWindow 
{
    private static StarFade _instance; //singleton

    public static StarFade Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (StarFade)GameObject.FindObjectOfType(typeof(StarFade));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "StarFade";
                    _instance = (StarFade)container.AddComponent(typeof(StarFade));
                }
            }

            return _instance;
        }
    }

    /*
    private bool _mpfoot = true;
    private Texture2D _mpfoottexture;
    private float _mpfooty;
    private bool _mpplay = false;
     */

    private float TimeToWait = 0.8f;
    private float FadeTime = 1.5f;
    private float FadeResultTime = 1.2f;
    private float StarSize = 80.0f;

    private Texture2D starBackground;
    private Texture2D starForground;

    private bool fadeStar = false;
    private bool timedFade = false;
    private bool results = false;

    private Rect starPos;

    private float f = 1.0f;
    private float s = 1.0f;
    private float delay;

    private float[] sf = { 0.0f, 0.0f, 0.0f };

    private float screenWidth;
    private float screenHeight;

    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        starBackground = (Texture2D)Resources.Load("star-deactivated");
        starForground = (Texture2D)Resources.Load("star-activated");
        starPos = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        delay = TimeToWait;

        /*
        if (_mpfoot)
        {
            _mpfooty = ((float)Screen.height * 0.8f) * -1.0f;
            _mpfoottexture = (Texture2D)Resources.Load("mpfoot");
            audio.clip = (AudioClip)Resources.Load("mpfart");
        }
         */
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void ShowStar(Rect pos, bool r)
    {
        starPos = pos;
        fadeStar = true;
        results = r;
    }

    public void ShowStar(Rect pos, bool r, float d)
    {
        starPos = pos;
        results = r;
        StartCoroutine(DelayStars(d));
    }

    private IEnumerator DelayStars(float delay)
    {
        yield return new WaitForSeconds(delay);
        fadeStar = true;
    }

    public void HideStar()
    {
        Object.Destroy(this);
    }

	// Use this for initialization
    public override void WinStart() 
    {
	
	}

    public override void WinUpdate()
    {
        /*
        if (_mpfoot)
        {
            if (Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.F))
            {
                States.Instance.PushState("MPFOOT", "yes");
                Debug.Log("MPFOOT");
            }
        }
         */

        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            starPos = new Rect((Screen.width * 0.5f) - starPos.width, (Screen.height * 0.5f) - starPos.height, starPos.width, starPos.height);
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        if (!results)
        {
            if (fadeStar && delay > 0.0f)
                delay -= Time.deltaTime;
            else
                timedFade = true;

            if (timedFade && f >= 0.0f)
            {
                s += StarSize * Time.deltaTime;
                f -= FadeTime * Time.deltaTime;
            }
        }
        else
        {
            for (int i = 0; i < sf.Length; ++i)
            {
                if (i == 0)
                {
                    if (sf[i] <= 1.0f)
                        sf[i] += FadeResultTime * Time.deltaTime;
                    else
                        sf[i] = 1.0f;
                }
                else
                {
                    if (sf[i - 1] >= 1.0f)
                    {
                        if (sf[i] <= 1.0f)
                            sf[i] += FadeResultTime * Time.deltaTime;
                        else
                            sf[i] = 1.0f;
                    }
                }
            }
        }
    }

    public override void WinOnGUI()
    {
        /*
        if (_mpfoot)
        {
            if (fadeStar && Results.Instance.GetScore() <= 0 && States.Instance.GetStateValue("MPFOOT") == "yes")
            {
                int width = (int)((float)Screen.height * 0.8f);
                int height = width;
                int x = (int)((float)Screen.width / 2.0f - width / 2.0f);
                int y = (int)_mpfooty;


                DrawTexture(new Rect(x, y, width, height), _mpfoottexture);
                
                int bottomheight = y + height;

                if (_mpfooty < (Screen.height - 50 - height))
                {
                    _mpfooty += 10.05f;
                }
                else
                {
                    if (!_mpplay)
                    {
                        audio.Play();
                        _mpplay = true;
                    }
                }
            }
        }
         */

        if (!results)
        {
            if (fadeStar)
            {
                int x = 0;
                for (int i = 0; i < Global.Instance.MaxStars; ++i)
                {
                    DrawTexture(new Rect(starPos.x + x, starPos.y, 25.0f, 25.0f), i < Results.Instance.GetScore() ? starForground : starBackground);

                    if (i == Results.Instance.GetScore())
                    {
                        GUI.color = new Color(1.0f, 1.0f, 1.0f, f);
                        DrawTexture(new Rect((starPos.x + x) - (s / 2.0f), starPos.y - (s / 1.5f), 25.0f + s, 25.0f + s), starForground);
                        GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }

                    x += 28;
                }
            }
        }
        else
        {
            if (fadeStar)
            {
                int x = 0;
                for (int i = 0; i < Global.Instance.MaxStars; ++i)
                {
                    if (i < Results.Instance.GetScore())
                    {
                        GUI.color = new Color(1.0f, 1.0f, 1.0f, sf[i]);
                        DrawTexture(new Rect(starPos.x + x, starPos.y, 25.0f, 25.0f), starForground);
                        GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }

                    x += 28;
                }
            }
        }
    }
}
