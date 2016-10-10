using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lie_down_borger_b : MonoBehaviour 
{	
	private void initializeExercise()
	{
        ToolBox tb = Util.ToggleResource<ToolBox>("ToolBox");
        if (tb)
        {
            tb.EmptyToolBox();
        }

        GameObject go = GameObject.Find("Bed");
        if (go != null)
        {
            Util.ToggleSubElementRenderer(go, "bottom_left_slide");
            Util.ToggleSubElementRenderer(go, "bottom_right_slide");
            Util.ToggleSubElementRenderer(go, "up_left_slide");
            Util.ToggleSubElementRenderer(go, "up_right_slide");
            Util.ToggleSubElementRenderer(go, "antislide");
        }
		
		ToolGrid tg = Util.ToggleResource<ToolGrid>("ToolGrid");
        if (tg)
        {
            tg.SetToolCorrectness("Lagen", true);
			tg.SetToolCorrectness("Spielerdug", true);
            tg.SetToolCorrectness("Sengekontrol", true);
        }
		Util.ToggleResource<ToolGrid>("ToolGrid");
		
		HUDTiled bhud = Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");
		if(bhud)
		{
			bhud.Buttons[(int)BedSheet.Position.BEDSIDE].Correct = true;
		}
		Util.ToggleResource<HUDTiled>("HUD_Bedsheet2");
		
		HUD thud = Util.ToggleResource<HUD>("HUD_SlideMat2");
		if(thud)
		{
			thud.Buttons[(int)SlideMat.Position.BOTTOMRIGHT].Correct = true;
		}
		Util.ToggleResource<HUD>("HUD_SlideMat2");
	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "BedHeadUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_lie_down_b_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.Critical, Text.Instance.GetString("sim_lie_down_b_state_1"), "", 0.0f, 0);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_lie_down_b_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "hoover_sengegaerde" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_lie_down_b_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMat2(Clone)#3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_lie_down_b_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_Bedsheet2(Clone)#0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_lie_down_b_state_5"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, "", 7.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_lie_down_b_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_lie_down_b_help_1");
        Help.Instance.AddHelpText(new string[] { "BedHeadUp" }, "sim_lie_down_b_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_lie_down_b_help_3");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_lie_down_b_help_4");
        Help.Instance.AddHelpText(new string[] { "hoover_sengegaerde" }, "sim_lie_down_b_help_5");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMat2(Clone)#3" }, "sim_lie_down_b_help_6");
        Help.Instance.AddHelpText(new string[] { "HUD_Bedsheet2(Clone)#0" }, "sim_lie_down_b_help_7");
		
        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_lie_down_b_talk_0");
        Talk.Instance.AddTalkDialog(pos1, "sim_lie_down_b_talk_1");
        Talk.Instance.AddTalkDialog(pos1, "sim_lie_down_b_talk_2");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("lie_down__C_idle");
		AnimateFemale.Instance.AddAnimation(new string[] { "HUD_SlideMat2(Clone)#3" }, "SlideMatBottomLeft", "place_gliding_mat_foot_end", 0.0f, false, 10);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "pull", "Bedsheet_01_294f", 0.0f, false, 20);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("130_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "SitIn", "B_bed_Sitting_Down_71f", 0.0f, false, 10);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "LeanIn", "lean_on_pillow", 0.0f, false, 20, -1.0f, 2.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "pull", "Bedsheet_01_294f", 0.0f, false, 30, -1.0f, 2.0f);
		
		// Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "pos_low", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedHeadUp" }, "head_end_pos_high_70_degree", 0.7f, 5.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "Talk_0_2" }, "head_end_pos_high_70_degree", 0.0f, 6.0f);
		
		AnimateDrawSheet2.Instance.SetIdle("lie_down_C_idle");
		AnimateDrawSheet2.Instance.AddAnimation(new string[] { "Talk_0_2" }, "pull", "bedsheet_pull_294f", 0.0f, false, 10);
		AnimateDrawSheet2.Instance.AddResetState(new string[] { "HUD_Bedsheet2(Clone)#0" });

        AnimateSlideMat2.Instance.AddResetState(new string[] { "HUD_SlideMat2(Clone)#3" });
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
				
				if(t == "hoover_sengegaerde")
				{
					GameObject.Find("click_sengegaerde").active = false;
					GameObject.Find("hoover_sengegaerde").active = false;
					GameObject.Find("Bed_top").active = false;	
				}
				else if(t == "HUD_SlideMat2(Clone)#3")
				{
					GameObject.Find("bottom_right_slide").GetComponent<Renderer>().enabled = true;
				}

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
				AnimateBed3.Instance.UpdateAnimation(t);
				AnimateDrawSheet2.Instance.UpdateAnimation(t);
                AnimateSlideMat2.Instance.UpdateAnimation(t);
                
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