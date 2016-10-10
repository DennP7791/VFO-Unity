using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Horsens_Sit_up_on_bed : MonoBehaviour 
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
        States.Instance.InitExerciseState(new string[] { "BedSittingUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedSittingLegsDown" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_01"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndDown" }, States.CheckCon.OnlyCheckWithHelp, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_02"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedCompleted" }, States.CheckCon.NotCritical, "Du skal huske at sænke både knækket under benene og senges fodende", 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_sit_up_on_bed_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedHeightDown" }, States.CheckCon.NotCritical, "", 7.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_hor_sit_up_on_bed_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_hor_sit_up_on_bed_help_1");
        Help.Instance.AddHelpText(new string[] { "BedSittingUp" }, "sim_hor_sit_up_on_bed_help_11");
        Help.Instance.AddHelpText(new string[] { "BedSittingLegsDown" }, "sim_hor_sit_up_on_bed_help_12");
        Help.Instance.AddHelpText(new string[] { "BedEndDown" }, "sim_hor_sit_up_on_bed_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_hor_sit_up_on_bed_help_3");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_hor_sit_up_on_bed_help_4");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_hor_sit_up_on_bed_help_5");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_hor_sit_up_on_bed_help_6");
        Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_hor_sit_up_on_bed_help_7");
        
        // Dialog
        int pos = Talk.Instance.AddTalkDialog("hoover_head", "sim_hor_sit_up_on_bed_talk_0");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_sit_up_on_bed_talk_1");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_sit_up_on_bed_talk_2");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_sit_up_on_bed_talk_3");
        Talk.Instance.AddTalkDialog("Talk_0_3", pos, "sim_hor_sit_up_on_bed_talk_4");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("2050_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "LiftLeg", "2050_30", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LeanForward", "2050_40", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "MoveLeg1", "2050_50", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "MoveLeg2", "2050_60", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_4" }, "MoveLeg3", "2050_70", 0.0f, false, 50);
		AnimateFemale.Instance.AddAnimation(new string[] { "BedHeightDown" }, "BedDown", "2050_80", 0.0f, false, 60);
        
        // Animations Borger
        AnimateBorger.Instance.SetIdle("2050_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "BedSittingUp" }, "Bedsitting", "2050_20", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedSittingLegsDown" }, "BedSLDown", "2050_21", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedEndDown" }, "BLDown", "2050_22", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "LiftLeg", "2050_30", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LeanForward", "2050_40", 0.0f, false, 50);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "MoveLeg1", "2050_50", 0.0f, false, 60);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "MoveLeg2", "2050_60", 0.0f, false, 70);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_4" }, "MoveLeg3", "2050_70", 0.0f, false, 80);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedSittingUp" }, "2050_20", 1.0f, 2.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedSittingLegsDown" }, "2050_21", 1.0f, 2.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndDown" }, "2050_22", 1.0f, 2.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeightDown" }, "pos_low", 1.0f, 5.4f);

        // Animate Pillow
        AnimatePillow.Instance.SetIdle("2050_10");
        AnimatePillow.Instance.AddAnimation(new string[] { "BedSittingUp" }, "Bedsitting", "2050_20", 0.0f, false, 20, 0.0f);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LeanForward", "2050_40", 0.0f, false, 30);
        AnimatePillow.Instance.AddAnimation(new string[] { "BedHeightDown" }, "BedDown", "2050_80", 0.0f, false, 40);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        Debug.Log(t);

        if (!help && (t == "BedSittingUp" || t == "BedSittingLegsDown" || t == "BedEndDown"))
        {
            if (States.Instance.GetExersiciseValue("Talk_0_0"))
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
        if (States.Instance.GetExersiciseValue("BedSittingLegsDown") && States.Instance.GetExersiciseValue("BedEndDown") && !States.Instance.GetExersiciseValue("BedCompleted"))
            SimCallback("BedCompleted");

        if(Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
	}
}