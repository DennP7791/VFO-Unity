using UnityEngine;
using System.Collections;
using ExerciseCollections;

public class Results : MonoBehaviour {
	
	private static Results _instance; //singleton

    private double score = (double)Global.Instance.MaxStars; 


    public static Results Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Results)GameObject.FindObjectOfType(typeof(Results));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Results";
                    _instance = (Results)container.AddComponent(typeof(Results));
                }
            }

            return _instance;
        }	
    }
	
	private string results = "\n\n";
	private bool showResults;
	private bool failed = false;
    private bool help = false;
	
	private Rect rectResultWindow = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 200, 500, 400);
	
	public bool Failed
	{
		get { return failed; }
		set { failed = value;}
	}
			
	/// <summary>
    ///    Shows the result window. Use ShowResult(float delay) to instiance results with a timed delay in seconds.
    /// </summary>
	public void ShowResults()
	{
        if (failed)
            score = 0.0;
		
        Help.Instance.HideHelpText();

        // Just to play speak at right time
        if (!failed)
        {
            string s = help ? Text.Instance.GetStringAndPlaySpeak("results_passed_help") : Text.Instance.GetStringAndPlaySpeak("results_passed_test");
        }
        else if (failed && GetScore() <= 0)
        {
            string s = Text.Instance.GetStringAndPlaySpeak("results_too_many_errors");
        }

		string clickTo = "\n\n" + Text.Instance.GetString("results_main_menu");
        if (failed) clickTo = "\n\n" + Text.Instance.GetString("results_start_over");
		Util.OkMessageBox(rectResultWindow, results + clickTo, true, Message.Type.Info, OkPressed);
	}

    public void OkPressed(Message message, bool value)
	{
        if (!Global.Instance.RunSimulationWithHelp)
            Global.Instance.updateScore(score);        
        
        if (!failed)
            SceneLoader.Instance.CurrentScene = 0;
        else
            SceneLoader.Instance.CurrentScene = SceneLoader.Instance.CurrentScene;
	}
	
	// Waits an amount before 
	private IEnumerator DelayResults(float delay)
	{
		yield return new WaitForSeconds(delay);
		ShowResults();
        StarFade.Instance.ShowStar(new Rect((Screen.width / 2 - 238), Screen.height / 2 - 186, 238, 186), true);
	}
	
	/// <summary>
	/// Depriciated
	/// </summary>
	/// <param name='delay'>
	/// Float representing the delay in seconds
	/// </param>
	public void ShowResults(float delay)
	{
		StartCoroutine(DelayResults(delay + 0.5f));	
	}

    public void ShowResults(bool f, bool h, string r, float d)
    {
        failed = f;
        help = h;
        string _r = r;


        if (!f && GetScore() <= 0)
        {
            failed = true;
            _r = Text.Instance.GetString("results_too_many_errors");
        }

        results += _r + "\n";

        StartCoroutine(DelayResults(d));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    /// <param name="r"></param>
    /// <param name="d"></param>
    public void ShowResults(bool f, string r, float d)
    {
        ShowResults(f, false, r, d);
    }

    /// <summary>
    /// Subrtact a star from the score
    /// </summary>
    public void SubtractStar()
    {
        score -= 1.0;
        if (score <= 0.0)
            score = 0.0;
    }

    public int GetScore()
    {
        return (int)score;
    }

    /// <summary>
    /// Depriciated
    /// </summary>
    /// <param name='delay'>
    /// Float representing the delay in seconds
    /// </param>
    public void ShowResults(float delay, double s)
    {
        if (!Global.Instance.RunSimulationWithHelp)
        {
            score = s;
        }
        StartCoroutine(DelayResults(delay));
    }
	
	
	/// <summary>
	/// Adds a string to the results list (TODO: make this whole thing more than just a text string
	/// </summary>
	/// <param name='r'>
	/// A string containing the result. New line is automaticly added after r
	/// </param>
	public void AddResults(string r)
	{
		results += r + "\n";
	}
	
	// Use this for initialization
	void Start () 
	{
		//results += "Resultater: \n\n";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    void OnDestroy()
    {
        _instance = null;
    }
}
