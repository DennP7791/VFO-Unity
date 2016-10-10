using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stand_up_borger_b : MonoBehaviour 
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
            tg.SetToolCorrectness("Rollator", true);
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
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_forward"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_Walker2(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_walker"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_feet"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_hand"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_lean"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_stand_b_state_stand"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedHeightUp" }, States.CheckCon.NotCritical, "", 5.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_stand_b_help_toolbox");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_stand_b_help_forward");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_stand_b_help_walker");
        Help.Instance.AddHelpText(new string[] { "HUD_Walker2(Clone)#1" }, "sim_stand_b_help_feet");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_stand_b_help_hand");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_stand_b_help_lean");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_stand_b_help_prepare");
        Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_stand_b_help_stand");

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_stand_b_talk_forward");
        Talk.Instance.AddTalkDialog(pos1, "sim_stand_b_talk_feet");
        Talk.Instance.AddTalkDialog(pos1, "sim_stand_b_talk_hand");
        Talk.Instance.AddTalkDialog(pos1, "sim_stand_b_talk_lean");
        Talk.Instance.AddTalkDialog(pos1, "sim_stand_b_talk_stand");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("idle_100f");

        // Animations Borger
        AnimateBorger.Instance.SetIdle("Idle_Sitting_100f");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "1", "B_bed_Sitting_Move_forward_131f", 0.0f, false, 10, -1.0f, 2.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "2", "B_bed_Sitting_place_legs_76f", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "3", "B_bed_Sitting_Standup_01_131f", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "4", "B_bed_Sitting_Standup_02_42f", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedHeightUp" }, "5", "B_bed_Sitting_Standup_03_107f", 0.0f, false, 50, -1.0f, 0.5f);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base_middle");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "middle_to_low", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightUp" }, "middle_to_low", 0.0f, 4.0f);

        // Animate Walker
        AnimateWalker2.Instance.AddResetState(new string[] { "HUD_Walker2(Clone)#1" });
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
                AnimateWalker2.Instance.UpdateAnimation(t);

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