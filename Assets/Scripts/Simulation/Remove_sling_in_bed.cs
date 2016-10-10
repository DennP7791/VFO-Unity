using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Remove_sling_in_bed : MonoBehaviour 
{	
	private void initializeExercise()
	{

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
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_remove_sling_bed_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "hoover_sengehest" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_remove_sling_bed_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_remove_sling_bed_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, "", 12.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_remove_sling_bed_help_0");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_remove_sling_bed_help_1");
        Help.Instance.AddHelpText(new string[] { "hoover_sengehest" }, "sim_remove_sling_bed_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_remove_sling_bed_help_3");
        
   
        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_remove_sling_bed_talk_0");
        Talk.Instance.AddTalkDialog(pos1, "sim_remove_sling_bed_talk_1");
        Talk.Instance.AddTalkDialog(pos1, "sim_remove_sling_bed_talk_2");

        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("540_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PullStraps", "540_20", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "RemoveSling", "540_50", 0.0f, false, 60);

        // Animate Male Helper
        AnimateMale.Instance.SetIdle("540_10");
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PullStraps", "540_20", 0.0f, false, 30);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "RemoveSling", "540_50", 0.0f, false, 60);


        // Animations Borger
        AnimateBorger.Instance.SetIdle("540_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LeanForward", "540_40", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "RemoveSling", "540_50", 0.0f, false, 20);

        // AnimateSling
		AnimateSling2.Instance.SetIdle("540_10");
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PullStraps", "540_20", 0.0f, false, 30);
        AnimateSling2.Instance.AddAnimation(new string[] { "hoover_sengehest" }, "RailUp", "540_30", 0.0f, false, 40);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_2" }, "RemoveSling", "540_50", 0.0f, false, 60);
		

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
		AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "530_20", 0.9f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
		AnimateBed3.Instance.AddAnimation(new string[] { "hoover_sengehest" }, "rail_down_left", 0.0f, 3.2f);
		AnimateBed3.Instance.AddAnimation(new string[] { "hoover_sengehest" }, "rail_down", 0.0f, 3.2f);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);
		
		if(t == "hoover_sengehest001")
			t = "hoover_sengehest";

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