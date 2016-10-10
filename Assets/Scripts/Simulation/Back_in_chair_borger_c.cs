using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Back_in_chair_borger_c : MonoBehaviour 
{	
	private bool armRestOnGroundL = false;
	private bool armRestOnGroundR = false;
	private Vector3 armRestPosL;
	private Quaternion armRestRotL;
	private Vector3 armRestPosR;
	private Quaternion armRestRotR;

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
            tg.SetToolCorrectness("Spielerdug", true);
            tg.SetToolCorrectness("Skammel", true);
        }
		tg = Util.ToggleResource<ToolGrid>("ToolGrid");
		
		HUDTiled thud = Util.ToggleResource<HUDTiled>("HUD_Wheel_Chair");
		if(thud)
		{
			thud.Buttons[(int)SlideMatWheelChair.Position.LEFT].Correct = true;
			thud.Buttons[(int)SlideMatWheelChair.Position.RIGHT].Correct = true;
		}
		thud = Util.ToggleResource<HUDTiled>("HUD_Wheel_Chair");
	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "ArmRestRemovedRight" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_Wheel_Chair(Clone)#2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "ArmRestAddedRight" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_4"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "ArmRestRemovedLeft" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_5"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "HUD_Wheel_Chair(Clone)#1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_6"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "ArmRestAddedLeft" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_7"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "chairPlaced" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_8"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "hoover_left_knee" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_9"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_10"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "hoover_right_knee" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_in_chair_c_state_11"), 0.0f);
		States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, "", 5.0f);
        
        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_back_in_chair_c_help_0");
        Help.Instance.AddHelpText(new string[] { "ToolboxDone" }, "sim_back_in_chair_c_help_1");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_back_in_chair_c_help_2");
        Help.Instance.AddHelpText(new string[] { "ArmRestRemovedRight" }, "sim_back_in_chair_c_help_3");
        Help.Instance.AddHelpText(new string[] { "HUD_Wheel_Chair(Clone)#2" }, "sim_back_in_chair_c_help_4");
        Help.Instance.AddHelpText(new string[] { "ArmRestAddedRight" }, "sim_back_in_chair_c_help_5");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_back_in_chair_c_help_6");
        Help.Instance.AddHelpText(new string[] { "ArmRestRemovedLeft" }, "sim_back_in_chair_c_help_7");
        Help.Instance.AddHelpText(new string[] { "HUD_Wheel_Chair(Clone)#1" }, "sim_back_in_chair_c_help_8");
        Help.Instance.AddHelpText(new string[] { "ArmRestAddedLeft" }, "sim_back_in_chair_c_help_9");
        Help.Instance.AddHelpText(new string[] { "chairPlaced" }, "sim_back_in_chair_c_help_10");
        Help.Instance.AddHelpText(new string[] { "hoover_left_knee" }, "sim_back_in_chair_c_help_11");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_back_in_chair_c_help_12");
        Help.Instance.AddHelpText(new string[] { "hoover_right_knee" }, "sim_back_in_chair_c_help_13");
	 
		
        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_back_in_chair_c_talk_0");
        Talk.Instance.AddTalkDialog("Talk_0_0", pos1, "sim_back_in_chair_c_talk_1");
        Talk.Instance.AddTalkDialog(pos1, "sim_back_in_chair_c_talk_2");
        Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_back_in_chair_c_talk_3");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("240_10");
		AnimateFemale.Instance.AddAnimation(new string[] { "HUD_Wheel_Chair(Clone)#1" }, "PlaceLeftSlideMat", "240_80", 1.0f, false, 10, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "HUD_Wheel_Chair(Clone)#2" }, "PlaceRightSlideMat", "240_40", 1.0f, false, 10, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "hoover_left_knee" }, "HandLeft", "250_100", 1.0f, false, 20, 1.0f, 0.0f);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "WeightLeft", "250_110", 0.0f, false, 30);
		AnimateFemale.Instance.AddAnimation(new string[] { "hoover_right_knee" }, "HandRight", "250_120", 0.0f, false, 40);
		AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "WeightRight", "250_130", 0.0f, false, 50);
        
        // Animations Borger
        AnimateBorger.Instance.SetIdle("240_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "TiltLeft", "240_20", 0.0f, false, 10, -1.0f, 2.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "TiltRight", "240_60", 0.0f, false, 10, -1.0f, 2.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "hoover_left_knee" }, "HandLeft", "250_100", 1.0f, false, 20, 1.0f, 0.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "PushRight", "250_110", 0.0f, false, 30, -1.0f, 1.0f);
		AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "PushLeft", "250_130", 0.0f, false, 30, -1.0f, 1.0f);
		
		// Animate Wheel Chair
		AnimateWheelChair3.Instance.SetIdle("base");
		AnimateWheelChair3.Instance.AddAnimation(new string[] { "start" }, "FeetUp", "feet_support_up", 0.0f, true, 10, 1.0f, 0.0f);
		AnimateWheelChair3.Instance.AddAnimation(new string[] { "start" }, "Lock", "lock", 0.0f, true, 10, 1.0f, 0.0f);
		
		// AnimateSlideMat
		AnimateWheelChairSlideMat_1.Instance.SetIdle("240_10_1");
		AnimateWheelChairSlideMat_1.Instance.AddResetState(new string[] { "HUD_Wheel_Chair(Clone)#2" });
		
		AnimateWheelChairSlideMat_2.Instance.SetIdle("240_10_2");
		AnimateWheelChairSlideMat_2.Instance.AddResetState(new string[] { "HUD_Wheel_Chair(Clone)#1" });
		
		// Animate Camera
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "start" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "ArmRestRemovedRight" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "ArmRestAddedRight" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "ArmRestRemovedLeft" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "ArmRestAddedLeft" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "HUD_Wheel_Chair(Clone)#2" }, 1, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "HUD_Wheel_Chair(Clone)#1" }, 2, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "Talk_0_0" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "Talk_0_1" }, 0, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "Talk_0_2" }, 3, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "Talk_0_3" }, 3, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "hoover_right_knee" }, 3, 0.0f, 1.0f);
		MoveAndFadeCamera.Instance.AddCameraFade(new string[] { "hoover_left_knee" }, 3, 0.0f, 1.0f);


        AnimateChair1.Instance.AddAnimation(new string[] { "chairPlaced" }, "chairplaced", "250_95", 0.0f, false, 10);
        AnimateChair1.Instance.AddResetState(new string[] { "chairPlaced" });
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;
		
		if(t == "hoover_ArmRest_Right")
		{
			GameObject ar1 = GameObject.Find("ArmRest_right");
			if(!armRestOnGroundR && ar1) {
				t = "ArmRestRemovedRight";
			}
			else if(ar1) {
				t = "ArmRestAddedRight";
			}
		}
		else if(t == "hoover_ArmRest_Left")
		{
			GameObject ar1 = GameObject.Find("ArmRest_left");
			if(!armRestOnGroundL && ar1) {
				t = "ArmRestRemovedLeft";
			}
			else if(ar1) {
				t = "ArmRestAddedLeft";
			}
		}

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
                if (t == "ArmRestRemovedRight")
                {
                    GameObject ar1 = GameObject.Find("ArmRest_right");
                    armRestPosR = ar1.transform.position;
                    armRestRotR = ar1.transform.rotation;
                    ar1.transform.localPosition += new Vector3(0.30f, -0.6f, 0.25f);
                    ar1.transform.localRotation = Quaternion.Euler(ar1.transform.localRotation.eulerAngles + new Vector3(0.0f, 0.0f, -90.0f));
                    armRestOnGroundR = true;
                }
                else if (t == "ArmRestAddedRight")
                {
                    GameObject ar1 = GameObject.Find("ArmRest_right");
                    ar1.transform.position = armRestPosR;
                    ar1.transform.rotation = armRestRotR;
                    armRestOnGroundR = false;
                }
                else if (t == "ArmRestRemovedLeft")
                {
                    GameObject ar1 = GameObject.Find("ArmRest_left");
                    armRestPosL = ar1.transform.position;
                    armRestRotL = ar1.transform.rotation;
                    ar1.transform.localPosition += new Vector3(-0.18f, -0.5f, 0.25f);
                    ar1.transform.localRotation = Quaternion.Euler(ar1.transform.localRotation.eulerAngles + new Vector3(270.0f, 0.0f, 0.0f));
                    armRestOnGroundL = true;
                }
                else if (t == "ArmRestAddedLeft")
                {
                    GameObject ar1 = GameObject.Find("ArmRest_left");
                    ar1.transform.position = armRestPosL;
                    ar1.transform.rotation = armRestRotL;
                }

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
				AnimateWheelChair3.Instance.UpdateAnimation(t);
				AnimateWheelChairSlideMat_1.Instance.UpdateAnimation(t);
				AnimateWheelChairSlideMat_2.Instance.UpdateAnimation(t);
				MoveAndFadeCamera.Instance.UpdateCameraFade(t);
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

    //public List<string> helpSpeak = new List<string>();
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