using UnityEngine;
using System.Collections; 

public class HUD : BaseWindow {

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
    public class BoxStyle
    {
        public bool AutoSize = true;
        public Rect Position = new Rect();
        public Texture2D Texture;
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
        private bool _correct = false;
        private BaseWindow _parent = null;

        public BaseWindow Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

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
                if (_parent.Button(Position, "", _btnStyle))
                {
                    this.Selected = !_selected;
                    return true;
                }
            }
            else
            {
                _parent.Box(Position, "", _btnStyle);
            }
            _parent.Box(Position, "", _selectedStyle);
            
            return false;
        }
    }

    public string Function; // Remove this, not used anymore
    public Texture2D Background;
    public Texture2D NextButton;
    public Texture2D NextButtonHover;
    public Color NextButtonColor;
    public Font NextButtonFont;
    public BoxStyle HumanStyle = new BoxStyle();
    public BoxStyle ContourStyle = new BoxStyle();
    public BoxStyle BedStyle = new BoxStyle();
    public BoxStyle ConnectionLineStyle = new BoxStyle();
    public HUDButtonStyle HudBtnStyle = new HUDButtonStyle();
    public HUDButton[] Buttons = new HUDButton[0];

    private int _idx = -1;
    private GUIStyle _bgStyle = new GUIStyle();
    private Rect _humanPosition = new Rect();
    private GUIStyle _humanBoxStyle = new GUIStyle();
    private GUIStyle _bedBoxStyle = new GUIStyle();
    private GUIStyle _contourBoxStyle = new GUIStyle();
    private GUIStyle _connectionLineBoxStyle = new GUIStyle();
    private GUIStyle _nextButtonStyle = new GUIStyle();
    private Rect _buttonPosition = new Rect();
    private Rect _nextButtonPosition = new Rect();
    private Rect[] _tmpBtnPositions;
    private bool _firstTime = true;

    private float screenWidth;
    private float screenHeight;

	// Use this for initialization
	public override void WinStart () 
    {

        _tmpBtnPositions = new Rect[Buttons.Length];  
        Initialize2();
        for (int i = 0; i < Buttons.Length; ++i)
        {
            _tmpBtnPositions[i] = Buttons[i].Position;
            _tmpBtnPositions[i].x = Buttons[i].Position.x - ContourStyle.Position.x;

        }
    }

    private void Initialize2()
    {
        if (!_firstTime)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            Position.width = screenWidth;
            Position.height = screenHeight;
        }
        //Position.x = ((float)Screen.width * 0.5f) - (Position.width * 0.5f);

        if (!HudBtnStyle.Normal)
        {
            Debug.LogWarning("Warning: HudBtnStyle.Normal has not been assigned.");
        }
        if (!HudBtnStyle.Hover)
        {
            Debug.LogWarning("Warning: HudBtnStyle.Hover has not been assigned.");
        }
        if (!HudBtnStyle.Selected)
        {
            Debug.LogWarning("Warning: HudBtnStyle.Selected has not been assigned.");
        }
        if (!HudBtnStyle.Disabled)
        {
            Debug.LogWarning("Warning: HudBtnStyle.Disabled has not been assigned.");
        }

        foreach (var button in Buttons)
        {
            button.HudBtnStyle = HudBtnStyle;
        }

        if (Background)
        {
            _bgStyle.normal.background = Background;
            //TODO: remove the magic number 10
            _bgStyle.border.left = _bgStyle.border.right = _bgStyle.border.top = _bgStyle.border.bottom = 10;
        }
        else
        {
            Debug.LogWarning("Warning: Background has not been assigned.");
        }


        if (ContourStyle.Texture)
        {
            if (ContourStyle.AutoSize)
            {
                ContourStyle.Position.width = ContourStyle.Texture.width;
                ContourStyle.Position.height = ContourStyle.Texture.height;
            }
            ContourStyle.Position.x = Position.width * 0.5f - ContourStyle.Position.width * 0.5f;
            _contourBoxStyle.normal.background = ContourStyle.Texture;
            _contourBoxStyle.border.left = _contourBoxStyle.border.right =
                _contourBoxStyle.border.top = _contourBoxStyle.border.bottom = 10;
        }

        if (BedStyle.Texture)
        {
            if (BedStyle.AutoSize)
            {
                BedStyle.Position.width = BedStyle.Texture.width;
                BedStyle.Position.height = BedStyle.Texture.height;
            }
            BedStyle.Position.x = Position.width * 0.5f - BedStyle.Position.width * 0.5f;
            _bedBoxStyle.normal.background = BedStyle.Texture;
        }

        if (HumanStyle.Texture)
        {
            if (HumanStyle.AutoSize)
            {
                HumanStyle.Position.width = HumanStyle.Texture.width;
                HumanStyle.Position.height = HumanStyle.Texture.height;
            }
            HumanStyle.Position.x = Position.width * 0.5f - HumanStyle.Position.width * 0.5f;
            _humanBoxStyle.normal.background = HumanStyle.Texture;
        }

        if (ConnectionLineStyle.Texture)
        {
            if (ConnectionLineStyle.AutoSize)
            {
                ConnectionLineStyle.Position.width = ConnectionLineStyle.Texture.width;
                ConnectionLineStyle.Position.height = ConnectionLineStyle.Texture.height;
            }
            ConnectionLineStyle.Position.x = Position.width * 0.5f - ConnectionLineStyle.Position.width * 0.5f;
            _connectionLineBoxStyle.normal.background = ConnectionLineStyle.Texture;
        }

        if (NextButton)
        {
            _nextButtonStyle.alignment = TextAnchor.MiddleCenter;
            _nextButtonStyle.normal.background = NextButton;
            _nextButtonStyle.hover.background = NextButtonHover;
            _nextButtonStyle.font = NextButtonFont;
            _nextButtonStyle.fontSize = 20;
            _nextButtonStyle.fontStyle = FontStyle.Bold;
            _nextButtonStyle.border.left = _nextButtonStyle.border.right = _nextButtonStyle.border.bottom = _nextButtonStyle.border.top = 10;
            _nextButtonStyle.normal.textColor = NextButtonColor;
            _nextButtonStyle.hover.textColor = NextButtonColor;
            _nextButtonStyle.active.textColor = NextButtonColor;
        }
        //_buttonPosition = new Rect(Position.x + Position.width + 5, Position.y + Position.height - 50, 50, 50);
        _nextButtonPosition = new Rect(ContourStyle.Position.x, ContourStyle.Position.y + ContourStyle.Position.height + 5, ContourStyle.Position.width, 50);


        for (int i = 0; i< Buttons.Length; i++)
        {
            Buttons[i].Parent = this;
            if (!_firstTime)
            {
                Buttons[i].Position.x = _tmpBtnPositions[i].x + ContourStyle.Position.x;
                Buttons[i].Position.y = _tmpBtnPositions[i].y;
            }
            
        }
        _firstTime = false;
    }

    public override void WinOnGUI()
    {
        doFunc();
    }

    void doFunc()
    {
        Box(new Rect(0, 0, Screen.width, Screen.height), "", _bgStyle);
        Box(ContourStyle.Position, "", _contourBoxStyle);
        Box(BedStyle.Position, "", _bedBoxStyle);
        Box(ConnectionLineStyle.Position, "", _connectionLineBoxStyle);
        bool correctExist = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Buttons[i].Draw())
            {
                if (Buttons[i].Selected)
                {
                    _idx = i;
                    Debug.Log("HUD Selected Position idx: " + _idx);
                }
                else
                    _idx = -1;
            }
            if (i != _idx)
            {
                Buttons[i].Selected = false;
            }
            correctExist|=Buttons[i].Correct;
        }
        Box(HumanStyle.Position, "", _humanBoxStyle);

        if (Button(_nextButtonPosition, "OK", _nextButtonStyle))
        {
            if (_idx >= 0 && _idx < Buttons.Length)
            {
                if (Buttons[_idx].Correct && Function.Length > 0)
                {
                    //Buttons[_idx].Disabled = true;
                    Buttons[_idx].Selected = false;
                    //Buttons[_idx].Correct = false;
                    
                    GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", this.name + "#" + _idx);
                    
                    //Global.Instance.SendMessage(Function, _idx);
                    this.enabled = false;
                    _idx = -1;
                }
                else
                    Util.MessageBox(new Rect(Screen.width / 2 - 200, 100, 400, 250), Text.Instance.GetString("hud_wrong_placement"), Message.Type.Error, true);
            }
            else if (!correctExist)
            {
                Debug.Log("CorrectExist: " + correctExist);
                this.enabled = false;
            }
            else
            {
                Util.MessageBox(new Rect(Screen.width / 2 - 200, 100, 400, 250), Text.Instance.GetString("hud_must_choose_placement"), Message.Type.Error, true);
            }
        }
    }
	
    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize2();
        }
	}
}
