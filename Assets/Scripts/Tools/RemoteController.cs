using UnityEngine;
using System.Collections;

public class RemoteController : BaseWindow
{

    public bool visible = false;
    public Texture2D texture;
    public Rect rect = new Rect(0, 0, 100, 200);

    public Rect buttonRect1_1 = new Rect(0, 0, 50, 50);
    public Rect buttonRect1_2 = new Rect(50, 0, 50, 50);
    public Rect buttonRect2_1 = new Rect(0, 50, 50, 50);
    public Rect buttonRect2_2 = new Rect(50, 50, 50, 50);
    public Rect buttonRect3_1 = new Rect(0, 100, 50, 50);
    public Rect buttonRect3_2 = new Rect(50, 100, 50, 50);

    private GameObject bed;


    private GUIStyle style = new GUIStyle();

    // Use this for initialization
    public override void WinStart()
    {
        bed = GameObject.Find("Main Camera");
        if (texture)
        {
            style.normal.background = texture;
        }
        else
        {
            Debug.LogWarning("texture is not set.");
        }
    }

    public override void WinOnGUI()
    {
        if (texture && visible)
        {
            //GUI.Box(rect, "", style);
            Content();
        }
    }
	
	
	
    void Content()
    {
        Position = rect;
        Box(new Rect(0, 0, Position.width, Position.height), "", style);

        if (Button(buttonRect1_1, "", GUIStyle.none))
        {
            bed.SendMessage("Animation1_1");
            Debug.Log("1_1");
        }
        if (Button(buttonRect1_2, "", GUIStyle.none))
        {
            bed.SendMessage("Animation1_2");
            Debug.Log("1_2");
        }
        if (Button(buttonRect2_1, "", GUIStyle.none))
        {
            bed.SendMessage("Animation2_1");
            Debug.Log("2_1");
        }
        if (Button(buttonRect2_2, "", GUIStyle.none))
        {
            bed.SendMessage("Animation2_2");
            Debug.Log("2_2");
        }
        if (Button(buttonRect3_1, "", GUIStyle.none))
        {
            bed.SendMessage("Animation3_1");
            Debug.Log("3_1");
        }
        if (Button(buttonRect3_2, "", GUIStyle.none))
        {
            bed.SendMessage("Animation3_2");
            Debug.Log("3_2");
        }
    }
	
	// Update is called once per frame
	public override void WinUpdate () {
	
	}
}
