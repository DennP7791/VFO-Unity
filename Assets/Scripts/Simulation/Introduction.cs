using UnityEngine;
using System.Collections;

public class Introduction : BaseWindow
{

    public Texture2D[] introductionImages;

    public Texture2D leftArrow;
    public Texture2D rightArrow;

    public Texture2D debugClickArea;

    public Rect[] arrows;
    public bool[] useLeftArrow;
    public Rect[] clickArea;

    public bool debugClick;

    private int steps = 0;
    private int lastStep = -1;
    private float time = 0.0f;
    private float alpha = 0.0f;

    private float xPos;
    private float yPos;
    private float screenWidth;
    private float screenHeight;

	// Use this for initialization
	public override void WinStart () 
    {
        BottomBarScript.EnableInfoButton(false);
        BottomBarScript.EnableHomeButton(false);
        BottomBarScript.EnableRefreshButton(false);

        if( SceneLoader.Instance.CurrentScene == -1)
        {
            GameObject.Instantiate((GameObject)Resources.Load("BottomBar"));
            GameObject.Instantiate((GameObject)Resources.Load("TopBar"));
        }

        xPos = ((float)Screen.width * 0.5f) - ((float)introductionImages[steps].width * 0.5f);
        yPos = ((float)Screen.height * 0.5f) - ((float)introductionImages[steps].height * 0.5f) + 10;
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Help.Instance.ShowHelpText();

        Help.Instance.AddHelpText(new string[] { "0" }, "introduction_guide_0");
        Help.Instance.AddHelpText(new string[] { "1" }, "introduction_guide_1");
        Help.Instance.AddHelpText(new string[] { "2" }, "introduction_guide_2");
        Help.Instance.AddHelpText(new string[] { "3" }, "introduction_guide_3");
        Help.Instance.AddHelpText(new string[] { "4" }, "introduction_guide_4");
        Help.Instance.AddHelpText(new string[] { "5" }, "introduction_guide_5");
        Help.Instance.AddHelpText(new string[] { "6" }, "introduction_guide_6");
        Help.Instance.AddHelpText(new string[] { "7" }, "introduction_guide_7");
        Help.Instance.AddHelpText(new string[] { "8" }, "introduction_guide_8");
        Help.Instance.AddHelpText(new string[] { "9" }, "introduction_guide_9");
        Help.Instance.AddHelpText(new string[] { "10" }, "introduction_guide_10");
        Help.Instance.AddHelpText(new string[] { "11" }, "introduction_guide_11");
        Help.Instance.AddHelpText(new string[] { "12" }, "introduction_guide_12");
        Help.Instance.AddHelpText(new string[] { "13" }, "introduction_guide_13");
        Help.Instance.AddHelpText(new string[] { "14" }, "introduction_guide_14");
        Help.Instance.AddHelpText(new string[] { "15" }, "introduction_guide_15");

        helpSteps(steps.ToString());
	}


	// Update is called once per frame
	public override void WinUpdate () 
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            xPos = ((float)Screen.width * 0.5f) - ((float)introductionImages[steps].width * 0.5f);
            yPos = ((float)Screen.height * 0.5f) - ((float)introductionImages[steps].height * 0.5f) + 10;
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        if (lastStep != steps)
        { 
            time += Time.deltaTime;
            if (time >= 1.0f)
            {
                alpha += Time.deltaTime * 0.4f;
                if (alpha >= 1.0f)
                {
                    time = 0.0f;
                    lastStep = steps;
                    alpha = 1.0f;
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mpos = Input.mousePosition;
            mpos.y = Screen.height - mpos.y;
            Rect c = clickArea[steps];
            c.x += xPos;
            c.y += yPos;
            if (c.Contains(mpos))
            {
                int s = steps + 1;
                if (s < introductionImages.Length)
                {
                    steps = s;
                    alpha = 0.0f;
                    helpSteps(steps.ToString());
                }
                else
                {
                    BottomBarScript.EnableRefreshButton(true);
                    Text.Instance.StopAudio();
                    Global.Instance.updateScore(3.0);
                    SceneLoader.Instance.CurrentScene = 0;
                }
            }
        }
	}

    void helpSteps(string steps)
    {
        Help.Instance.UpdateHelp(steps);
    }

    public override void WinOnGUI()
    {
        if (steps >= 0)
        {
            DrawTexture(new Rect(xPos, yPos, (float)introductionImages[steps].width, (float)introductionImages[steps].height), introductionImages[steps]);

            GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            Rect r = arrows[steps];
            r.x += xPos;
            r.y += yPos;
            DrawTexture(r, useLeftArrow[steps] ? leftArrow : rightArrow);
            GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (debugClick)
            {
                Rect c = clickArea[steps];
                c.x += xPos;
                c.y += yPos;
                DrawTexture(c, debugClickArea);
            }
        }
    }
}
