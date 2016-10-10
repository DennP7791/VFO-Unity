using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Talk : BaseWindow 
{
	private static Talk _instance; //singleton

    public static Talk Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Talk)GameObject.FindObjectOfType(typeof(Talk));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Talk";
                    _instance = (Talk)container.AddComponent(typeof(Talk));
                }
            }

            return _instance;
        }	
    }

    void OnDestroy()
    {
        _instance = null;
    }
	
	public override void WinStart()
	{
		
	}
	
	public override void WinUpdate()
	{
		
	}
	
	public class TalkToPatient 
	{
		private List<string> dialogQ 	= new List<string>();
		private List<bool> ignoreQ 		= new List<bool>();
        private List<string> talkAtState = new List<string>();

		private int count 				= 0;
		
		public string GetDialogQ(int pos)
		{
  			return dialogQ[pos];
		}
		
		public bool GetIgnoreQ(int pos)
		{
			return ignoreQ[pos];
		}

        public string OnlyQuestionAfterState(int pos)
        {
            return talkAtState[pos];
        }
		
		public void SetIgnoreQ(int pos, bool v)
		{
			ignoreQ[pos] = v;
		}
		
		public int Count
		{
			get { return count; }
			set {}
		}
		
		public int CountUnignored
		{
			get 
			{
				int c = 0;
				for(int i = 0; i < ignoreQ.Count; ++i)
					if((bool)ignoreQ[i] == false) c++;
				
				return c;
			}
			set { }
		}
		
		public int IndexOfQuestion(string q)
		{
			return dialogQ.IndexOf(q);
		}
				
		public TalkToPatient() {
		}
		
		public void AddTalk(string state, string question)
		{
			dialogQ.Add(question);
			ignoreQ.Add(false);
            talkAtState.Add(state);
			count += 1;
		}		
		
		public string DebugDialogQ()
		{
			string s = "";
			for(int i = 0; i < dialogQ.Count; ++i)
			{
				s += (string)dialogQ[i] + " ";
			}
			
			return s;
		}
	}
	
	private List<TalkToPatient> talkObjects 	= new List<TalkToPatient>();
    private List<string> talkStates             = new List<string>();
	private List<string> randomQuestions 		= new List<string>();
    private List<string> recordedStates         = new List<string>();

    private int currentPosition = -1;
	private bool talkOn 		= false;

    private int queueCurrentPosition = -1;
    private int queueRealPos = -1;


	
	private GUISkin guiSkin;

    public void UpdateTalk(string state)
    {
        recordedStates.Add(state);
        int pos = talkStates.IndexOf(state);
        if (pos != -1)
        {
            AskQuestions(pos);
        }
        else if(state.Substring(0, 4) == "Talk" && state != "Talk_Undo")
        {
            int p1 = 0;
            int p2 = 0;

            try
            {
                p1 = int.Parse(state.Substring(5, 1));
                p2 = int.Parse(state.Substring(7, 1));
                IgnoreQuestion(p1, p2);
            }
            catch (System.FormatException)
            {
                Debug.LogError("Error getting talk string");
            }
        }
    }

    /// <summary>
    ///     Add talk dialog
    /// </summary>
    /// <param name="state">State that trigger the talk dialog</param>
    /// <param name="question">String containing the question</param>
    /// <param name="answer">String containing the answer</param>
    /// <returns>
    ///     position of the state
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
    public int AddTalkDialog(string state, string question)
    {
        TalkToPatient myTalk = new TalkToPatient();

        myTalk.AddTalk("", question);

        talkObjects.Add(myTalk);
        talkStates.Add(state);
        return talkObjects.Count - 1;
    }
	
	/// <summary>
    ///     Adds talk dialog to a specific talk state
    /// </summary>
    /// <param name="position">The state to add the talk dialog to</param>
    /// <param name="question">String containing the question to ask</param>
    /// <param name="answer">String containing the answer to the question</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void AddTalkDialog(string state, int position, string question)
	{
		if(talkObjects.Count > position)
		{
			talkObjects[position].AddTalk(state, question);
		}
		else
		{
			Debug.LogError("Trying to add dialog to a state that isn't defined");
		}
	}
    public void AddTalkDialog(int position, string question)
    {
        AddTalkDialog("", position, question);
    }
	
	/// <summary>
    ///     Asks questions (opens a talk dialog with buttons respesenting the questions in the state
    /// </summary>
    /// <param name="position">integer representing the state</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void AskQuestions(int position)
	{
		currentPosition = position;
		RandomizeQuestions();
		talkOn = true;
		States.Instance.PushState("TalkDialogActive" , "yes");
	}
	
	/// <summary>
    ///     Ignores a question and removes it from the dialog window
    /// </summary>
    /// <param name="state">The talk dialog state</param>
    /// <param name="pos">The number of the question</param>
    /// <returns>
    ///     none
    ///     <remarks>
    ///         none
    ///     </remarks>
    /// </returns>
	public void IgnoreQuestion(int state, int pos)
	{
		talkObjects[state].SetIgnoreQ(pos, true);
	}
	
	public void DebugTalk()
	{
		foreach(TalkToPatient t in talkObjects)
		{
			Debug.Log(t.DebugDialogQ());
        }
	}
	
	void Awake () 
	{
		guiSkin = (GUISkin)Resources.Load("VirtuelForflytningTemp");
	}
	
	private void RandomizeQuestions()
	{
		List<string> s = new List<string>();
		for(int i = 0; i < talkObjects[currentPosition].Count; ++i)
			s.Add(talkObjects[currentPosition].GetDialogQ(i));	
		
		randomQuestions.Clear();
		
		for(int i = 0; i < talkObjects[currentPosition].Count; ++i)
		{
			int rpos = Random.Range(0, s.Count);
			randomQuestions.Add((string)s[rpos]);
			s.RemoveAt(rpos);
		}
	}

    private bool AskQuestionOnlyIfState(int curretPos, int realPos)
    {
        string _onlytalkafterstate = talkObjects[curretPos].OnlyQuestionAfterState(realPos);

        if (_onlytalkafterstate != "")
        {
            if (recordedStates.Contains(_onlytalkafterstate))
                return false;
            else
                return true;
        }
        else
        {
            return false;
        }
    }
	
	void AskWindow(int windowId)
	{
		int x = 85;
		
		for(int i = 0; i < talkObjects[currentPosition].Count; ++i)
		{
			string question = (string)randomQuestions[i];
			int realPos = talkObjects[currentPosition].IndexOfQuestion(question);
			
			if((bool)talkObjects[currentPosition].GetIgnoreQ(realPos) == false && AskQuestionOnlyIfState(currentPosition, realPos) == false)
			{
				if(Button(new Rect(20, x, 460, 40), Text.Instance.GetString(question), guiSkin.GetStyle("Button")))
				{
					States.Instance.PushState("TalkDialogActive" , "no");
					talkOn = false;
					GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "Talk_" + currentPosition.ToString() + "_" + realPos.ToString());
                    queueCurrentPosition = currentPosition;
                    queueRealPos = realPos;
				}
				x+= 50;
			}
		}
		
		if(Button(new Rect(20, Screen.height - 145, 460, 40), Text.Instance.GetString("talk_dialog_undo"), guiSkin.GetStyle("Button")))
		{
			States.Instance.PushState("TalkDialogActive" , "no");
			talkOn = false;
            GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "Talk_Undo");
		}
	}
	
	public override void WinOnGUI () 
	{	
		GUIStyle st = guiSkin.GetStyle("Window");
		if(talkOn)
		{
			//int height = ((talkObjects[currentState].CountUnignored + 1) * 70) + 35;
			//GUI.Window(1, new Rect(Screen.width / 2 - 250, 30, 500, 900), AskWindow, "Vælg");
			Position = new Rect(Screen.width / 2 - 250, 30, 500, Screen.height);
			Box(new Rect(0, 0, Position.width, Position.height), "", st);
			AskWindow(1);
		}	
	}
}
