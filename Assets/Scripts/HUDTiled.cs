using UnityEngine;
using System.Collections;

public class HUDTiled : BaseWindow {



    [System.Serializable]
    public class ToolButton
    {
        public string Function = "";
        public bool Correct = false;
        public bool Disabled = false;
        public string Text;
        public float LeftPadding = 10;
        public Texture2D Normal;
        public Texture2D Hover;
        public Rect Position;
    }

    public Texture2D BackGround;
    public ToolButton[] Buttons = new ToolButton[0];

    private Rect _winPosition = new Rect();
    private Rect _position = new Rect();
    private GUIStyle _backStyle = new GUIStyle();
    private GUIStyle[] _buttonStyles = new GUIStyle[0];
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

        Position = new Rect(0f, 0f, Screen.width, Screen.height);
        float tmp = 0f;
        if (Buttons.Length > 0)
        {
            _buttonStyles = new GUIStyle[Buttons.Length];
            for (int i = 0; i < _buttonStyles.Length; i++)
            {
                _buttonStyles[i] = new GUIStyle();
                _buttonStyles[i].normal.background = Buttons[i].Normal;
                _buttonStyles[i].hover.background = Buttons[i].Hover;
                tmp += Buttons[i].Position.width;
            }
        }
        _backStyle.normal.background = BackGround;
        Position = new Rect(0f, 0f, Screen.width, Screen.height);

        float x = (Screen.width - (tmp)) * 0.5f;
        float y = 100;

        if (Buttons.Length > 0)
        {
            Buttons[0].Position.x = x;
            Buttons[0].Position.y = y;
            for (int i = 1; i < Buttons.Length; i++)
            {
                Buttons[i].Position.x = Buttons[i - 1].Position.x + Buttons[i - 1].Position.width;
                Buttons[i].Position.y = y;
            }
        }
    }

    public override void WinOnGUI()
    {
        Box(new Rect(0, 0, Position.width, Position.height), "", _backStyle);

        for (int i = 0; i < Buttons.Length; i++)
        {
            if (Button(Buttons[i].Position, "", _buttonStyles[i]) && !Buttons[i].Disabled)
            {
                if (Buttons[i].Correct)
                {
                    if (Buttons[i].Function.Length > 0)
                    {
                        Debug.Log("Calling: " + Buttons[i].Function);
                        GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", this.name + "#" + i);
                        //Global.Instance.SendMessage(Buttons[i].Function);
                    }
                    //Buttons[i].Correct = false;
                    this.enabled = false;
                }
                else
                    IncorrectMessage();
            }
        }
    }

	// Update is called once per frame
	public override void WinUpdate () 
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize();
        }
	}

    void IncorrectMessage()
    {
        Util.MessageBox(new Rect(100, 50, 300, 200), Text.Instance.GetString("hud_tiled_wrong_placement"), Message.Type.Error, true, true);
    }

}
