﻿using UnityEngine;
using System.Collections;

public class ELearning : BaseWindow
{
    public Texture2D leftArrow;
    public Texture2D rightArrow;
    public Texture2D leftArrowDisabled;
    public Texture2D rightArrowDisabled;

    public Texture2D background;

    public string[] welcomeText;
    public Texture2D[] pictures_dk;
    public Texture2D[] pictures_se;

    private Texture2D[] pictures;

    public Rect leftButtonPos;
    public Rect rightButtonPos;

    private int position = 0;
    private string text = "";

    private bool runlearning = false;

    Message msg;

    private void GetText()
    {
        if (welcomeText.Length > 0)
        {
            if (welcomeText.Length > position - 1 && position >= 0)
                text = Text.Instance.GetStringAndPlaySpeak(welcomeText[position]);
            if (msg)
                msg.Text = text;
        }
    }

    public void TestWindow(Message msg, bool value)
    {

    }

    // Use this for initialization
    public override void WinStart()
    {
        if (Global.Instance.ProgramLanguage == "sv-SE")
            pictures = (Texture2D[])pictures_se.Clone();
        else
            pictures = (Texture2D[])pictures_dk.Clone();

        runlearning = true;
        GetText();
        msg = Util.InfoWindow(new Rect(0, 0, 800, 470), text, false, Message.Type.Info, false, false, false, TestWindow);
        BottomBarScript.EnableInfoButton(false);
        BottomBarScript.EnableHomeButton(false);
        BottomBarScript.EnableRefreshButton(false);
    }

    // Update is called once per frame
    public override void WinUpdate()
    {

    }

    void WindowTexture(int id)
    {
        DrawTexture(new Rect(0, 0, 500, 320), pictures[position]);
    }

    public override void WinOnGUI()
    {
        if (!runlearning || pictures.Length == 0)
            return;

        DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        leftButtonPos.x = (int)(26.0f * ((float)Screen.width / 980.0f));
        leftButtonPos.y = (int)(280.0f * ((float)Screen.height / 630.0f));
        rightButtonPos.x = (int)(918.0f * ((float)Screen.width / 980.0f));
        rightButtonPos.y = (int)(280.0f * ((float)Screen.height / 630.0f));

        if (Button(leftButtonPos, position == 0 ? leftArrowDisabled : leftArrow, GUIStyle.none))
        {
            if (position > 0)
            {
                position -= 1;
                GetText();
            }
        }

        if (Button(rightButtonPos, position >= welcomeText.Length ? rightArrowDisabled : rightArrow, GUIStyle.none))
        {
            if (position < welcomeText.Length - 1)
            {
                position += 1;
                GetText();
            }
            else
            {
                Global.Instance.updateScore(3.0);
                SceneLoader.Instance.CurrentScene = 0;
                BottomBarScript.EnableRefreshButton(true);
                runlearning = false;
            }
        }

        GUI.Window(1, new Rect(Screen.width / 2.0f - 250, Screen.height / 2.0f - 100, 500, 320), WindowTexture, "");
    }
}
