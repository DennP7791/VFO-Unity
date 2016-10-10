using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Back_in_chair_borger_b_new : MonoBehaviour
{
    private void initializeExercise()
    {
    }

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "hoover_lock" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_chair_b_new_state_0"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_chair_b_new_state_1"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_1" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_chair_b_new_state_2"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_2" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_back_chair_b_new_state_3"), 0.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_3" }, States.CheckCon.NotCritical, "", 8.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_back_chair_b_new_help_0");
        Help.Instance.AddHelpText(new string[] { "hoover_lock" }, "sim_back_chair_b_new_help_1");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_back_chair_b_new_help_2");
        Help.Instance.AddHelpText(new string[] { "Talk_0_1" }, "sim_back_chair_b_new_help_3");
        Help.Instance.AddHelpText(new string[] { "Talk_0_2" }, "sim_back_chair_b_new_help_4");

        // Dialog
        int pos1 = Talk.Instance.AddTalkDialog("hoover_head", "sim_back_chair_b_new_talk_0");
        Talk.Instance.AddTalkDialog("Talk_0_0", pos1, "sim_back_chair_b_new_talk_1");
        Talk.Instance.AddTalkDialog(pos1, "sim_back_chair_b_new_talk_2");
        Talk.Instance.AddTalkDialog("Talk_0_2", pos1, "sim_back_chair_b_new_talk_3");

        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("245_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_2" }, "BackRight", "245_50", 0.0f, false, 30);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_0_3" }, "BackLeft", "245_60", 0.0f, false, 40);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("245_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_0" }, "MoveFeet", "245_30", 0.0f, false, 10);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_1" }, "PutHands", "245_40", 0.0f, false, 20);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_2" }, "BackRight", "245_50", 0.0f, false, 30);
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_0_3" }, "BackLeft", "245_60", 0.0f, false, 40);

        // Animate Wheel Chair
        AnimateWheelChair3.Instance.SetIdle("base");
        AnimateWheelChair3.Instance.AddAnimation(new string[] { "start" }, "FeetUp", "feet_support_up", 0.0f, true, 10, 1.0f, 0.0f);
        AnimateWheelChair3.Instance.AddAnimation(new string[] { "hoover_lock" }, "Lock", "lock", 0.0f, true, 10, 1.0f, 0.0f);
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
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(t);
                AnimateBorger.Instance.UpdateAnimation(t);
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

    //public List<string> helpSpeak = new List<string>();
    //PlayHelpClip playHelpClip;

    // Use this for initialization
    void Start()
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
        if (SceneLoader.Instance.CurrentScene != -1)
        {
            help = Global.Instance.RunSimulationWithHelp;
        }
        else
        {
            States.Instance.PushState("DEBUG");
            GameObject.Instantiate((GameObject)Resources.Load("BottomBar"));
            GameObject.Instantiate((GameObject)Resources.Load("TopBar"));
        }

        // Initialize and define simulation
        initializeExercise();
        defineExercise();

        if (help)
        {
            Help.Instance.ShowHelpText();
        }

        // trick for not displaying the toolbox
        States.Instance.PushState("ToolboxDone", "yes");

        // Start the simulation
        SimCallback("start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            States.Instance.DebugState();
        }
    }
}