using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Higher_up_borger_a : MonoBehaviour 
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
            tg.SetToolCorrectness("Sengekontrol", true);
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
        States.Instance.InitExerciseState(new string[] { "BedHeadDown" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_a_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_a_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_high_up_a_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, "Du skal huske at bede borgeren om at flytte sig højere op i sengen.", 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndDown" }, States.CheckCon.NotCritical, "", 5.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_high_up_a_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_high_up_a_help_1");
        Help.Instance.AddHelpText(new string[] { "BedHeadDown" }, "sim_high_up_a_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_high_up_a_help_3");
        Help.Instance.AddHelpText(new string[] { "BedEndUp" }, "sim_high_up_a_help_4");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_high_up_a_help_5");


        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_high_up_a_talk_0");
        Talk.Instance.AddTalkDialog(pos1, "sim_high_up_a_talk_1");
		
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("idle_100f");
        
        // Animations Borger
        AnimateBorger.Instance.SetIdle("Idle_back__low_in_bed_100f");
        AnimateBorger.Instance.AddAnimation(new string[] { "start" }, "MoveHeadUp", "Bed_lift_torso_2f", 0.0f, true, 10, 0.5f, 0.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedHeadDown" }, "MoveHeadDown", "Bed_lift_torso_2f", 0.0f, true, 10, 0.0f, 4.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "BendLegs", "on_back_lifting_legs_91f", 0.0f, false, 20, -1.0f, 2.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "BedEndUp" }, "MoveFeetUp", "Bed_lift_legs_2f", 0.0f, true, 20, 0.3f, 4.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveBack", "back_crawl_238f", 0.0f, false, 30, -1.0f, 2.0f);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedEndDown" }, "MoveFeedDown", "Bed_lift_legs_2f", 0.0f, true, 20, 0.1f, 4.0f);
		
		// Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "head_end_pos_high_35_degree", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeadDown" }, "head_end_pos_high_35_degree", 0.0f, 4.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndUp" }, "foot_end_pos_high", 0.3f, 4.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndDown" }, "foot_end_pos_high", 0.0f, 4.0f);
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
                   /* int pos = Help.Instance.GetSpeak(t);
                    if (pos != -1)
                        playHelpClip.PlayClip(pos);*/ 
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
				AnimateBed3.Instance.UpdateAnimation(t);
                
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

   // public List<string> _helpSpeak = new List<string>();
    // PlayHelpClip playHelpClip;

	// Use this for initialization
	void Start () 
	{
       /* if(!GetComponent<PlayHelpClip>())
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