using UnityEngine;
using System.Collections;

public class ResolutionCheck : MonoBehaviour {

	// Use this for initialization
    private int oldResX, oldResY;
    private bool fullscreen = false;
    private bool fullscreencheck = false;
    private bool mobileDevice = false;

    private float timer = 0.0f;
    private float checktime = 1.0f;
    private int framecount = 0;

    private Message msg;

	void Awake () 
    {
        DontDestroyOnLoad(transform.gameObject);
	}

    public void CheckResolution(string res)
    {
        int x, y = 0;

        string[] sx = res.Split(new char[] {' '});
        if (sx.Length == 2)
        {
            bool t1 = int.TryParse(sx[0], out x);
            bool t2 = int.TryParse(sx[1], out y);

#if UNITY_ANDROID || UNITY_IOS
            mobileDevice = true;
#endif

            if(t1 && t2)
            {
                if ((x < 960 || y < 630) && !fullscreencheck && !Screen.fullScreen && !mobileDevice)
                {
                    oldResX = x;
                    oldResY = y;
                    fullscreencheck = true;
                    msg = Util.OkMessageBox(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), Text.Instance.GetString("resolution_check_too_low"), true, Message.Type.Warning, OkClicked);
                }
                else if (x >= 960 && y >= 630)
                {
                    oldResX = x;
                    oldResY = y;
                    if (msg && msg.Visible)
                    {
                        fullscreencheck = false;
                        msg.Destroy();
                    }
                }
            }
        }
    }

    public void OkClicked(Message message, bool value)
    {
        int x = 0, y = 0;
        Resolution[] res = Screen.resolutions;

        foreach (Resolution r in res)
        {
            if (r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height)
            {
                x = r.width;
                y = r.height;
            }
        }

        if (x != 0 && y != 0)
        {
            framecount = Time.frameCount;
            fullscreen = true;
            Screen.SetResolution(x, y, true);
        }
        else
        {
            fullscreen = false;
            Util.MessageBox(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), Text.Instance.GetString("resolution_check_can_not_change"), Message.Type.Warning, true, true);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        timer += Time.deltaTime;

        if (timer > checktime)
        {
            timer = 0.0f;
            CheckResolution(Screen.width.ToString() + " " + Screen.height.ToString());
        }

        if (fullscreen && !Screen.fullScreen && Time.frameCount > framecount+2)
        {
            fullscreen = false;
            fullscreencheck = false;
            Application.ExternalEval("window.location.href=window.location.href;");
        }
	}
}
