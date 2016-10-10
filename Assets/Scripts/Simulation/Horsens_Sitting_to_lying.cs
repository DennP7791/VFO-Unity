using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Horsens_Sitting_to_lying : MonoBehaviour 
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
            tg.SetToolCorrectness("Sengekontrol", true);
            tg.SetToolCorrectness("Skammel", true);
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
        States.Instance.InitExerciseState(new string[] { "chairPlaced" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sitting_to_lying_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedHeightUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sitting_to_lying_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sitting_to_lying_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedHeightDown" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_hor_sitting_to_lying_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndDown" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_hor_sitting_to_lying_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedCompleted" }, States.CheckCon.NotCritical, "Du skal huske at både sænke sengen og køre fodenden ned", 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sitting_to_lying_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sitting_to_lying_state_6"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, "", 6.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_hor_sitting_to_lying_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_hor_sitting_to_lying_help_1");
        Help.Instance.AddHelpText(new string[] { "chairPlaced" }, "sim_hor_sitting_to_lying_help_2");
        Help.Instance.AddHelpText(new string[] { "BedHeightUp" }, "sim_hor_sitting_to_lying_help_3");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_hor_sitting_to_lying_help_4");
        Help.Instance.AddHelpText(new string[] { "BedHeightDown" }, "sim_hor_sitting_to_lying_help_5");
        Help.Instance.AddHelpText(new string[] { "BedEndDown" }, "sim_hor_sitting_to_lying_help_6");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_hor_sitting_to_lying_help_7");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_hor_sitting_to_lying_help_8");
        
        // Dialog
        int pos = Talk.Instance.AddTalkDialog("hoover_head", "sim_hor_sitting_to_lying_talk_0");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_sitting_to_lying_talk_1");
        Talk.Instance.AddTalkDialog("Talk_0_1", pos, "sim_hor_sitting_to_lying_talk_2");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_sitting_to_lying_talk_3");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("2010_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "BedHeightUp" }, "ElevateBed", "2010_30", 0.0f, false, 10, 1.0f, 0.0f);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PutFeet", "2010_40", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "BedHeightDown" }, "LowerBed", "2010_50", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LiftLeg", "2010_70", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "LiftLeg2", "2010_80", 0.0f, false, 50);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "LiftButt", "2010_90", 0.0f, false, 60);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("2010_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "BedHeightUp" }, "ElevateBed", "2010_30", 0.0f, false, 10, 1.0f, 0.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PutFeet", "2010_40", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedHeightDown" }, "LowerBed", "2010_50", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LiftLeg", "2010_70", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "LiftLeg2", "2010_80", 0.0f, false, 50);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "LiftButt", "2010_90", 0.0f, false, 60);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("2010_10");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "head_end_pos_high_35_degree", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightUp" }, "2010_30", 1.0f, 6.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightDown" }, "2010_30", 0.0f, 5.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndDown" }, "2010_60", 1.0f, 3.0f);

        //Animate Chair
        AnimateChair1.Instance.AddAnimation(new string[] { "chairPlaced" }, "PlaceChair", "2010_10", 0.0f, false, 10);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);

        if (!help && (t == "BedHeightDown" || t == "BedEndDown"))
        {
            if (!States.Instance.GetExersiciseValue("Talk_0_0") && !States.Instance.GetExersiciseValue("Talk_0_1"))
            {
                t = "NoState";
            }
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
                AnimateChair1.Instance.UpdateAnimation(t);

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
        if (States.Instance.GetExersiciseValue("BedHeightDown") && States.Instance.GetExersiciseValue("BedEndDown") && !States.Instance.GetExersiciseValue("BedCompleted"))
            SimCallback("BedCompleted");

        if(Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
	}
}