using UnityEngine;
using System.Collections;

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
        public Rect rect = new Rect(0.0f, 0.0f, 50f, 50f);
        public float maxSize =   1.5f;
        public float animSpeed = 3.0f;
        
        private GUIStyle _style= new GUIStyle();
        private float _currSize = 1.0f;
        private Rect _currRect = new Rect();
        private bool _first = true;
        private float _xOffset = 0.0f;
        private bool _hover = false;
        private bool _changed = false;
        private bool _status = false;

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
            get { return _currRect.width / 2; }
        }

        public float Distance(Button button)
        {
            return Mathf.Abs(this.X - button.X);
        }

        public bool Draw()
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
            if (!Toggle)
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

            if (!_changed)
            if (_currRect.Contains(mousePos))
            {
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
                _hover = false;
                if (_currSize > 1.0)
                {
                    _currSize -= animSpeed * _currSize * _currSize * Time.deltaTime;
                }
                else
                    _currSize = 1.0f;
            }

            _currRect.width = rect.width*_currSize;
            _currRect.height = rect.height*_currSize;
            _currRect.y = Screen.height - rect.y -_currRect.height;
            _currRect.x = rect.x + _xOffset - _currRect.width / 2;
            _changed = false;
            if (!Toggle)
            {
                if(GUI.Button(_currRect, "", _style) && Function.Length > 0)
                    Global.Instance.SendMessage(Function);
                return true;
            }
            else
            {
                if (GUI.Button(_currRect, "", _style))
                {
                    _status = !_status;
                    if(Function.Length > 0)
                        Global.Instance.SendMessage(Function, _status);
                }
                return _status;
            }
            
        }
    }
    #endregion

    public Texture2D BottomBarTexture;
    public int Depth;
    public float ButtonMinDistance = 6.0f;
    public Button[] buttons;

    private Rect  _barRect;
    private GUIStyle _bottomBarStyle;

	// Use this for initialization
	void Start () {
        _barRect = new Rect(0.0f, 
            Screen.height - BottomBarTexture.height, 
            Screen.width, BottomBarTexture.height);
        _bottomBarStyle = new GUIStyle();
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

        _barRect.y = Screen.height - BottomBarTexture.height;
        _barRect.width = Screen.width;

        GUI.depth = Depth;
        GUI.Box(_barRect, "", _bottomBarStyle);

        int idx = -1;
        for (int i = buttons.Length-1; i>=0; i--)
        {
            if (buttons[i].Draw())
            {
                //do something.
                //Global.Instance.ToggleInfoWin();
            }
            if(buttons[i].Hover)
                idx = i;
        }

        //Trivial animation
        if(idx > -1)
        {
            float tmp = (buttons[idx].CurrSize - 1)/2;
            float size = tmp;
            for (int i = idx-1; i >= 0; i--)
            {
                if (size < 0.05f)
                    continue;
                buttons[i].CurrSize = 1 + size;
                size /= 2;
            }
            size = tmp;
            for (int i = idx+1; i < buttons.Length; i++)
            {
                if (size < 0.05f)
                    continue;
                buttons[i].CurrSize = 1 + size;
                size /= 2;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

	}
}
