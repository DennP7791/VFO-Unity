using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sit_up_borger_c : MonoBehaviour 
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
            tg.SetToolCorrectness("Kollega", true);
            tg.SetToolCorrectness("Kontrol", true);
        }
        Util.ToggleResource<ToolGrid>("ToolGrid");

        HUD hud = Util.ToggleResource<HUD>("HUD_Helper");
        if (hud)
        {
            hud.Buttons[(int)Helper.Position.CENTERLEFT].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_Helper");

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
        States.Instance.InitExerciseState(new string[] { "HUD_Helper(Clone)#6" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sit_c_state_helper"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "bendLegs" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sit_c_state_bendlegs"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sit_c_state_movearms"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_1_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sit_c_state_ready"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "sitUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sit_c_state_situp"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedHeightDown" }, States.CheckCon.NotCritical, "", 5.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_sit_c_help_toolbox");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_sit_c_help_helper");
        Help.Instance.AddHelpText(new string[] { "HUD_Helper(Clone)#6" }, "sim_sit_c_help_bendlegs");
        Help.Instance.AddHelpText(new string[] { "bendLegs" }, "sim_sit_c_help_placearms");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_sit_c_help_ready");
        Help.Instance.AddHelpText(new string[] { "Talk_1_0" }, "sim_sit_c_help_situp");
        Help.Instance.AddHelpText(new string[] { "sitUp" }, "sim_sit_c_help_beddown");

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_sit_c_talk_arms");
        int pos2 = Talk.Instance.AddTalkDialog("clickedHelper", "sim_sit_c_talk_ready");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("idle_100f");
        AnimateFemale.Instance.AddAnimation(new string[] { "bendLegs" }, "BendLegs", "_c_help_bend_legs_211f", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "sitUp" }, "SitUp", "C_Lying_to_sitting_298f", 0.0f, false, 20);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("Idle_Side_100f");
        AnimateBorger.Instance.AddAnimation(new string[] { "bendLegs" }, "BendLegs", "Side_bend_legs_55f", 3.125f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "PlaceArms", "Side_Place_arms_51f", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "sitUp" }, "SitUp", "C_bed_Lying_to_Sitting_298f", 0.0f, false, 30);

        // Animations Male Helper
        AnimateMale.Instance.SetIdle("60_20");
        AnimateMale.Instance.AddAnimation(new string[] { "sitUp" }, "SitUp", "C_bed_Lying_to_sitting_298F", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "BedHeightDown" }, "BedHeightDown", "60_60", 0.0f, false, 20);
        AnimateMale.Instance.AddResetState(new string[] { "HUD_Helper(Clone)#6" });


        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightDown" }, "pos_low", 1.0f, 4.0f);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);

        if (t == "hoover_left_knee" || t == "hoover_right_knee")
        {
            if (!States.Instance.GetExersiciseValue("bendLegs"))
                t = "bendLegs";
            else
                t = "sitUp";
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
                    StarFade.Instance.ShowStar(new Rect((Screen.width / 2 - 138), Screen.height / 2 - 90, 138, 90), false);
                    Results.Instance.SubtractStar();
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

                if (t == "boxPlaced")
                {
                    GameObject.Find("Box").GetComponent<Renderer>().enabled = true;
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateMale.Instance.UpdateAnimation(t);
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

    //public List<AudioClip> helpSpeak = new List<AudioClip>();
    //PlayHelpClip playHelpClip;

	// Use this for initialization
	void Start () 
	{
        /*if(!GetComponent<PlayHelpClip>())
            gameObject.AddComponent("PlayHelpClip");
        playHelpClip = GetComponent<PlayHelpClip>();
        playHelpClip.AddHelpClips(helpSpeak);*/ 

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