using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Place_sling_in_chair : MonoBehaviour 
{	
	private void initializeExercise()
	{

	}

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "hoover_lock" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_chair_state_lockchair"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_chair_state_chair"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_chair_state_lock"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_sling_chair_state_lean"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, "", 32.0f);


        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_sling_chair_help_01");
        Help.Instance.AddHelpText(new string[] { "hoover_lock" }, "sim_sling_chair_help_0");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_sling_chair_help_1");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_sling_chair_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_sling_chair_help_3"); ;

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_sling_chair_talk_0");
        Talk.Instance.AddTalkDialog(pos1, "sim_sling_chair_talk_1");
        Talk.Instance.AddTalkDialog(pos1, "sim_sling_chair_talk_2");
		Talk.Instance.AddTalkDialog(pos1, "sim_sling_chair_talk_3");
        
        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("520_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_0" }, "LeanForward", "520_30", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "LeanBack", "520_50", 0.0f, false, 20);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "PullStrops", "520_60", 4.4f, false, 30);

        // Animate Male Helper
        AnimateMale.Instance.SetIdle("520_10");
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_1" }, "PlaceSling", "520_40", 0.0f, false, 10);
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "PullStrops", "520_60", 0.0f, false, 30, -1.0f, 2.0f);

         // AnimateSling
        AnimateSling2.Instance.SetIdle("520_10");
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_1" }, "PlaceSling", "520_40", 0.0f, false, 10, -1.0f, 0.5f);
		AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_2" }, "LeanBack", "520_50", 1.0f, false, 20);
        AnimateSling2.Instance.AddAnimation(new string[] { "Talk_0_3" }, "PullStrops", "520_60", 4.4f, false, 30);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("520_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "LeanForward", "520_30", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "LeanBack", "520_50", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "PullStrops", "520_60", 4.4f, false, 30);

        //Animate Wheelchair
        AnimateWheelChair3.Instance.SetIdle("520_10");
        AnimateWheelChair3.Instance.AddAnimation(new string[] { "hoover_lock" }, "LockChair", "1110_20", 0.0f, false, 10);
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
                AnimateMale.Instance.UpdateAnimation(t);
                AnimateSling2.Instance.UpdateAnimation(t);
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
        if(Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
	}
}