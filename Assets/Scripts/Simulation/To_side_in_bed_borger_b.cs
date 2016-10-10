using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class To_side_in_bed_borger_b : MonoBehaviour 
{	
	private void initializeExercise()
	{
        ToolBox tb = Util.ToggleResource<ToolBox>("ToolBox");
        if (tb)
        {
            tb.EmptyToolBox();
        }

        ToolGrid tg = Util.ToggleResource<ToolGrid>("ToolGrid");
        if (tg)
        {
            tg.SetToolCorrectness("Spielerdug", true);
            tg.SetToolCorrectness("Lagen", true);
            tg.SetToolCorrectness("Skridsikker", true);
        }
        Util.ToggleResource<ToolGrid>("ToolGrid");

        HUD hud = Util.ToggleResource<HUD>("HUD_SlideMat2");
        if (hud)
        {
            hud.Buttons[(int)SlideMat2.Position.TOPRIGHT].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_SlideMat2");

        HUDTiled hud2 = Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");
        if (hud2)
        {
            hud2.Buttons[(int)BedSheet.Position.ARMS].Correct = true;
        }
        Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");

        HUD shud = Util.ToggleResource<HUD>("HUD_AntiSlideMat2");
        if (shud)
        {
            shud.Buttons[(int)AntiSlideMat.Position.BOTTOM].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_AntiSlideMat2");

        GameObject go = GameObject.Find("Bed");
        if (go != null)
        {
            Util.ToggleSubElementRenderer(go, "bottom_left_slide");
            Util.ToggleSubElementRenderer(go, "bottom_right_slide");
            Util.ToggleSubElementRenderer(go, "up_left_slide");
            Util.ToggleSubElementRenderer(go, "up_right_slide");
            Util.ToggleSubElementRenderer(go, "antislide");
        }

        MouseOverObject.Instance.HoverDrawSheet = true;
	}

    private void defineExercise()
    {
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_0_extra"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMat2(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "clickedDrawsheet" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_side_b_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, "", 4.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_side_b_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_side_b_help_0_extra");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_side_b_help_1");
        Help.Instance.AddHelpText(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "sim_side_b_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_side_b_help_3");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMat2(Clone)#1" }, "sim_side_b_help_4");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_side_b_help_5");
        Help.Instance.AddHelpText(new string[] { "clickedDrawsheet" }, "sim_side_b_help_6");

        // Dialog
        int pos = Talk.Instance.AddTalkDialog("hoover_head", "sim_side_b_talk_0_extra");
        Talk.Instance.AddTalkDialog(pos, "sim_side_b_talk_0");
        Talk.Instance.AddTalkDialog(pos, "sim_side_b_talk_1");
        Talk.Instance.AddTalkDialog(pos, "sim_side_b_talk_2");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("20_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlacedAnitSlideMat", "20_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveHalf", "20_30", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PlaceSlideMat", "20_40", 0.3f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "clickedDrawsheet" }, "PullDrawsheet", "20_60", 0.0f, false, 40);
       
        // Animations Borger
        AnimateBorger.Instance.SetIdle("20_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlacedAnitSlideMat", "20_20", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveHalf", "20_30", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "MoveBack", "20_50", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "clickedDrawsheet" }, "PullDrawsheet", "20_60", 2.2f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "OntoBack", "20_70", 0.0f, false, 50);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        //AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);

        // Animate Slidemat
        AnimateSlideMat2.Instance.SetIdle("20_10");
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PlacedSlideMat", "20_40", 0.0f, false, 10, -1.0f, 0.5f);
        AnimateSlideMat2.Instance.AddResetState(new string[] { "HUD_SlideMat2(Clone)#1" });

        // Animate AntiSlideMat
        AnimateAntiSlideMat.Instance.SetIdle("20_10");
        AnimateAntiSlideMat.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "20_20", 0.0f, false, 10, -1.0f, 0.5f);
        AnimateAntiSlideMat.Instance.AddResetState(new string[] { "HUD_AntiSlideMat2(Clone)#2" });

        // Animate Drawsheet
        AnimateDrawSheet2.Instance.SetIdle("20_10");
        AnimateDrawSheet2.Instance.AddAnimation(new string[] { "Talk_0_1" }, "PlaceDrawSheet", "20_30", 0.0f, false, 10);
        AnimateDrawSheet2.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PullDrawSheet", "20_40", 10.8f, false, 20);
        AnimateDrawSheet2.Instance.AddAnimation(new string[] { "clickedDrawsheet" }, "PullDrawsheet", "20_60", 2.2f, false, 30);
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
                    /*int pos = Help.Instance.GetSpeak(t);
                    if (pos != -1)
                        playHelpClip.PlayClip(pos);*/ 
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
                AnimateBed3.Instance.UpdateAnimation(t);
                AnimateSlideMat2.Instance.UpdateAnimation(t);
                AnimateDrawSheet2.Instance.UpdateAnimation(t);
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

    //public List<string> _helpSpeak = new List<string>();
    //PlayHelpClip playHelpClip;

	// Use this for initialization
	void Start () 
	{
        /*if(!GetComponent<PlayHelpClip>())
            gameObject.AddComponent("PlayHelpClip");
        playHelpClip = GetComponent<PlayHelpClip>();
        playHelpClip.AddHelpClips(_helpSpeak);*/ 

        // Clear old states
		States.Instance.ClearStates();
		
		// Set callback name to this gameobject
		States.Instance.PushState("actionCallbackGameObjectName", gameObject.name);
		
        // If run in editor or not
		if(SceneLoader.Instance.CurrentScene != -1) {
			help = Global.Instance.RunSimulationWithHelp;
		}
        else {
            States.Instance.PushState("DEBUG");
            GameObject.Instantiate((GameObject)Resources.Load("BottomBar"));
            GameObject.Instantiate((GameObject)Resources.Load("TopBar"));
        }
		
        // Initialize and define simulation
		initializeExercise();
        defineExercise();
		
		if(help) {
			Help.Instance.ShowHelpText();
		}
			
		// trick for not displaying the toolbox
		States.Instance.PushState("ToolboxDone", "yes");

        // Start the simulation
        SimCallback("start");
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
	}
}