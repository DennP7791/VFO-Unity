using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
    Message message;

    void TestHelp()
    {
        Debug.Log("helpbox yeah!");
    }

    void TestMessage(Message message, bool value)
    {
        if(value)
            Debug.Log("This is only a test 2");
    }

    void HelpBoxTest()
    {
        Debug.Log("HelpBoxTest");
        HelpBox res = Util.HelpBox("this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox  this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpboxis an helpbox this is an helpbox this is an helpbox this is an helpbox this is an helpbox  ", TestHelp);
    }

    void FeedbackWindowTest()
    {
        Debug.Log("FeedBackWindowTest");
        //message = Util.FeedbackWindow(new Rect(100, 100, 400, 300), "Please leave us a feedback");
    }

    void MessageBoxTest()
    {
        Debug.Log("MessageBoxTest");
        message = Util.MessageBox(new Rect(100, 100, 400, 300), "something", Message.Type.Info, true, true);
        message.textStyle.fontSize = 30;
    }

    void CancellableMessageBoxTest()
    {
        Debug.Log("CancellableMessageBoxTest");
        message = Util.CancellableMessageBox(new Rect(500, 100, 400, 300), "message", Message.Type.Warning, TestMessage);
    }

    void OkMessageBoxTest()
    {
        Debug.Log("OkMessageBoxTest");
        message = Util.OkMessageBox(new Rect(100, 100, 400, 300), "message", true, Message.Type.Info, TestMessage);
    }

    void RemoteToolTest()
    {
        Debug.Log("RemoteToolTest");
        //Global.Instance.DisplayRemote(true);
    }

    void InfoWindowTest()
    {
        Debug.Log("InfoWindowTest");
        Util.InfoWindow(new Rect(0, 0, 400, 300), "This is an info window!");
        Util.ToggleResource<Message>("InfoWindow");
    }

	void Start () {

        //Util.SetToolTipText("this is a test");
        //FeedbackWindowTest();
        //CancellableMessageBoxTest();
        //Util.CancellableMessageBox(new Rect(300, 0, 400, 300), "message", Message.Type.Warning, TestMessage);
        //HelpBoxTest();


        //HUDTiled boxHUD = Util.InstantiateResource<HUDTiled>("HUD_Box");

        HelpBoxTest();
        //Util.InstantiateResource<Box>("Box");

        //InfoWindowTest();
        //BottomBarScript.DisableInfoButton();

        //HUDTiled bs = Util.InstantiateResource<HUDTiled>("HUD_Bedsheet");
        //if (bs)
        //{
        //    bs.Buttons[(int)BedSheet.Position.THIGH].Correct = true;
        //}

        //ToolGrid tg = Util.InstantiateResource<ToolGrid>("ToolGrid");
        //if (tg)
        //{
        //    //tg.SetToolCorrectness("Spielerdug", true);
        //    tg.SetToolCorrectness("Kontrol", true);
        //    //tg.SetToolCorrectness("Skridsikker", true);
        //}

        //Global.Instance.updateScore(0);

        //CancellableMessageBoxTest();

        //Debug.Log("toggling");
        //GameObject go = GameObject.Find("Bed");
        //if (go != null)
        //{
        //    Util.ToggleSubElementRenderer(go, "bottom_left_slide");
        //    Util.ToggleSubElementRenderer(go, "bottom_right_slide");
        //    Util.ToggleSubElementRenderer(go, "up_left_slide");
        //    Util.ToggleSubElementRenderer(go, "up_right_slide");
        //    Util.ToggleSubElementRenderer(go, "antislide");
        //}

        //HUD hud = Util.ToggleResource<HUD>("HUD_SlideMat");
        //hud.Buttons[(int)SlideMat.Position.BOTTOMLEFT].Correct = true;


        //Util.ToggleResource<HUD>("HUD_Helper");
        //Util.ToggleResource<HUD>("HUD_Helper");
        //HUD hud = Util.ToggleResource<HUD>("HUD_SlideMat");
        //if (hud)
        //{
        //    hud.Buttons[(int)SlideMat.Position.TOPLEFT].Correct = true;
        //    hud.Buttons[(int)SlideMat.Position.TOPRIGHT].Correct = true;
        //    hud.Buttons[(int)SlideMat.Position.BOTTOMLEFT].Correct = true;
        //    hud.Buttons[(int)SlideMat.Position.BOTTOMRIGHT].Correct = true;
        //}

        //HUDTiled hud = Util.ToggleResource<HUDTiled>("HUD_BedSheet2");
        //if (hud)
        //{
        //    hud.Buttons[(int)BedSheet.Position.BEDSIDE].Correct = true;
        //}

        //ExerciseCollections.ExerciseCategoryCollection ec = new ExerciseCollections.ExerciseCategoryCollection();
        //ec.Add(new ExerciseCollections.ExerciseCategory("asd"));
        //AntiSlideMat mat0 = Util.InstantiateResource<AntiSlideMat>("AntiSlideMat");
        //mat0.SetPosition(AntiSlideMat.Position.TOP);
        //AntiSlideMat mat1 = Util.InstantiateResource<AntiSlideMat>("AntiSlideMat");
        //mat1.SetPosition(AntiSlideMat.Position.CENTER);
        //AntiSlideMat mat2 = Util.InstantiateResource<AntiSlideMat>("AntiSlideMat");
        //mat2.SetPosition(AntiSlideMat.Position.BOTTOM);

        //SlideMat mat0 = Util.InstantiateResource<SlideMat>("SlideMat");
        //mat0.SetPosition(SlideMat.Position.BOTTOMLEFT);
        //SlideMat mat1 = Util.InstantiateResource<SlideMat>("SlideMat");
        //mat1.SetPosition(SlideMat.Position.BOTTOMRIGHT);
        //SlideMat mat2 = Util.InstantiateResource<SlideMat>("SlideMat");
        //mat2.SetPosition(SlideMat.Position.TOPLEFT);
        //SlideMat mat3 = Util.InstantiateResource<SlideMat>("SlideMat");
        //mat3.SetPosition(SlideMat.Position.TOPRIGHT);

        //Walker w0 = Util.InstantiateResource<Walker>("Walker");
        //w0.SetPosition(Walker.Position.ADJACENT);
        //Walker w1 = Util.InstantiateResource<Walker>("Walker");
        //w1.SetPosition(Walker.Position.CLOSE);
        //Walker w2 = Util.InstantiateResource<Walker>("Walker");
        //w2.SetPosition(Walker.Position.FAR);
	}

    void OnGUI()
    {

    }

    void Awake()
    {
    }

	// Update is called once per frame
	void Update () {

	}
}
