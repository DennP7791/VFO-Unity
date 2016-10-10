using UnityEngine;
using System.Collections;

//
public class BottomBarScript : MonoBehaviour
{

    #region Support Class
    /// <summary>
    ///     Button Class
    /// </summary>
    [System.Serializable]
    public class Button
    {
        public string Function = "";
        public bool Toggle = false;
        public Texture2D normalTexture;
        public Texture2D hoverTexture ;
        public Texture2D clickTexture ;
        public Texture2D disabledTexture;
        public Rect rect = new Rect(0.0f, 0.0f, 50f, 50f);
        public float maxSize =   1.5f;
        public float animSpeed = 3.0f;
		public string hoverText = "";

        private GUIStyle _style = new GUIStyle();
        private float _currSize = 1.0f;
        private Rect _currRect = new Rect();
        private bool _first = true;
        private float _xOffset = 0.0f;
        private bool _hover = false;
        private bool _changed = false;
        private bool _status = false;
        private bool _disabled = false;

        public bool Disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        public bool Value
        {
            get { return _status; }
            set { _status = value; }
        }

        public Rect CurrRect
        {
            get { return _currRect;}
            set { _currRect = value; }
        }

        public float Offset
        {
            get { return _xOffset; }
            set { _xOffset = value;}
        }

        public float CurrSize
        {
            get { return _currSize; }
            set { _currSize = value; _changed = true; }                
        }

        public bool Hover
        {
            get { return _hover; }
        }

        public float X
        {
            get { return _currRect.x; }
        }

        public float Radius
        {
            get { return _currRect.width * 0.5f; }
        }

        public float Distance(Button button)
        {
            return Mathf.Abs(this.X - button.X);
        }

        public bool Draw(float offset)
        {
            if (_first)
            {
                _first = false;
                _currRect.width  = rect.width ;
                _currRect.height = rect.height;
                _currRect.x = rect.x;
                _currRect.y = Screen.height - rect.y;
            }

            //It sucks that the Inspector can't use Construcor and/or properties :(
            if (Disabled)
            {
                if (disabledTexture)
                    _style.normal.background = disabledTexture;
                _style.hover.background = null;
                _style.active.background = null;
            }
            else if (!Toggle)
            {
                if (normalTexture)
                    _style.normal.background = normalTexture;
                if (hoverTexture)
                    _style.hover.background  = hoverTexture;
                if (clickTexture)
                    _style.active.background = clickTexture;
            }
            else
            {
                _style.hover.background = null;
                _style.active.background = null;
                if (normalTexture && !_status)
                    _style.normal.background = normalTexture;
                else if (clickTexture)
                    _style.normal.background = clickTexture;

            }
            
            Vector2 mousePos = new 
                Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

            //if(!_disabled && _currSize > 1.0f)
            if (!_changed)
            if (_currRect.Contains(mousePos) && !_disabled)
            {
				if(!_hover)
				{
					Util.SetToolTipText(Text.Instance.GetString(hoverText));	
				}
                _hover = true;
                if (_currSize < maxSize)
                {
                    _currSize += animSpeed * _currSize * _currSize * Time.deltaTime;
                }
                else
                    _currSize = maxSize;
            }
            else
            {
				if(_hover)
				{
					Util.SetToolTipText("");
				}
                _hover = false;
                if (_currSize > 1.0f)
                {
                    _currSize -= animSpeed * _currSize * _currSize * Time.deltaTime;
                }
                else
                    _currSize = 1.0f;
            }

            // center buttons with offset
            _xOffset = Screen.width * 0.5f - offset;

            _currRect.width = rect.width*_currSize;
            _currRect.height = rect.height*_currSize;
            _currRect.y = Screen.height - rect.y -_currRect.height;
            _currRect.x = rect.x + _xOffset - _currRect.width / 2;
            _changed = false;

            if (!Toggle)
            {
                if(GUI.Button(_currRect, "", _style) && Function.Length > 0)
                    if(!_disabled)
                        Global.Instance.SendMessage(Function);
                return true;
            }
            else
            {
                if (GUI.Button(_currRect, "", _style))
                {
                    if (!_disabled)
                    {
                        _status = !_status;
                        if (Function.Length > 0)
                            Global.Instance.SendMessage(Function, _status);
                    }
                }
                return _status;
            }
            
        }
    }
    #endregion

    public Texture2D BottomBarTexture;

    public Texture2D StarActive;
    public Texture2D StartDeactive;

    public int Depth;
    public float Height = 45f;
    public float ButtonMinDistance = 6.0f;
    public Button[] buttons;

    private Rect  _barRect;
    private Vector2 _starPos;
    private GUIStyle _bottomBarStyle;
    private GUIStyle _version = new GUIStyle();
    private bool _noButtons = false;

    public bool NoButtons
    {
        set { _noButtons = value; }
        get { return _noButtons; }
    }

	// Use this for initialization
	void Start () {
        _barRect = new Rect(0.0f,
            Screen.height - Height,
            Screen.width, Height);
        _bottomBarStyle = new GUIStyle();
        _bottomBarStyle.border.top = 10;

        _starPos = new Vector2(10.0f, Screen.height - 10.0f);

        _version.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
	}

    void OnGUI()
    {
        if (BottomBarTexture)
            _bottomBarStyle.normal.background = BottomBarTexture;
        else
        {
            Debug.LogWarning(
                "The variable 'BottomBarTexture' of BottomBarScript has not been assigned." +
                "You probably need to Assign it in the inspector.");
            return;
        }

        _barRect.y = Screen.height - Height;
        _barRect.width = Screen.width;

        GUI.depth = Depth;
        GUI.Box(_barRect, "", _bottomBarStyle);

        int idx = -1;
        for (int i = buttons.Length-1; i>=0 && !_noButtons; i--)
        {
            float offset = buttons[buttons.Length - 1].rect.x * 0.5f;
            if (buttons[i].Draw(offset))
            {
                //do something.
                //Global.Instance.ToggleInfoWin();
            }
            if(buttons[i].Hover)
			{
                idx = i;
			}
        }

        if ((Global.Instance.HasHelpOrTestRun && Global.Instance.HasPreasurePointsRun) || States.Instance.GetStateValueB("DEBUG"))
        {
            int x = 0;
            for (int i = 0; i < Global.Instance.MaxStars; ++i)
            {
                GUI.DrawTexture(new Rect(10.0f + x, Screen.height - 29.0f, 16, 16), i < Results.Instance.GetScore() ? StarActive : StartDeactive);
                x += 16;
            }
        }

        GUI.Label(new Rect(Screen.width - 140, Screen.height - 26, 140, 20), "v: " + Global.Instance.ProgramVersion.ToString("0.000") + " " + Global.Instance.ProgramLanguage, _version);

        ////Trivial animation
        //if(idx > -1)
        //{
        //    float tmp = (buttons[idx].CurrSize - 1)/2;
        //    float size = tmp;
        //    for (int i = idx-1; i >= 0; i--)
        //    {
        //        if (size < 0.05f)
        //            continue;
        //        buttons[i].CurrSize = 1 + size;
        //        size /= 2;
        //    }
        //    size = tmp;
        //    for (int i = idx+1; i < buttons.Length; i++)
        //    {
        //        if (size < 0.05f)
        //            continue;
        //        buttons[i].CurrSize = 1 + size;
        //        size /= 2;
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {

	}

    public static void EnableInfoButton(Message message, bool value)
    {
        SetButtonDisabled(1, !value);
    }

    public static void EnableInfoButton(bool value)
    {
        SetButtonDisabled(1, !value);
    }


    public static void SetButtonDisabled(int idx, bool value)
    {
        BottomBarScript obj = (BottomBarScript)GameObject.FindObjectOfType(typeof(BottomBarScript));
        if (obj)
            if(idx >= 0 && idx < obj.buttons.Length)
                obj.buttons[idx].Disabled = value;
    }

    public static void EnableHomeButton(Message message, bool value)
    {
        SetButtonDisabled(0, !value);
    }

    public static void EnableHomeButton(bool value)
    {
        SetButtonDisabled(0, !value);
    }

    
    public static void EnableRefreshButton(Message message, bool value)
    {
        SetButtonDisabled(2, !value);
    }

    public static void EnableRefreshButton(bool value)
    {
        SetButtonDisabled(2, !value);
    }
     

    public static void SetNoButtons(bool value)
    {
        BottomBarScript obj = (BottomBarScript)GameObject.FindObjectOfType(typeof(BottomBarScript));
        if (obj)
            obj.NoButtons = value;
    }
}
