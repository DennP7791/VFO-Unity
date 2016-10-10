using UnityEngine;
using System.Collections;

public class HUDBed : MonoBehaviour {

    [System.Serializable]
    public class NextButtonStyle
    {
        [System.Serializable]
        public class Dimension
        {
            public float width;
            public float height;
        }
        public Dimension Size;
        public Texture2D TextureNormal;
        public Texture2D TextureHover;
        public Texture2D TextureClicked;
    }

    [System.Serializable]
    public class HUDButtonStyle
    {
        public Texture2D Normal;
        public Texture2D Hover;
        public Texture2D Selected;
        public Texture2D Disabled;

        public HUDButtonStyle() :this(null, null, null)
        {
        }

        public HUDButtonStyle(Texture2D normal, Texture2D hover, Texture2D selected)
        {
            this.Normal = normal;
            this.Hover = hover;
            this.Selected = selected;
        }
    }

    [System.Serializable]
    public class HUDButton
    {
        public Rect Position;

        public string Function;

        private HUDButtonStyle _hbStyle = new HUDButtonStyle();
        private GUIStyle _btnStyle = new GUIStyle();
        private GUIStyle _selectedStyle = new GUIStyle();
        private bool _disabled = false;
        private bool _selected = false;
        private bool _correct = true;

        public HUDButtonStyle HudBtnStyle
        {
            set { _hbStyle = value; SetDefaultStyle(); }
            get { return _hbStyle; }
        }

        public bool Correct
        {
            set { _correct = value; }
            get { return _correct; }
        }

        public bool Disabled
        {
            set { _disabled = value; SetDefaultStyle(); }
            get { return _disabled; }
        }

        public bool Selected
        {
            set { _selected = value; SetDefaultStyle(); }
            get { return _selected; }
        }

        private void SetDefaultStyle()
        {
            //TODO: remove the magic number 5
            if(_btnStyle.border.left != 5)
                _btnStyle.border.left = 
                    _btnStyle.border.right =                    
                    _btnStyle.border.top =
                    _btnStyle.border.bottom = 5;
            if (_selected)
            {
                _selectedStyle.normal.background = _hbStyle.Selected;
            }
            else
            {
                _selectedStyle.normal.background = null;
            }
            if (!_disabled)
            {
                _btnStyle.normal.background = _hbStyle.Normal;
                _btnStyle.hover.background = _hbStyle.Hover;
            }
            else
            {
                _btnStyle.normal.background = _hbStyle.Disabled;
                _btnStyle.hover.background = null;
            }
        }

        public bool Draw()
        {
            if (!_disabled)
            {
                if (GUI.Button(Position, "", _btnStyle))
                {
                    this.Selected = !_selected;
                    return true;
                }
            }
            else
            {
                GUI.Box(Position, "", _btnStyle);
            }
            GUI.Box(Position, "", _selectedStyle);
            
            return false;
        }
    }

    public string Function = "";
    public Rect Position = new Rect();
    public Texture2D Background;
    public Texture2D HumanTexture;
    public HUDButtonStyle HudBtnStyle = new HUDButtonStyle();
    public HUDButton[] Buttons = new HUDButton[0];

    private int _id = -1;
    private int _idx = -1;
    private GUIStyle _style = new GUIStyle();
    private Rect _humanPosition = new Rect();
    private GUIStyle _humanBoxStyle = new GUIStyle();
    private Rect _buttonPosition = new Rect();

	// Use this for initialization
	void Start () {
        if (_id == -1)
            _id = WindowHandler.Register(this.gameObject);

        if (!HudBtnStyle.Normal)
        {
            Debug.LogWarning("Error");
        }
        if (!HudBtnStyle.Hover)
        {
            Debug.LogWarning("Error");
        }
        if (!HudBtnStyle.Selected)
        {
            Debug.LogWarning("Error");
        }
        if (!HudBtnStyle.Disabled)
        {
            Debug.LogWarning("Error");
        }

        foreach (var button in Buttons)
        {
            button.HudBtnStyle = HudBtnStyle;
        }

        if (Background)
        {
            _style.normal.background = Background;
            //TODO: remove the magic number 10
            _style.border.left = _style.border.right = _style.border.top = _style.border.bottom = 10;
        }
        else
        {
            Debug.LogWarning("Error");
        }

        if (HumanTexture)
        {
            _humanPosition.width = HumanTexture.width;
            _humanPosition.height = HumanTexture.height;
            _humanPosition.x = Position.width * 0.5f - _humanPosition.width * 0.5f;
            _humanPosition.y = Position.height * 0.5f - _humanPosition.height * 0.5f;
            _humanBoxStyle.normal.background = HumanTexture;
        }

        _buttonPosition = new Rect(Position.x + Position.width + 5, Position.y + Position.height - 50, 50, 50);
	}

    void OnGUI()
    {
        GUI.Window(_id, Position, doFunc, "", _style);
        if (GUI.Button(_buttonPosition, "OK"))
        {
            if (_idx >=0 && _idx < Buttons.Length)
                if (Buttons[_idx].Correct)
                {
                    Global.Instance.SendMessage(Function, _idx);
                    Debug.Log("ok");
                }
                else
                    Util.MessageBox(new Rect(100, 50, 800, 500), Text.Instance.GetString("hud_wrong_placement"), Message.Type.Error, true);
            else
                Util.MessageBox(new Rect(100, 50, 800, 500), Text.Instance.GetString("hud_must_choose_placement"), Message.Type.Error, true);
        }
    }

    void doFunc(int id)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Buttons[i].Draw())
            {
                _idx = i;
                Debug.Log("Selected Position idx: " + _idx);
            }
            if (i != _idx)
            {
                Buttons[i].Selected = false;
            }
        }
        GUI.Box(_humanPosition, "", _humanBoxStyle);
    }
    // Update is called once per frame
    void Update()
    {
	    
	}
}
