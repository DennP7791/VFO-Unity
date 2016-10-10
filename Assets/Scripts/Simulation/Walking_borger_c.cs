using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Walking_borger_c : MonoBehaviour
{
    private void initializeExercise()
    {
    }

    private void defineExercise()
    {
        // Exercise States
        States.Instance.InitExerciseState(new string[] { "hoover_upper_torso", "hoover_hips" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_walk_c_state_place_hand"), 3.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_0_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_walk_c_state_walk_helper"), 3.0f);
        States.Instance.InitExerciseState(new string[] { "Talk_1_0" }, States.CheckCon.NotCritical, Text.Instance.GetString("sim_walk_c_state_walk_borger"), 5.0f);

        // Help text
        Help.Instance.AddHelpText(new string[] { "start" }, "sim_walk_c_help_place_hand");
        Help.Instance.AddHelpText(new string[] { "hoover_hips", "hoover_upper_torso" }, "sim_walk_c_help_walk_helper");
        Help.Instance.AddHelpText(new string[] { "Talk_0_0" }, "sim_walk_c_help_walk_borger");

        // Dialog
        Talk.Instance.AddTalkDialog("clickedHelper", "sim_walk_c_talk_walk_helper");
        Talk.Instance.AddTalkDialog("hoover_head", "sim_walk_c_talk_walk_borger");

        // Animations Female Helper
        AnimateFemale.Instance.SetIdle("200_10");
        AnimateFemale.Instance.AddAnimation(new string[] { "hoover_hips" }, "HandOnHips", "200_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "hoover_upper_torso" }, "HandOnSholder", "200_20", 0.0f, false, 10);
        AnimateFemale.Instance.AddAnimation(new string[] { "Talk_1_0" }, "Walking1", "200_30", 0.0f, false, 20);

        // Animations Borger
        AnimateBorger.Instance.SetIdle("200_10");
        AnimateBorger.Instance.AddAnimation(new string[] { "Talk_1_0" }, "Walking", "200_30", 0.0f, false, 10);

        //Animations Helper
        AnimateMale.Instance.SetIdle("210_25");
        AnimateMale.Instance.AddAnimation(new string[] { "Talk_1_0" }, "Walking", "210_30", 0.0f, false, 10);

        // Animate Walker
        AnimateWalker2.Instance.AddAnimation(new string[] { "Talk_1_0" }, "Walking", "200_30", 0.0f, false, 10);

        //Animate Wheelchair
        AnimateWheelChair3.Instance.AddAnimation(new string[] { "Talk_1_0" }, "Walking", "210_30", 0.0f, false, 10);
    }

    public void SimCallback(string t)
    {
        if (States.Instance.GetStateValueB("showingErrorMessage"))
            return;

        string femaleState = t;

        if (t == "hoover_hips")
        {
            States.Instance.PushState("handsOnHips", "yes");
            States.Instance.PushState("handOnSholder", "no");
        }

        if (t == "hoover_upper_torso")
        {
            States.Instance.PushState("handOnSholder", "yes");
            States.Instance.PushState("handsOnHips", "no");
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
                if (help)
                {
                    Help.Instance.UpdateHelp(t);
                    /*int pos = Help.Instance.GetSpeak(t);
                    if (pos != -1)
                        playHelpClip.PlayClip(pos);*/ 
                }

                Talk.Instance.UpdateTalk(t);
                AnimateFemale.Instance.UpdateAnimation(femaleState);
                AnimateBorger.Instance.UpdateAnimation(t);
                AnimateWalker2.Instance.UpdateAnimation(t);
                AnimateMale.Instance.UpdateAnimation(t);
                AnimateWheelChair3.Instance.UpdateAnimation(t);

                if (States.Instance.HasFinished())
                {
                    if (States.Instance.GetStateValueB("handsOnHips") && Results.Instance.GetScore() > 1)
                        Results.Instance.SubtractStar();

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
    void Start()
    {
        /*if (!GetComponent<PlayHelpClip>())
            gameObject.AddComponent("PlayHelpClip");
        playHelpClip = GetComponent<PlayHelpClip>();
        playHelpClip.AddHelpClips(_helpSpeak);*/ 

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