using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Place_sling_in_bed : MonoBehaviour 
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
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_bend"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_turn"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "slingPlaced" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_sling"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_back"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_pull"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_bed_state_strops"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_5" }, States.CheckCon.NotCritical, "", 4.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_sling_bed_help_tool");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_sling_bed_help_bend");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_sling_bed_help_turn");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_sling_bed_help_sling");
        Help.Instance.AddHelpText(new string[] { "slingPlaced" }, "sim_sling_bed_help_back");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_sling_bed_help_pull");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_sling_bed_help_strops");
        Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_sling_bed_help_cross");    

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_sling_bed_talk_bend");
        Talk.Instance.AddTalkDialog(pos1, "sim_sling_bed_talk_turn");
        Talk.Instance.AddTalkDialog("Talk_0_1", pos1, "sim_sling_bed_talk_back");
        Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_sling_bed_talk_pull");
        Talk.Instance.AddTalkDialog("slingPlaced", pos1, "sim_sling_bed_talk_strops");
        Talk.Instance.AddTalkDialog("slingPlaced", pos1, "sim_sling_bed_talk_cross");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("505_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "505_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "505_30", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "slingPlaced" }, "PlaceSling", "505_40", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "505_50", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "TurnBack", "505_60", 0.0f, false, 50);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullStraps", "505_70", 2.4f, false, 60);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_5" }, "CrossStraps", "505_80", 0.0f, false, 70);

        // Animate Male Helper
        AnimateMale.Instance.SetIdle("505_10");
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "505_20", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "505_30", 0.0f, false, 20);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "505_50", 0.0f, false, 40);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullStraps", "505_70", 0.0f, false, 60);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_5" }, "CrossStraps", "505_80", 0.0f, false, 70);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("505_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "505_20", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TurnRight", "505_30", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "505_50", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "TurnBack", "505_60", 0.0f, false, 50);

        // AnimateSling
        AnimateSling2.Instance.AddAnimation(new string[] { "slingPlaced" }, "PlaceSling", "505_40", 0.0f, false, 30);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_2" }, "TurnLeft", "505_50", 0.0f, false, 40);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_4" }, "PullStraps", "505_70", 2.4f, false, 60);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_5" }, "CrossStraps", "505_80", 0.0f, false, 70, -1.0f, 0.5f);
        AnimateSling2.Instance.AddResetState(new string[] { "slingPlaced" });

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
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
                AnimateSling2.Instance.UpdateAnimation(t);

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