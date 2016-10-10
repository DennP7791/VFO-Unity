using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Higher_up_borger_c : MonoBehaviour 
{	
	private void initializeExercise()
	{
        ToolBox tb = Util.ToggleResource<ToolBox>("ToolBox");
        if (tb)
        {
            Global.Instance.toolButtonArray = new ToolButton[0];
        }

        ToolGrid tg = Util.ToggleResource<ToolGrid>("ToolGrid");
        if (tg)
        {
            tg.SetToolCorrectness("Spielerdug", true);
            tg.SetToolCorrectness("Skridsikker", true);
            tg.SetToolCorrectness("Lagen", true);
        }
        Util.ToggleResource<ToolGrid>("ToolGrid");

        HUD hud = Util.ToggleResource<HUD>("HUD_SlideMat2");
        if (hud)
        {
            hud.Buttons[(int)SlideMat2.Position.TOPRIGHT].Correct = true;
            hud.Buttons[(int)SlideMat2.Position.TOPLEFT].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_SlideMat2");

        HUD shud = Util.ToggleResource<HUD>("HUD_AntiSlideMat2");
        if (shud)
        {
            shud.Buttons[(int)AntiSlideMat.Position.BOTTOM].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_AntiSlideMat2");

        HUDTiled shud2 = Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");
        if (shud2)
        {
            shud2.Buttons[(int)BedSheet.Position.THIGH].Correct = true;
        }
        Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");

        GameObject go = GameObject.Find("Bed");
        if (go != null)
        {
            Util.ToggleSubElementRenderer(go, "bottom_left_slide");
            Util.ToggleSubElementRenderer(go, "bottom_right_slide");
            Util.ToggleSubElementRenderer(go, "up_left_slide");
            Util.ToggleSubElementRenderer(go, "up_right_slide");
            Util.ToggleSubElementRenderer(go, "antislide");
        }

	}

    private void defineExercise()
    {
        // Exercise States        
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_8"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_9"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMat2(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMat2(Clone)#0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_Bedsheet2(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_c_state_6"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, "", 6.0f);
        
        // Help text (husk at sengekontrollen ikke skal bruges mere)
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_high_up_c_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_high_up_c_help_8");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_high_up_c_help_9");
        Help.Instance.AddHelpText(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "sim_high_up_c_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_high_up_c_help_3");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMat2(Clone)#1" }, "sim_high_up_c_help_4");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_high_up_c_help_5");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMat2(Clone)#0" }, "sim_high_up_c_help_6");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_high_up_c_help_65");
        Help.Instance.AddHelpText(new string[] { "HUD_Bedsheet2(Clone)#1" }, "sim_high_up_c_help_10");

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_high_up_c_talk_4");
        Talk.Instance.AddTalkDialog(pos1, "sim_high_up_c_talk_1");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#1", pos1, "sim_high_up_c_talk_2");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#0", pos1, "sim_high_up_c_talk_3");
        Talk.Instance.AddTalkDialog(pos1, "sim_high_up_c_talk_5");

        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("500_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "60_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "60_30", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PlaceSlideMat1", "500_40", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "60_50", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "TurnBack", "60_65", 0.0f, false, 60);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_Bedsheet2(Clone)#1" }, "PlaceBedSheet", "60_70", 0.0f, false, 70);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullBedSheet", "60_110", 0.0f, false, 80);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("500_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "60_20", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "60_30", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "60_50", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "TurnBack", "60_65", 0.0f, false, 60);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullBedSheet", "60_110", 0.0f, false, 80);
 
        // Animate Male
        AnimateMale.Instance.SetIdle("500_10");
        AnimateMale.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "60_20", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "60_30", 0.0f, false, 20);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "60_50", 0.0f, false, 40);
        AnimateMale.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#0" }, "PlaceSlideMat2", "500_60", 0.0f, false, 50);
        AnimateMale.Instance.AddAnimation(new string[] { "HUD_Bedsheet2(Clone)#1" }, "PlaceBedSheet", "60_70", 0.0f, false, 70);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullBedSheet", "60_110", 0.0f, false, 80);

		// Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);

        // Animate Drawsheet
        AnimateDrawSheet2.Instance.SetIdle("60_10");
        AnimateDrawSheet2.Instance.AddAnimation(new string[] { "HUD_Bedsheet2(Clone)#1" }, "PlaceBedSheet", "60_70", 0.0f, false, 70);
        AnimateDrawSheet2.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullBedSheet", "60_110", 0.0f, false, 80);
        AnimateDrawSheet2.Instance.AddResetState(new string[] { "HUD_Bedsheet2(Clone)#1" });

        // Animate Slidemat
        AnimateSlideMat2.Instance.SetIdle("500_10");
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PlaceSlideMat1", "500_40", 0.0f, false, 30, -1.0f, 0.0f);
        AnimateSlideMat2.Instance.AddResetState(new string[] { "HUD_SlideMat2(Clone)#1" });

        AnimateSlideMat21.Instance.SetIdle("500_10b");
        AnimateSlideMat21.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#0" }, "PlaceSlideMat2", "500_60b", 0.0f, false, 50,  -1.0f, 0.0f);
        AnimateSlideMat21.Instance.AddResetState(new string[] { "HUD_SlideMat2(Clone)#0" });

        // Animate Pillow
        AnimatePillow.Instance.SetIdle("500_10");
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_5" }, "PullBedSheet", "60_110", 4.32f, false, 10);
		AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullBedSheet", "500_100", 4.1f, false, 20);

        // AnimateAntiSlidemat
        AnimateAntiSlideMat.Instance.SetIdle("60_10");
        AnimateAntiSlideMat.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "60_20", 0.0f, false, 10);
        AnimateAntiSlideMat.Instance.AddResetState(new string[] { "HUD_AntiSlideMat2(Clone)#2" });
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);

        if (t != _currentState && !States.Instance.GetExersiciseValue(t) && !States.Instance.HasFinished())
        {
            _currentState = t;
            int rv = States.Instance.UpdateState(t, help);
            Debug.Log("Rv: " + rv.ToString());
            if (rv != -1)
            {
                if (!States.Instance.GetExerciseCritical(rv))
                {
                    States.Instance.PushState("showingErrorMessage");
                    Util.OkMessageBox(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), "\n\n" + States.Instance.GetExerciseError(), OkClicked);
                    Results.Instance.SubtractStar();
                    StarFade.Instance.ShowStar(new Rect((Screen.width / 2 - 138), Screen.height / 2 - 90, 138, 90), false);
                }
                else
                {
                    Results.Instance.ShowResults(true, Text.Instance.GetString("results_failed") + "\n\n" + States.Instance.GetExerciseError(), 0.0f);
                    if (States.Instance.GetExerciseVideo() != -1) Video.Instance.PlayMovie(States.Instance.GetExerciseVideo());
                }
            }
            else
            {
                if (help)
                {
                    Help.Instance.UpdateHelp(t);
                }
                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
                AnimateBed3.Instance.UpdateAnimation(t);
                AnimateMale.Instance.UpdateAnimation(t);
                AnimateSlideMat2.Instance.UpdateAnimation(t);
                AnimateSlideMat21.Instance.UpdateAnimation(t);
                AnimateDrawSheet2.Instance.UpdateAnimation(t);
                AnimatePillow.Instance.UpdateAnimation(t);
                AnimateAntiSlideMat.Instance.UpdateAnimation(t);

                if (States.Instance.HasFinished())
                {
                    string s = help ? Text.Instance.GetString("results_passed_help") : Text.Instance.GetString("results_passed_test");

                    string rms = States.Instance.GetComments();
                    s += rms.Length > 1 ? "\n\n" + Text.Instance.GetString("results_comment") + " " + rms : "\n";

                    Results.Instance.ShowResults(false, help, s, States.Instance.GetExerciseDelay(States.Instance.CurrentState()));
                }
            }
        }
    }

    public void OkClicked(Message message, bool value)
    {
        States.Instance.PushState("showingErrorMessage", "no");
        StarFade.Instance.HideStar();
    }

    // Temp test of states
    public string _currentState = "";
    public bool help = false;

    // Use this for initialization
    void Start()
    {
        // Clear old states
        States.Instance.ClearStates();

        // Set callback name to this gameobject
        States.Instance.PushState("actionCallbackGameObjectName", gameObject.name);

        // If run in editor or not
        if (SceneLoader.Instance.CurrentScene != -1)
        {
            help = Global.Instance.RunSimulationWithHelp;
        }
        else
        {
            States.Instance.PushState("DEBUG");
            GameObject.Instantiate((GameObject)Resources.Load("BottomBar"));
            GameObject.Instantiate((GameObject)Resources.Load("TopBar"));
        }

        // Initialize and define simulation
        initializeExercise();
        defineExercise();

        if (help)
        {
            Help.Instance.ShowHelpText();
        }

        // trick for not displaying the toolbox
        States.Instance.PushState("ToolboxDone", "yes");

        // Start the simulation
        SimCallback("start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
    }
}