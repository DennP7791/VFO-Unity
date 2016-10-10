using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Temporary help class til we create something with more learning, images etc.
public class Help : MonoBehaviour {
	
	private static Help _instance; //singleton

    public static Help Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Help)GameObject.FindObjectOfType(typeof(Help));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Help";
                    _instance = (Help)container.AddComponent(typeof(Help));
                }
            }

            return _instance;
        }	
    }
	
//	private bool showHelp = false;
	private string helpText = "";
	
	private HelpBox msg;
	
	private List<string> LHelpText = new List<string>();
    private List<string> LHelpState = new List<string>();

    private string currentState = "";
	
	/// <summary>
    ///     Adds text to a list of help texts used in the simulation
    /// </summary>
    /// <param name="text">string containing the text, use \n for new line</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void AddHelpText(string[] statearr, string text)
	{
        for (int i = 0; i < statearr.Length; ++i)
        {
            LHelpText.Add(text);
            LHelpState.Add(statearr[i]);
        }
	}

    // Depriciated
    public void AddHelpText(string text)
    {
        LHelpText.Add(text);
    }
	
	/// <summary>
    ///     Sets the current help text (changes to a new position in the list of help texts for the simulation)
    /// </summary>
    /// <param name="state">integer representing the state / position</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void SetHelpState(int state)
	{
		helpText = LHelpText[state];
		msg.Text = helpText;
	}
	
	/// <summary>
    ///     Toggle help window and text on
    /// </summary>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void ShowHelpText()
	{
		msg.enabled = true;
		msg.Text = helpText;
	}
	
	/// <summary>
    ///     Toggles help window and text off
    /// </summary>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void HideHelpText()
	{
		msg.enabled = false;
//		showHelp = false;	
	}
	
	// Use this for initialization
	void Awake () {
	 	msg = Util.HelpBox("");
		//msg.textStyle.fontSize = 16;
		msg.enabled = false;
	}

    void OnDestroy()
    {
        _instance = null;
    }

    public void UpdateHelp(string state)
    {
        if (state != currentState)
        {
            currentState = state;
            int pos = LHelpState.IndexOf(currentState);
            if (pos != -1)
            {
                msg.Text = Text.Instance.GetStringAndPlaySpeak(LHelpText[pos]);
            }
        }
    }
}
