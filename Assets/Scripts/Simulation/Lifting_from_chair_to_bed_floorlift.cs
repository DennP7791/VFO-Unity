using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lifting_from_chair_to_bed_floorlift : MonoBehaviour 
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
            tg.SetToolCorrectness("Liftkontrol", true);
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
        States.Instance.InitExerciseState(new string[] { "BedSittingUp" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_chair_bed_floor_state_0"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "BedHeightDown" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_chair_bed_floor_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedCompleted" }, States.CheckCon.NotCritical, "Du skal huske at både placere sengen i en siddende position samt sænke den", 0.0f);
        States.Instance.InitExerciseState(new string[] { "clickedSling1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_30"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "FloorLiftUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_3"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "FloorLiftIn" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_4"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_5"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "BedHeightUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_6"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "FloorLiftDown" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_chair_bed_floor_state_7"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "clickedSling2" }, States.CheckCon.NotCritical, "", 10.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_chair_bed_floor_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_chair_bed_floor_help_1");
        Help.Instance.AddHelpText(new string[] { "BedSittingUp" }, "sim_chair_bed_floor_help_2");
        Help.Instance.AddHelpText(new string[] { "BedHeightDown" }, "sim_chair_bed_floor_help_3");
        Help.Instance.AddHelpText(new string[] { "clickedSling1" }, "sim_chair_bed_floor_help_40");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_chair_bed_floor_help_4");
        Help.Instance.AddHelpText(new string[] { "FloorLiftUp" }, "sim_chair_bed_floor_help_5");
        Help.Instance.AddHelpText(new string[] { "FloorLiftIn" }, "sim_chair_bed_floor_help_6");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_chair_bed_floor_help_7");
        Help.Instance.AddHelpText(new string[] { "BedHeightUp" }, "sim_chair_bed_floor_help_8");
        Help.Instance.AddHelpText(new string[] { "FloorLiftDown" }, "sim_chair_bed_floor_help_9");
              
        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_chair_bed_floor_talk_00");
        Talk.Instance.AddTalkDialog("Talk_0_0", pos1, "sim_chair_bed_floor_talk_0");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("530_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "clickedSling1" }, "AttachStraps", "530_40", 1.24f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "FloorLiftUp" }, "FloorLiftUp", "530_50", 1.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveLift", "530_70", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "clickedSling2" }, "DetachStraps", "530_100", 1.0f, false, 40, 1.0f, 0.0f);
		

        // Animate Male Helper
        AnimateMale.Instance.SetIdle("530_10");
        AnimateMale.Instance.AddAnimation(new string[] { "clickedSling1" }, "AttachStraps", "530_40", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "FloorLiftUp" }, "FloorLiftUp", "530_50", 0.0f, false, 20);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveLift", "530_70", 0.0f, false, 30);
        AnimateMale.Instance.AddAnimation(new string[] { "clickedSling2" }, "DetachStraps", "530_100", 1.0f, false, 40, 1.0f, 0.0f);
		
		
		// Animations Borger
        AnimateBorger.Instance.SetIdle("530_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "FloorLiftUp" }, "FloorLiftUp", "530_50", 1.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveLift", "530_70", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedHeightUp" }, "BedUp", "530_80", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "FloorLiftDown" }, "FloorLiftDown", "530_90", 0.0f, false, 40, -1.0f, 2.0f);
		

        // AnimateSling
        AnimateSling2.Instance.SetIdle("530_10");
        AnimateSling2.Instance.AddAnimation(new string[] { "clickedSling1" }, "AttachStraps", "530_40", 1.24f, false, 10);
        AnimateSling2.Instance.AddAnimation(new string[] { "FloorLiftUp" }, "FloorLiftUp", "530_50", 1.0f, false, 20);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveLift", "530_70", 0.0f, false, 30);
        AnimateSling2.Instance.AddAnimation(new string[] { "FloorLiftDown" }, "FloorLiftDown", "530_90", 0.0f, false, 40);
        AnimateSling2.Instance.AddAnimation(new string[] { "clickedSling2" }, "DetachStraps", "530_100", 1.0f, false, 50, 1.0f, 0.0f);


		// Animate Floor Lift
        AnimateFloorLift.Instance.SetIdle("530_10");
        AnimateFloorLift.Instance.AddAnimation(new string[] { "start" }, "Feet", "feet", 0.0f, true, 20, 1.0f, 0.0f);
		AnimateFloorLift.Instance.AddAnimation(new string[] { "FloorLiftUp" }, "FloorLiftUp", "530_50", 1.0f, false, 10);
		AnimateFloorLift.Instance.AddAnimation(new string[] { "FloorLiftIn" }, "FloorLiftIn2", "feet", 0.0f, true, 20, 0.0f, 3.0f);
		AnimateFloorLift.Instance.AddAnimation(new string[] { "Talk_0_1" }, "MoveLift", "530_70", 0.0f, false, 30);
		AnimateFloorLift.Instance.AddAnimation(new string[] { "FloorLiftDown" }, "FloorLiftDown", "530_90", 0.0f, false, 40);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedSittingUp" }, "530_20", 1.0f, 5.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightDown" }, "530_30", 1.0f, 5.0f);
		AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightUp" }, "530_30", 0.0f, 5.0f);


        // Animate Wheelchair
        AnimateWheelChair3.Instance.SetIdle("530_10");
		Transparency2.Instance.SetAlpha(1.0f);
		
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "start" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "Talk_0_1" }, 1, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "clickedSling2" }, 2, 0.0f, 1.0f);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);

        if (!help && (t == "BedHeightDown" || t == "BedSittingUp"))
        {
            if (States.Instance.GetExersiciseValue("clickedSling1"))
            {
                t = "NoState";
            }
        }
		
		if (t == "clickedSling")
        {
        	if (!States.Instance.GetExersiciseValue("clickedSling1"))
           		t = "clickedSling1";
        	else
           		t = "clickedSling2";
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
				
				
				if (t == "FloorLiftUp")
                {
                    Transparency2.Instance.StartFade();
                }
				
				if (t == "clickedSling1" || t == "clickedSling2")
                {
                    GameObject go = GameObject.Find("sail");
                    if (go)
                        go.layer = LayerMask.NameToLayer("Default");
                }
                else if (t == "FloorLiftDown")
                {
                    GameObject go = GameObject.Find("sail");
                    if (go)
                        go.layer = LayerMask.NameToLayer("mouseOver");
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
                AnimateBed3.Instance.UpdateAnimation(t);
                AnimateMale.Instance.UpdateAnimation(t);
                AnimateSling2.Instance.UpdateAnimation(t);
                AnimateFloorLift.Instance.UpdateAnimation(t);
				MoveAndFadeCamera.Instance.UpdateCameraFade(t);
				
				GameObject go1 = GameObject.Find("Wheelchair");
				if(go1)
                	AnimateWheelChair3.Instance.UpdateAnimation(t);

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
        if (States.Instance.GetExersiciseValue("BedHeightDown") && States.Instance.GetExersiciseValue("BedSittingUp") && !States.Instance.GetExersiciseValue("BedCompleted"))
            SimCallback("BedCompleted");

        if(Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
	}
}