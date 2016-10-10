using UnityEngine;
using System.Collections;

public class HUDWalker : BaseWindow {



    [System.Serializable]
    public class ToolButton
    {
        public bool Correct = false;
        public bool Disabled = false;
        public string Text;
        public float LeftPadding = 10;
        public Texture2D Normal;
        public Texture2D Hover;
        public Rect Position;
    }

    
    
    public Texture2D BackGround;
    public ToolButton ButtonDistant;
    public ToolButton ButtonClose;
    public ToolButton ButtonAdjacent;

    private Rect _winPosition = new Rect();
    private Rect _position = new Rect();
    private GUIStyle _backStyle = new GUIStyle();
    private GUIStyle _buttonLeftStyle = new GUIStyle();
    private GUIStyle _buttonRightStyle = new GUIStyle();
    private GUIStyle _buttonCenterStyle = new GUIStyle();
    private Walker _walker = null;

    private float screenWidth;
    private float screenHeight;

	// Use this for initialization
	public override void WinStart () {

        Initialize();
	}

    private void Initialize()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        _buttonLeftStyle.normal.background = ButtonDistant.Normal;
        _buttonLeftStyle.hover.background = ButtonDistant.Hover;
        _buttonCenterStyle.normal.background = ButtonClose.Normal;
        _buttonCenterStyle.hover.background = ButtonClose.Hover;
        _buttonRightStyle.normal.background = ButtonAdjacent.Normal;
        _buttonRightStyle.hover.background = ButtonAdjacent.Hover;
        _backStyle.normal.background = BackGround;
        Position = new Rect(0f, 0f, Screen.width, Screen.height);

        float x = (Screen.width - (ButtonDistant.Position.width + ButtonClose.Position.width + ButtonAdjacent.Position.width)) * 0.5f;
        float y = 100;

        ButtonDistant.Position.x = x;
        ButtonDistant.Position.y = y;
        ButtonClose.Position.x = ButtonDistant.Position.x + ButtonDistant.Position.width;
        ButtonClose.Position.y = y;
        ButtonAdjacent.Position.x = ButtonClose.Position.x + ButtonClose.Position.width;
        ButtonAdjacent.Position.y = y;
    }


    public override void WinOnGUI()
    {
        Box(new Rect(0, 0, Position.width, Position.height), "", _backStyle);

        if (Button(ButtonDistant.Position, "", _buttonLeftStyle) && !ButtonDistant.Disabled)
        {
            if (ButtonDistant.Correct)
            {
                _walker = Util.InstantiateResource<Walker>("Walker");
                if (_walker)
                    _walker.SetPosition(Walker.Position.FAR);
                ButtonDistant.Correct = false;
                Destroy();
            }
            else
                IncorrectMessage();
        }
        if (Button(ButtonClose.Position, "", _buttonCenterStyle) && !ButtonClose.Disabled)
        {
            if (ButtonClose.Correct)
            {
                    _walker = Util.InstantiateResource<Walker>("Walker");
                    if (_walker)
                        _walker.SetPosition(Walker.Position.CLOSE);
                    ButtonClose.Correct = false;
                    Destroy();
            }
            else
                IncorrectMessage();
        }
        if (Button(ButtonAdjacent.Position, "", _buttonRightStyle) && !ButtonAdjacent.Disabled)
        {
            if (ButtonAdjacent.Correct)
            {
                _walker = Util.InstantiateResource<Walker>("Walker");
                if (_walker)
                    _walker.SetPosition(Walker.Position.ADJACENT);
                ButtonAdjacent.Correct = false;
                Destroy();
            }
            else
                IncorrectMessage();
        }
    }

	// Update is called once per frame
	public override void WinUpdate () {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize();
        }
	}

    void IncorrectMessage()
    {
        Util.MessageBox(new Rect(100, 50, 300, 200), Text.Instance.GetString("hud_wrong_placement"), Message.Type.Error, true, true);
    }

}
