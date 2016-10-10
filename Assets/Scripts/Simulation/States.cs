using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Simple class for handleing simulation states / actions
public class States : MonoBehaviour 
{
	private static States _instance; //singleton

    public static States Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (States)GameObject.FindObjectOfType(typeof(States));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "States";
                    _instance = (States)container.AddComponent(typeof(States));
                }
            }

            return _instance;
        }
    }

    private class Exercise
    {
        private List<string> eStateName;
        private string eStateError;
        private bool eStateValue;
        private bool eStateCritical;
        private CheckCon eStateCondition;
        private bool eStateOnlyWithHelp;
        private float eStateTime;
        private float eStateDelay;
        private int eStateVideo;
        private string eStateRemember;
        private string returnEstateRemember;

        public Exercise(string[] statearr, CheckCon condition, string error, string remember, float delay, int video)
        {
            eStateName = new List<string>();
            for (int i = 0; i < statearr.Length; ++i)
                eStateName.Add(statearr[i]);

            eStateError = error;
            eStateValue = false;
            eStateCondition = condition;
            eStateTime = 0.0f;
            eStateDelay = delay;
            eStateVideo = video;
            eStateRemember = remember;
            returnEstateRemember = "";
        }

        public bool hasName(string name)
        {
            bool v = false;

            int pos = eStateName.IndexOf(name);
            if (pos != -1)
            {
                v = true;

                if (pos > 0)
                {
                    Debug.Log("checkingHasName: " + name);
                    returnEstateRemember = eStateRemember;
                }
                else
                {
                    Debug.Log("checkingHasName: " + name);
                    returnEstateRemember = "";
                }
            }

            return v;
        }

        public bool Value
        {
            get { return eStateValue; }
            set { eStateValue = value; eStateTime = Time.realtimeSinceStartup; }
        }

        public string Error
        {
            get { return eStateError; }
        }

        public int Condition
        {
            get { return (int)eStateCondition; }
        }

        public float Delay
        {
            get { return eStateDelay; }
        }

        public int Video
        {
            get { return eStateVideo; }
        }

        public string RememberString
        {
            get { return returnEstateRemember; }
        }
    }

    public enum CheckCon 
    { 
        NotCritical         = 1,
        Critical            = 2,
        OnlyCheckWithHelp   = 3,
        VideoError          = 4,
        ShowVideo           = 5
    };
	
	private List<string> stateName  = new  List<string>();
	private List<string> stateValue = new List<string>();
	private List<float> stateTime   = new List<float>();

    private List<Exercise> exerciseStates = new List<Exercise>();
    private List<string> eStateNameDebug = new List<string>();

    private int currentState = -1;
    private bool _help = false;

    private string currentError = "";
    private bool currentCritical = false;
    private int currentVideo = -1;

    /// <summary>
    /// Initialize an exercise state
    /// </summary>
    /// <param name="statearr">array of states</param>
    /// <param name="critical">is the state critical</param>
    /// <param name="errorMessage">error message to display</param>
    /// <param name="delay">delay before showing results</param>
    public void InitExerciseState(string[] statearr, CheckCon check, string errorMessage, float delay)
    {
        Exercise e = new Exercise(statearr, check, errorMessage, "", delay, -1);
        exerciseStates.Add(e);
    }


   /// <summary>
   /// Initialize an exercise state
   /// </summary>
   /// <param name="statearr">array of states</param>
   /// <param name="critical">is the state critical</param>
   /// <param name="errorMessage">error message to display</param>
   /// <param name="rememberMessage">message displayed in results as a notice about none critical error</param>
   /// <param name="delay">delay before showing results</param>
    public void InitExerciseState(string[] statearr, CheckCon check, string errorMessage, string rememberMessage, float delay)
    {
        Exercise e = new Exercise(statearr, check, errorMessage, rememberMessage, delay, -1);
        exerciseStates.Add(e);
    }

    /// <summary>
    /// Initialize an exercise state
    /// </summary>
    /// <param name="statearr">array of states</param>
    /// <param name="critical">is the state critical</param>
    /// <param name="errorMessage">error message to display</param>
    /// <param name="rememberMessage">message displayed in results as a notice about none critical error</param>
    /// <param name="delay">delay before showing results</param>
    public void InitExerciseState(string[] statearr, CheckCon check, string errorMessage, string rememberMessage, float delay, int video)
    {
        Exercise e = new Exercise(statearr, check, errorMessage, rememberMessage, delay, video);
        exerciseStates.Add(e);
    }

    /// <summary>
    ///     Get an exercise state error
    /// </summary>
    /// <returns>
    ///     string with the error text
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
    public string GetExerciseError()
    {
        return currentError;
    }

    /// <summary>
    /// Get a video from an exercise state
    /// </summary>
    /// <param name="pos">the state number</param>
    /// <returns>the string of the video, empty if no video is attached to the state</returns>
    public int GetExerciseVideo()
    {
        int er = -1;
        for (int i = 0; i < exerciseStates.Count; i++)
        {
            if (!exerciseStates[i].Value)
            {
                er = exerciseStates[i].Video;
                break;
            }
        }
        return er;
    }

    /// <summary>
    /// Get delay for an exercise state
    /// </summary>
    /// <param name="pos">the state number</param>
    /// <returns>the delay in seconds</returns>
    public float GetExerciseDelay(int pos)
    {
        return exerciseStates[pos].Delay;
    }

    /// <summary>
    /// Is the state critical or not
    /// </summary>
    /// <param name="pos">the state number</param>
    /// <returns>true if critical, false otherwise</returns>
    public bool GetExerciseCritical(int depriciated)
    {
        bool gc = false;

        if (currentCritical || currentVideo != -1)
            gc = true;

        return gc;
    }

    /// <summary>
    /// return a string with comments about the exercise
    /// </summary>
    /// <returns>string of comments</returns>
    public string GetComments()
    {
        string rememberString = "";
        for (int i = 0; i < exerciseStates.Count; ++i)
        {
            string r = exerciseStates[i].RememberString;
            if (r != "")
            {
                rememberString += r;
                rememberString += "\n";
            }    
        }
        return rememberString;
    }

    /// <summary>
    /// Has the exercise finished (is the current state the last)
    /// </summary>
    /// <returns>true if finished, otherwise false</returns>
    public bool HasFinished()
    {
        if (currentState >= exerciseStates.Count -1)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Get the current state
    /// </summary>
    /// <returns>int representing the current state</returns>
    public int CurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Check a state by name, if it has been completed
    /// </summary>
    /// <param name="name">name of the state to check agianst</param>
    /// <returns></returns>
    public bool GetExersiciseValue(string name)
    {
        bool _v = false;

        for (int i = 0; i < exerciseStates.Count; ++i)
        {
            if (exerciseStates[i].hasName(name) && exerciseStates[i].Value)
                _v = true;
        }

        return _v;
    }

    /// <summary>
    ///     Check current state against others
    /// </summary>
    /// <param name="name">Name of the state</param>
    /// <param name="help">Check in help mode or test mode</param>
    /// <returns>
    ///     -1 if there is no errors, otherwise the position in the statelist of the problem.
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
    public int UpdateState(string name, bool help)
    {
        _help = help;
        int _currentState = -1;
		int errorCode = -1;
		
		// Get the current exericse
        for (int i = 0; i < exerciseStates.Count; ++i)
        {
            if (exerciseStates[i].hasName(name))
            {
                _currentState = i;
                break;
            }
        }
		
		// Get List of Possible Errors
		List<int> MyErrors = new  List<int>();
		List<int> MyConditions = new List<int>();
		
		if(_currentState != -1)
		{
			for(int i = 0; i < _currentState; ++i)
			{
				if(!exerciseStates[i].Value && i != _currentState)
				{
					MyErrors.Add(i);
					MyConditions.Add(exerciseStates[i].Condition);
				}
			}
		}
		
		// Remove errorstates from the list if not applyable
		if(MyErrors.Count > 0)
		{
			for(int i = 0; i < MyErrors.Count; ++i)
			{
				if(exerciseStates[_currentState].Condition != (int)CheckCon.ShowVideo && MyConditions[i] == (int)CheckCon.VideoError)
				{
					MyConditions.RemoveAt(i);
					MyErrors.RemoveAt(i);
				}
				else if(!help && MyConditions[i] == (int)CheckCon.OnlyCheckWithHelp)
				{
					MyConditions.RemoveAt(i);
					MyErrors.RemoveAt(i);
				}
			}
		}
		
		if(_currentState != -1)
		{		
			// Return remaining errors  if applyable
			if(MyErrors.Count > 0)
			{
				// return critical video error
				int videoPos 		= MyConditions.IndexOf((int)CheckCon.VideoError);
				int criticalPos		= MyConditions.IndexOf((int)CheckCon.Critical);
			
			
				if(exerciseStates[_currentState].Condition == (int)CheckCon.ShowVideo && videoPos != -1)
				{
					errorCode = MyErrors[videoPos];
					currentVideo = exerciseStates[errorCode].Video;
					currentError = exerciseStates[errorCode].Error;
					currentCritical = true;
				}
				else if(criticalPos != -1)
				{
					errorCode = MyErrors[criticalPos];
					currentError = exerciseStates[errorCode].Error;
					currentCritical = true;
				}
				else if(_currentState > 0)
				{
					errorCode = MyErrors[0];
					currentError = exerciseStates[errorCode].Error;
				}
			}
			
			if(errorCode == -1)
			{
				currentState = _currentState;
				exerciseStates[currentState].Value = true;
				Debug.Log("Setting true on state: " + name);
			}
		}
			
		
		/*
        int _error = -1;
        int _overrideerror = -1;
        if( _currentState != -1)
        {
            for (int i = 0; i < _currentState; ++i)
            {
                if (!exerciseStates[i].Value)
                {
                    if (!help && exerciseStates[i].Condition == (int)CheckCon.OnlyCheckWithHelp)
                    {
                        _error = -1;
                    }
                    else if (exerciseStates[_currentState].Condition == (int)CheckCon.ShowVideo && exerciseStates[i].Condition == (int)CheckCon.VideoError)
                    {
                        _error = i;
                        currentVideo = exerciseStates[i].Video;
                        currentError = exerciseStates[i].Error;
                        currentCritical = true;
                    }
                    else if (exerciseStates[i].Condition == (int)CheckCon.VideoError)
                    {
                        _error = -1;
                        _overrideerror = 1;
                    }
                    else if (exerciseStates[i].Condition == (int)CheckCon.Critical )
                    {
                        _error = i;
                        currentVideo = exerciseStates[i].Video;
                        currentError = exerciseStates[i].Error;
                        currentCritical = true;
                    }
                    else
                    {
                        _error = i;
                        currentError = exerciseStates[i].Error;
                    }

                    if (_error != -1)
                        break;
                }
            }

            if (_error == -1)
            {
                currentState = _currentState;
                if (_overrideerror == -1)
                    exerciseStates[_currentState].Value = true;
            }
        }*/
		
        return errorCode;
    }

    public void RecordState(string state)
    {
        eStateNameDebug.Add(state);
    }

    public void DebugState()
    {
        string s = "";
        for (int i = 0; i < eStateNameDebug.Count; i++)
        {
            s += eStateNameDebug[i];
            s += "\n";
        }
        Debug.Log("Debug States: \n" + s);
    }

    /// <summary>
    ///     Push a state
    /// </summary>
    /// <param name="name">Name of the state</param>
    /// <param name="svalue">The vlaue of the state (usually yes / no)</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
    public void PushState(string name, string svalue)
	{
		if(!stateName.Contains(name)) {
			stateName.Add(name);
			stateValue.Add(svalue);
			stateTime.Add(Time.realtimeSinceStartup);
		}
		else
		{
			int pos = stateName.IndexOf(name);
			if(pos != -1) {
				stateName[pos] = name;
				stateValue[pos] = svalue;
				stateTime[pos] = Time.realtimeSinceStartup;
			}
			else {
				Debug.LogError("Error! Something went terribly wrong in the state thingy");
			}
		}
	}
	
	public void PushState(string name)
	{
		PushState(name, "yes");
	}
	
	public void ClearStates()
	{
		stateName.Clear();
		stateValue.Clear();
		stateTime.Clear();
        exerciseStates.Clear();
        eStateNameDebug.Clear();
        currentState = -1;
        _help = false;
	}
	
	public void PopState(string name)
	{
		int pos = stateName.IndexOf(name);
		if(pos != -1)
		{
			stateName.RemoveAt(pos);
			stateValue.RemoveAt(pos);
			stateTime.RemoveAt(pos);
		}
	}
	
	/// <summary>
    ///    Returns the value of a state
    /// </summary>
    /// <param name="name">Name of the state</param>
    /// <returns>
    ///     value of the state
    ///     <remarks>
    ///         to get a bool value on a yes / no state, use GetStateValueB()
    ///     </remarks>
    /// </returns>
	public string GetStateValue(string name)
	{
		string rv = "";
		int pos = stateName.IndexOf(name);
		if(pos != -1)
		{
			rv = (string)stateValue[pos];
		}
		else
		{
			Debug.LogWarning("Couldn't find state with name: " + name);
		}
		return rv;
	}
	
	/// <summary>
    ///     Returns true of the value of the state is 'yes'
    /// 	Yea it's odd
    /// </summary>
    /// <param name="name">Name of the state</param>
    /// <returns>
    ///     boolean true if the value of the state is 'yes'
    ///     <remarks>
    ///         use GetStateValue() to return a string if the state contains other values than yes / no
    ///     </remarks>
    /// </returns>
	public bool GetStateValueB(string name)
	{
		bool rv = false;
		int pos = stateName.IndexOf(name);
		if(pos != -1)
		{
			if((string)stateValue[pos] == "yes")
				rv = true;
		}
		return rv;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
}