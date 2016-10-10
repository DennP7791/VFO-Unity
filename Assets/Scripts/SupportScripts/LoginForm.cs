using UnityEngine;
using System.Collections;

public class LoginForm : BaseWindow {


    [System.Serializable]
    public class WindowStyle
    {
        public Texture2D Background;
        public Texture2D Logo_DK;
        public Texture2D Logo_SE;
        public Texture2D Logo;
        public Texture2D Form;
        public Font Font;
        public int FontSize;
        public Color32 FontColor;
        public float Width = 0.0f;
        public float Height = 0.0f;
    }

    [System.Serializable]
    public class ButtonStyle
    {  
        public Texture2D Normal;
        public Texture2D Hover;
        public Texture2D Active;
        public float Height = 0.0f;
    }

    [System.Serializable]
    public class TextFieldStyle 
    {
        public Rect Position;
    }

    public WindowStyle windowStyle = new WindowStyle();
    public ButtonStyle buttonStyle = new ButtonStyle();
    public TextFieldStyle userFieldStyle = new TextFieldStyle();
    public TextFieldStyle passFieldStyle = new TextFieldStyle();

    private string _userName = "";
    private string _passWord = "";
    private bool _rememberLogin = false;
    private GUIStyle _backgroundStyle = new GUIStyle();
    private GUIStyle _windowStyle = new GUIStyle();
    private GUIStyle _buttonStyle = new GUIStyle();
    private GUIStyle _logoStyle = new GUIStyle();
    private Rect _backgroundPosition = new Rect(); 
    private Rect _logoPosition = new Rect();
    private Rect _windowPosition = new Rect();
    private Rect _buttonPosition = new Rect();
    private Rect _userLabelPosition = new Rect(0f, 0f, 90f, 0f);
    private Rect _passLabelPosition = new Rect(0f, 0f, 90f, 0f);
    private float screenHeight = Screen.height;
    private float screenWidth = Screen.width;

    private Rect _userFieldPostion = new Rect();
    private Rect _passwordFieldPostion = new Rect();

    private void Initialize2()
    {
        userFieldStyle.Position = _userFieldPostion;
        passFieldStyle.Position = _passwordFieldPostion;
        Position = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
        _backgroundPosition = new Rect(0.0f, 0.0f, Screen.width, Screen.height);

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        _windowPosition = new Rect(
            (_backgroundPosition.width - windowStyle.Width) * 0.5f,
            (_backgroundPosition.height - windowStyle.Height) * 0.5f,
            windowStyle.Width, windowStyle.Height);

        //Setting position relative to the form window
        userFieldStyle.Position.x += _windowPosition.x;
        userFieldStyle.Position.y += _windowPosition.y;

        passFieldStyle.Position.x += _windowPosition.x;
        passFieldStyle.Position.y += _windowPosition.y;

        _userLabelPosition.x = userFieldStyle.Position.x;
        _userLabelPosition.y = userFieldStyle.Position.y;
        _userLabelPosition.height = userFieldStyle.Position.height;
        userFieldStyle.Position.x = _userLabelPosition.x + _userLabelPosition.width;
        userFieldStyle.Position.width -= _userLabelPosition.width;

        _logoPosition = new Rect((Screen.width - windowStyle.Logo.width) * 0.5f, (Screen.height * 0.5f) - 240.0f, windowStyle.Logo.width, windowStyle.Logo.height);

        _passLabelPosition.x = passFieldStyle.Position.x;
        _passLabelPosition.y = passFieldStyle.Position.y;
        _passLabelPosition.height = passFieldStyle.Position.height;
        passFieldStyle.Position.x = _passLabelPosition.x + _passLabelPosition.width;
        passFieldStyle.Position.width -= _passLabelPosition.width;


        _buttonPosition.x = _windowPosition.x + 10;
        _buttonPosition.y = _windowPosition.y + _windowPosition.height - 10f - buttonStyle.Height;
        _buttonPosition.width = _windowPosition.width - 20;
        _buttonPosition.height = buttonStyle.Height;

        _backgroundStyle.normal.background = windowStyle.Background;
        _logoStyle.normal.background = windowStyle.Logo;

        _windowStyle.normal.background = windowStyle.Form;
        _windowStyle.border.left = _windowStyle.border.right = _windowStyle.border.top = _windowStyle.border.bottom = 10;

        _buttonStyle.normal.background = buttonStyle.Normal;
        _buttonStyle.active.background = buttonStyle.Active;
        _buttonStyle.hover.background = buttonStyle.Hover;
        _buttonStyle.font = windowStyle.Font;
        _buttonStyle.fontSize = windowStyle.FontSize;
        _buttonStyle.fontStyle = FontStyle.Bold;
        _buttonStyle.normal.textColor = windowStyle.FontColor;
        _buttonStyle.active.textColor = windowStyle.FontColor;
        _buttonStyle.hover.textColor = windowStyle.FontColor;
        _buttonStyle.alignment = TextAnchor.MiddleCenter;
        _buttonStyle.border = new RectOffset(10, 10, 10, 10);
    }

	// Use this for initialization
	public override void  WinStart () {
        //Profiler.logFile = "mylog.log";
        //Profiler.enabled = true;

        if (Global.Instance.ProgramLanguage == "sv-SE")
            windowStyle.Logo = windowStyle.Logo_SE;
        else
            windowStyle.Logo = windowStyle.Logo_DK;

        BottomBarScript.SetNoButtons(true);

        _userFieldPostion = userFieldStyle.Position;
        _passwordFieldPostion = passFieldStyle.Position;

        //Application.ExternalCall("CheckScreenRes", "Din skærmopløsning er for lille til, at vise programmet korrekt. Luk dette vindue og tryk F11 for at maximere programmert!");

        Initialize2();

        if (PlayerPrefs.GetInt("remember") == 1)
        {
            _userName = PlayerPrefs.GetString("userName");
            _passWord = PlayerPrefs.GetString("userPass");
            _rememberLogin = true;
        }
        else if (Debug.isDebugBuild)
        {
            //_userName = "vfo@welfaredenmark.dk";
            //_passWord = "test";
        }
	}

    public override void WinOnGUI()
    {
        GUI.skin.label.font = windowStyle.Font;
        GUI.skin.label.fontSize = windowStyle.FontSize; 

        GUI.skin.textField.font = windowStyle.Font;
        GUI.skin.textField.fontSize = windowStyle.FontSize;

        Box(_backgroundPosition, "", _backgroundStyle);
        Box(_logoPosition, "", _logoStyle);
        Box(_windowPosition, "", _windowStyle);
        Label(_userLabelPosition, Text.Instance.GetString("login_form_username") + " ");
        //GUI.SetNextControlName("username");
        _userName = TextField(userFieldStyle.Position, _userName);
        Label(_passLabelPosition, Text.Instance.GetString("login_form_password") + " ");
        //GUI.SetNextControlName("password");
        Label(new Rect(_passLabelPosition.x, _passLabelPosition.y + 30, _passLabelPosition.width, _passLabelPosition.height), Text.Instance.GetString("login_form_remember") + " ");
        _passWord = PasswordField(passFieldStyle.Position, _passWord, '*');

        _rememberLogin = Toggle(new Rect(passFieldStyle.Position.x, passFieldStyle.Position.y + 30, 30, 30), _rememberLogin, "");

        if (
            Button(_buttonPosition, "Ok", _buttonStyle) 
            || (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.Return && !GameObject.Find("infomessage"))
            )
        {
            //TODO: add regulard expression here!
            if (_userName.Length < 5 || _passWord.Length < 2 || _userName.Contains("'") || _userName.Contains("\""))
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("data_manager_wrong_username_password"), Message.Type.Error, true, true);
                return;
            }

            Message msg = Util.MessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("login_form_checking_loging_data"), Message.Type.Info, false, true);
            msg.name = "infomessage";

            DataManager.JsonCredentials credentials = new DataManager.JsonCredentials(_userName, _passWord);
            StartCoroutine(DataManager.ValidateCredentials(credentials));
            //send password

            if (_rememberLogin)
            {
                PlayerPrefs.SetInt("remember", 1);
                PlayerPrefs.SetString("userName", _userName);
                PlayerPrefs.SetString("userPass", _passWord);
            }
            else
            {
                PlayerPrefs.SetInt("remember", 0);
                PlayerPrefs.SetString("userName", "");
                PlayerPrefs.SetString("userPass", "");
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
