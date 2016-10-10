using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Place_sling_in_bed_single : MonoBehaviour 
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
            tg.SetToolCorrectness("Sejl", true);
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
	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_0"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_1"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "slingPlaced" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_2"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_3"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_4"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "sengehest1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_5"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "hoover_sengehest" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_6"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_7"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_5" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_8"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_6" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_9"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_7" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_10"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_8" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_11"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_9" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_12"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "sengehest2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_place_sling_in_bed_single_state_13"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_10" }, States.CheckCon.NotCritical, "", 7.0f);
	
		
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_place_sling_in_bed_single_help_0");
		Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_place_sling_in_bed_single_help_1");
		Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_place_sling_in_bed_single_help_2");
		Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_sling_bed_help_sling");
		Help.Instance.AddHelpText(new string[] { "slingPlaced" }, "sim_place_sling_in_bed_single_help_3");
		Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_place_sling_in_bed_single_help_4");
		Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_place_sling_in_bed_single_help_5");
		Help.Instance.AddHelpText(new string[] { "sengehest1" }, "sim_place_sling_in_bed_single_help_6");
		Help.Instance.AddHelpText(new string[] { "hoover_sengehest" }, "sim_place_sling_in_bed_single_help_7");
		Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_place_sling_in_bed_single_help_8");
		Help.Instance.AddHelpText(new string[] { "Talk_0_5" }, "sim_place_sling_in_bed_single_help_9");
		Help.Instance.AddHelpText(new string[] { "Talk_0_6" }, "sim_place_sling_in_bed_single_help_10");
		Help.Instance.AddHelpText(new string[] { "Talk_0_7" }, "sim_place_sling_in_bed_single_help_11");
		Help.Instance.AddHelpText(new string[] { "Talk_0_8" }, "sim_place_sling_in_bed_single_help_12");
		Help.Instance.AddHelpText(new string[] { "Talk_0_9" }, "sim_place_sling_in_bed_single_help_13");
		Help.Instance.AddHelpText(new string[] { "sengehest2" }, "sim_place_sling_in_bed_single_help_14");
		
       
        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_place_sling_in_bed_single_talk_0");
        Talk.Instance.AddTalkDialog(pos1, "sim_place_sling_in_bed_single_talk_1");
		Talk.Instance.AddTalkDialog(pos1, "sim_place_sling_in_bed_single_talk_2");
		Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_place_sling_in_bed_single_talk_3");
		Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_place_sling_in_bed_single_talk_4");
		Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_place_sling_in_bed_single_talk_5");
		Talk.Instance.AddTalkDialog("Talk_0_5", pos1, "sim_place_sling_in_bed_single_talk_6");
		Talk.Instance.AddTalkDialog("Talk_0_5", pos1, "sim_place_sling_in_bed_single_talk_7");
		Talk.Instance.AddTalkDialog("Talk_0_5", pos1, "sim_place_sling_in_bed_single_talk_8");
		Talk.Instance.AddTalkDialog("Talk_0_5", pos1, "sim_place_sling_in_bed_single_talk_9");
		Talk.Instance.AddTalkDialog("Talk_0_5", pos1, "sim_place_sling_in_bed_single_talk_10");
		
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("1130_10");
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLLeg", "1130_20", 0.0f, false, 10, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "1130_30", 0.0f, false, 20, -1.0f, 0.5f);
		AnimateFemale.Instance.AddAnimation(new string[] { "slingPlaced" }, "PlaceSling", "505_40", 0.0f, false, 30);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnBack", "1130_50", 0.0f, false, 40);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "StropUnderThigh", "1130_60", 0.0f, false, 50);
		AnimateFemale.Instance.AddAnimation(new string[] { "hoover_sengehest" }, "Sengehest", "1130_80", 1.0f, false, 60, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "StropUnderLeg", "1130_90", 0.0f, false, 70);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_5" }, "BendRLeg", "1130_100", 0.0f, false, 80);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnLeft", "1130_110", 0.0f, false, 90);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_7" }, "FlattenSling", "1130_120", 0.0f, false, 100);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_8" }, "TurnBack2", "1130_130", 0.0f, false, 110);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_9" }, "StropUnderThigh2", "1130_140", 0.0f, false, 120);
		AnimateFemale.Instance.AddAnimation(new string[] { "sengehest2" }, "", "1130_150", 1.0f, false, 130, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_10" }, "NotSureSick", "1130_160", 0.0f, false, 140);
 

        // Animations Borger
        AnimateBorger.Instance.SetIdle("1130_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLLeg", "1130_20", 1.67f, false, 10, 1.0f, 0.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "1130_30", 0.0f, false, 20);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnBack", "1130_50", 0.0f, false, 30);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_5" }, "BendRLeg", "1130_100", 0.0f, false, 40);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnLeft", "1130_110", 0.0f, false, 50);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_8" }, "TurnBack2", "1130_130", 0.0f, false, 60);
        

        // AnimateSling
		AnimateSling2.Instance.SetIdle("505_10");
        AnimateSling2.Instance.AddAnimation(new string[] { "slingPlaced" }, "PlaceSling", "505_40", 0.0f, false, 10);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_3" }, "StropUnderThigh", "1130_60", 0.0f, false, 20);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_4" }, "StropUnderLeg", "1130_90", 0.0f, false, 30);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_6" }, "TurnLeft", "1130_110", 0.0f, false, 40);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_7" }, "FlattenSling", "1130_120", 0.0f, false, 50);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_8" }, "TurnBack2", "1130_130", 0.0f, false, 60);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_9" }, "StropUnderThigh2", "1130_140", 0.0f, false, 70);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_10" }, "NotSureSick", "1130_160", 0.0f, false, 80);
		AnimateSling2.Instance.AddResetState(new string[] {"slingPlaced"});
        

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
		AnimateBed3.Instance.AddAnimation(new string[] { "sengehest1" }, "rail_down_left", 0.0f, 3.0f);
		AnimateBed3.Instance.AddAnimation(new string[] { "hoover_sengehest" }, "rail_down", 1.0f, 3.0f);
		AnimateBed3.Instance.AddAnimation(new string[] { "sengehest2" }, "rail_down_left", 1.0f, 3.0f);
		
		
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "hoover_sengehest" }, 1, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "sengehest2" }, 0, 0.0f, 1.0f);
		
        //AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);
		
		if (t == "hoover_sengehest001")
        {
        	if (!States.Instance.GetExersiciseValue("sengehest1"))
           		t = "sengehest1";
        	else
           		t = "sengehest2";
        }

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
                AnimateSling2.Instance.UpdateAnimation(t);
				MoveAndFadeCamera.Instance.UpdateCameraFade(t);

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