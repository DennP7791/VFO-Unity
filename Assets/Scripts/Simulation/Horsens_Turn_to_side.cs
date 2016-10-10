using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Horsens_Turn_to_side : MonoBehaviour 
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
            tg.SetToolCorrectness("Kollega", true);
        }
        Util.ToggleResource<ToolGrid>("ToolGrid");

        GameObject go = GameObject.Find("Bed");
        if (go != null)
        {
            Util.ToggleSubElementRenderer(go, "bottom_left_slide");
            Util.ToggleSubElementRenderer(go, "bottom_right_slide");
            Util.ToggleSubElementRenderer(go, "up_left_slide");
            Util.ToggleSubElementRenderer(go, "up_right_slide");
            Util.ToggleSubElementRenderer(go, "antislide");
        }

        HUD hud = Util.ToggleResource<HUD>("HUD_SlideMat2");
        if (hud)
        {
            hud.Buttons[(int)SlideMat2.Position.TOPRIGHT].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_SlideMat2");

        HUD hud3 = Util.ToggleResource<HUD>("HUD_Helper");
        if (hud3)
        {
            hud3.Buttons[(int)Helper.Position.CENTERLEFT].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_Helper");

        MouseOverObject.Instance.HoverSlidemat = true;
	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "HUD_Helper(Clone)#6" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMat2(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_6"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_5" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_7"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_6" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_8"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "clickedSlidemat" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_turn_to_side_state_9"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_7" }, States.CheckCon.NotCritical, "", 8.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_hor_turn_help_toolbox");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_hor_turn_help_placehelper");
        Help.Instance.AddHelpText(new string[] { "HUD_Helper(Clone)#6" }, "sim_hor_turn_help_bendLegs");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_hor_turn_help_turntoright");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_hor_turn_help_slidemat");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMat2(Clone)#1" }, "sim_hor_turn_help_onback");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_hor_turn_help_movebutt");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_hor_turn_help_moveupper");
        Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_hor_turn_help_liftlegs");
        Help.Instance.AddHelpText(new string[] { "Talk_0_5" }, "sim_hor_turn_help_turntorightagain");
        Help.Instance.AddHelpText(new string[] { "Talk_0_6" }, "sim_hor_turn_help_slidematcorrect");
        Help.Instance.AddHelpText(new string[] { "clickedSlidemat" }, "sim_hor_turn_help_pushbutt"); 
        
        // Dialog
        int pos = Talk.Instance.AddTalkDialog("hoover_head", "sim_hor_turn_to_side_talk_0");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_turn_to_side_talk_1");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#1", pos, "sim_hor_turn_to_side_talk_2");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#1", pos, "sim_hor_turn_to_side_talk_3");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#1", pos, "sim_hor_turn_to_side_talk_4");
        Talk.Instance.AddTalkDialog("HUD_SlideMat2(Clone)#1", pos, "sim_hor_turn_to_side_talk_5");
        Talk.Instance.AddTalkDialog("Talk_0_5", pos, "sim_hor_turn_to_side_talk_6");
        Talk.Instance.AddTalkDialog("Talk_0_5", pos, "sim_hor_turn_to_side_talk_7");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("2000_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "2000_30", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnToSide", "2000_40", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "SlideMatPlace", "2000_50", 0.0f, false, 30, 1.0f, 0.0f);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnToBack", "2000_60", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "LiftButt", "2000_70", 0.0f, false, 50);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "LiftUpper", "2000_80", 0.0f, false, 60);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftLegs", "2000_90", 0.0f, false, 70);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnToSide2", "2000_100", 0.0f, false, 80);
        AnimateFemale.Instance.AddAnimation(new string[] { "clickedSlidemat" }, "ClickSlideMat", "2000_110", 0.0f, false, 90);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_7" }, "PushButt", "2000_120", 0.0f, false, 100);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("2000_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "2000_30", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnToSide", "2000_40", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnToBack", "2000_60", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "LiftButt", "2000_70", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_4" }, "LiftUpper", "2000_80", 0.0f, false, 50);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftLegs", "2000_90", 0.0f, false, 60);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnToSide2", "2000_100", 0.0f, false, 70);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_7" }, "PushButt", "2000_120", 0.0f, false, 80);

        // Animate Male Helper
        AnimateMale.Instance.SetIdle("2000_10");
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "2000_30", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnToSide", "2000_40", 0.0f, false, 20);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnToBack", "2000_60", 0.0f, false, 30);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "LiftButt", "2000_70", 0.0f, false, 50);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "LiftUpper", "2000_80", 0.0f, false, 60);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnToSide2", "2000_100", 0.0f, false, 80);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_7" }, "PushButt", "2000_120", 0.0f, false, 100);
        AnimateMale.Instance.AddResetState(new string[] { "HUD_Helper(Clone)#6" });

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);

        // Animate Slidemat
        AnimateSlideMat2.Instance.SetIdle("2000_10");
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#1" }, "PlaceSlidemat", "2000_50", 0.0f, false, 10, 1.0f, 0.0f);
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "clickedSlidemat" }, "ClickSlideMat", "2000_110", 0.0f, false, 20);
        AnimateSlideMat2.Instance.AddResetState(new string[] { "HUD_SlideMat2(Clone)#1" });

        AnimatePillow.Instance.SetIdle("2000_10");
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnToSide", "2000_40", 0.0f, false, 20);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnToBack", "2000_60", 0.0f, false, 30);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_4" }, "LiftUpper", "2000_80", 2.16f, false, 60);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnToSide2", "2000_100", 0.0f, false, 80);
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
                    Results.Instance.ShowResults(true, States.Instance.GetExerciseError(), 0.0f);
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
                AnimateMale.Instance.UpdateAnimation(t);
                AnimateSlideMat2.Instance.UpdateAnimation(t);
                AnimatePillow.Instance.UpdateAnimation(t);

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