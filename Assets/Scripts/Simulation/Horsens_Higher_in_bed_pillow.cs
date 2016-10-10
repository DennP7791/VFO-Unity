using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Horsens_Higher_in_bed_pillow : MonoBehaviour 
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
            tg.SetToolCorrectness("Spielerdug", true);
            tg.SetToolCorrectness("Sengekontrol", true);
            tg.SetToolCorrectness("Skridsikker", true);
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

        HUD thud = Util.ToggleResource<HUD>("HUD_SlideMatPillow");
        if (thud)
        {
            thud.Buttons[(int)AntiSlideMat.Position.TOP].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_SlideMatPillow");

        HUD shud = Util.ToggleResource<HUD>("HUD_AntiSlideMat2");
        if (shud)
        {
            shud.Buttons[(int)AntiSlideMat.Position.BOTTOM].Correct = true;
        }
        Util.ToggleResource<HUD>("HUD_AntiSlideMat2");

        //MouseOverObject.Instance.HoverSlidemat = true;
	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_SlideMatPillow(Clone)#0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndUp" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_4" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_55"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_6"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_5" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_hor_high_bed_pil_state_7"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "BedEndDown" }, States.CheckCon.NotCritical, "", 6.0f);
        //States.Instance.InitExerciseState(new string[] { "Talk_0_6" }, States.CheckCon.NotCritical, "", 12.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_hor_high_bed_pil_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_hor_high_bed_pil_help_1");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_hor_high_bed_pil_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_hor_high_bed_pil_help_3");
        Help.Instance.AddHelpText(new string[] { "HUD_SlideMatPillow(Clone)#0" }, "sim_hor_high_bed_pil_help_4");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_hor_high_bed_pil_help_5");
        Help.Instance.AddHelpText(new string[] { "Talk_0_3" }, "sim_hor_high_bed_pil_help_6");
        Help.Instance.AddHelpText(new string[] { "BedEndUp" }, "sim_hor_high_bed_pil_help_65");
        Help.Instance.AddHelpText(new string[] { "Talk_0_4" }, "sim_hor_high_bed_pil_help_7");
        Help.Instance.AddHelpText(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "sim_hor_high_bed_pil_help_8");
        Help.Instance.AddHelpText(new string[] { "Talk_0_5" }, "sim_hor_high_bed_pil_help_9");
        //Help.Instance.AddHelpText(new string[] { "BedEndDown" }, "sim_hor_high_bed_pil_help_10");

        // Dialog
        int pos = Talk.Instance.AddTalkDialog("hoover_head", "sim_hor_high_bed_pil_talk_0");
        Talk.Instance.AddTalkDialog("Talk_0_0", pos, "sim_hor_high_bed_pil_talk_1");
        Talk.Instance.AddTalkDialog("HUD_SlideMatPillow(Clone)#0", pos, "sim_hor_high_bed_pil_talk_2");
        Talk.Instance.AddTalkDialog("Talk_0_2", pos, "sim_hor_high_bed_pil_talk_3");
        Talk.Instance.AddTalkDialog(pos, "sim_hor_high_bed_pil_talk_35");
        Talk.Instance.AddTalkDialog("HUD_SlideMatPillow(Clone)#0", pos, "sim_hor_high_bed_pil_talk_4");
        //Talk.Instance.AddTalkDialog("HUD_SlideMatPillow(Clone)#0", pos, "sim_hor_high_bed_pil_talk_5");

        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("2030_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "", "2030_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "Look2", "2030_30", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_SlideMatPillow(Clone)#0" }, "SlidematPlaced", "2030_40", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "Look3", "2030_50", 0.0f, false, 40);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "Look4", "2030_60", 0.0f, false, 50);
        AnimateFemale.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "AntiSlideMatPlaced", "2030_90", 0.0f, false, 70);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftButt", "2030_100", 0.0f, false, 80);
        //AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_6" }, "LiftHead", "2030_120", 0.0f, false, 100);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("2030_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "Look1", "2030_20", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "Look2", "2030_30", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "HUD_SlideMatPillow(Clone)#0" }, "SlidematPlaced", "2030_40", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "Look3", "2030_50", 0.0f, false, 40);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "Look4", "2030_60", 0.0f, false, 50);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedEndUp" }, "FootEnd1", "2030_70", 0.0f, false, 60);
        AnimateBorger.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "AntiSlideMatPlaced", "2030_90", 0.0f, false, 70);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftButt", "2030_100", 0.0f, false, 80);
        AnimateBorger.Instance.AddAnimation(new string[] { "BedEndDown" }, "FootEnd2", "2030_110", 0.0f, false, 90);
        //AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_6" }, "LiftHead", "2030_120", 1.64f, false, 100);

        // Animate Bed
        AnimateBed3.Instance.SetIdle("base");
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "start" }, "rail_down_left", 1.0f, 0.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndUp" }, "2030_70", 1.0f, 2.0f);
        AnimateBed3.Instance.AddAnimation(new string[] { "BedEndDown" }, "2030_70", 0.0f, 2.0f);

        // Animate Slidemat
        AnimateSlideMat2.Instance.SetIdle("2030_10");
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "HUD_SlideMatPillow(Clone)#0" }, "SlidematPlaced", "2030_40", 0.0f, false, 10, 1.0f, 0.0f);
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "Talk_0_2" }, "Look3", "2030_50", 0.0f, false, 40);
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "Talk_0_3" }, "Look4", "2030_60", 0.0f, false, 50);
        AnimateSlideMat2.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftButt", "2030_100", 0.0f, false, 80);
        //AnimateSlideMat2.Instance.AddAnimation(new string[] { "Talk_0_6" }, "LiftHead", "2030_120", 0.0f, false, 100);
        AnimateSlideMat2.Instance.AddResetState(new string[] { "HUD_SlideMatPillow(Clone)#0" });

        // Animate Antislidemat
        AnimateAntiSlideMat.Instance.AddAnimation(new string[] { "HUD_AntiSlideMat2(Clone)#2" }, "PlaceAntiSlideMat", "2030_90", 0.0f, false, 10);
        AnimateAntiSlideMat.Instance.AddAnimation(new string[] { "BedEndDown" }, "BedEndDown2", "2030_110", 0.0f, false, 20);//, 0.0f, 4.0f);
        AnimateAntiSlideMat.Instance.AddResetState(new string[] { "HUD_AntiSlideMat2(Clone)#2" });

        AnimatePillow.Instance.SetIdle("2030_10");
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_0" }, "Look1", "2030_20", 0.0f, false, 10);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_1" }, "Look2", "2030_30", 0.0f, false, 20);
        AnimatePillow.Instance.AddAnimation(new string[] { "Talk_0_5" }, "LiftButt", "2040_120", 1.8f, false, 30);

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
                AnimateSlideMat2.Instance.UpdateAnimation(t);
                AnimateAntiSlideMat.Instance.UpdateAnimation(t);
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