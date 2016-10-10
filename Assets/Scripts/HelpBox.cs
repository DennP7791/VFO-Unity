using System;
using UnityEngine;
using System.Collections;

public class HelpBox : BaseWindow
{

    public delegate void CallBack();

    #region Support Classes
    [System.Serializable]
    public class ButtonStyle
    {
        public float Width = 50f;
        public float Height = 30f;
        public Texture2D Normal;
        public Texture2D Hover;
        public FontStyle TextStyle = FontStyle.Bold;
        public Color TextColor = Color.black;
    }

    /// <summary>
    ///     Class that contains the icons used in the window
    /// </summary>
    [System.Serializable]
    public class Icons
    {

        public float Width = 10f;
        public float Height = 10f;
        public RectOffset padding = new RectOffset();
        public Texture2D Info;
        public Texture2D Warning;
        public Texture2D Error;

        public void Initialize()
        {
            Color color = new Color(0f, 0f, 0f, 0f);
            Texture2D tmp = new Texture2D(1, 1);
            tmp.SetPixel(0, 0, color);
            tmp.Apply();
            Info = Info ? Info : tmp;
            Warning = Warning ? Warning : tmp;
            Error = Error ? Error : tmp;
        }

    }

    /// <summary>
    ///  Text style
    /// </summary>
    [System.Serializable]
    public class TextStyle
    {
        public Font font;
        public int fontSize = 30;
        public Color textColor = Color.white;
        public RectOffset padding = new RectOffset();
    }
    #endregion //Suport classes

    #region Inspector Variables
    public ButtonStyle OkButtonStyle = new ButtonStyle();
    public ToolBox.ScrollButton ScrollButtonStyle;
    public bool Closeable = true; //if true, the window can be closed with a click in any point
    public bool Visible = false;
    public Texture2D Background;
    public Texture2D MsgBackground; //background texture
    public Rect Rect = new Rect(0, 100, 100, 200);
    public TextStyle textStyle;
    public Icons icons;
    public bool Cancellable = false;
    public bool OnlyOk = false;
    public bool FullScreen = false;
    public bool Editable = false;
    #endregion //Inspector variables

    private string _text = "";
    private CallBack _callback = null;
    private bool _initizialized = false;

    private GUIStyle _style = new GUIStyle();
    private GUIStyle _textStyle = new GUIStyle();
    private GUIStyle _okButtonStyle = new GUIStyle();
    private GUIStyle _scrollDownButton = new GUIStyle();
    private GUIStyle _scrollUpButton = new GUIStyle();
    private Rect _scrollDownPosition = new Rect();
    private Rect _scrollUpPosition = new Rect();
    private Rect _textRect = new Rect();
    private Rect _okPosition = new Rect();
    private Rect _realRect = new Rect();
    private GUIContent _buttonContent = new GUIContent();
    private Vector2 _scrollVector = Vector2.zero;
    private float _rowHeight = 10;

    private float screenWidth;
    private float screenHeight;

    public string Text
    {
        get { return _text; }
        set 
        { 
            _text = value;
            RecalculateHeight();
        }
    }

    // Use this for initialization
    public override void WinStart()
    {
    }

    public override void WinOnGUI()
    {
        //TODO: remove visible.
        if (!Visible)
            return;

        if (!_initizialized)
            return;

        //Initialize();
        Content();

        if (!Closeable)
            return;
    }


    void Content()
    {

        this.Position = Rect;
        Box(new Rect(0, 0, Position.width, Position.height), "", _style);

        // - SCROLLVIEW BEGIN -------------------------------------
        BeginScrollView(_textRect, _scrollVector, _realRect,
                GUIStyle.none, GUIStyle.none);

        GUI.Box(new Rect(0, 0, _realRect.width, _realRect.height), _text, _textStyle);

        EndScrollView(true);


        if (_realRect.height > _textRect.height)
        {
            if (Button(_scrollUpPosition, "", _scrollUpButton))
            {
                ScrollUp();
            }
            if (Button(_scrollDownPosition, "", _scrollDownButton))
            {
                ScrollDown();
            }
        }

        if(_callback != null)
        if (Button(_okPosition, _buttonContent, _okButtonStyle))
        {
            if (_callback != null)
                _callback();
            Destroy();
        }

    }

    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            if (_initizialized)
                Initialize(_callback);
        }
    }

    public void Initialize()
    {
        Initialize(_callback);
    }

    private void RecalculateHeight()
    {
        float textHeight = _textStyle.CalcHeight(new GUIContent(_text), _textRect.width);
        _realRect = new Rect(_textRect.x, _textRect.y, _textRect.width, textHeight < _textRect.height ? _textRect.height : textHeight);
    }

    public void Initialize(CallBack callBack)
    {
        float x = 0f;
        float y = 0f;

        Rect.x = (Screen.width - 680) * 0.5f;

        //Rect = new Rect((Screen.width - 680)*0.5f, 45, 680, Rect.height);
        if (Rect.x <= 150.0f)
            Rect.x = 150.0f;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        _callback = callBack;

        if (_callback == null)
        {
            OkButtonStyle.Width = 0f;
            OkButtonStyle.Height = 0f;
        }

        icons.Initialize();
        _textRect.x = x;
        _textRect.y = y;
        _textRect.width = Rect.width - OkButtonStyle.Width;
        _textRect.height = Rect.height;

        _textStyle.padding = textStyle.padding;
        _textStyle.wordWrap = true;
        _textStyle.normal.textColor = textStyle.textColor == Color.clear ? Color.white : textStyle.textColor;
        _textStyle.fontSize = textStyle.fontSize;
        if (textStyle.font)
            _textStyle.font = textStyle.font;
        else
        {
            Debug.LogWarning(
                "The variable '_textStyle.font' of Message.cs has not been assigned in "
                + this.name + ". You probably need to Assign it in the inspector.\n" +
                "The default skin font (fixed size) will be assigned.");
        }

        float textHeight = _textStyle.CalcHeight(new GUIContent(_text), _textRect.width);
        _realRect = new Rect(_textRect.x, _textRect.y, _textRect.width, textHeight < _textRect.height ? _textRect.height : textHeight);

        _rowHeight = _textStyle.CalcHeight(new GUIContent("g"), _textRect.width);

        _scrollDownButton.normal.background = ScrollButtonStyle.downTextures.normal;
        _scrollDownButton.hover.background = ScrollButtonStyle.downTextures.hover;
        _scrollUpButton.normal.background = ScrollButtonStyle.upTextures.normal;
        _scrollUpButton.hover.background = ScrollButtonStyle.upTextures.hover;
        _scrollDownPosition.x = _textRect.x + (_textRect.width - ScrollButtonStyle.width - _textStyle.padding.right*0.5f);
        _scrollDownPosition.y = _textRect.y + _textRect.height - ScrollButtonStyle.height - _textStyle.padding.bottom;
        _scrollDownPosition.width = ScrollButtonStyle.width;
        _scrollDownPosition.height = ScrollButtonStyle.height;
        _scrollUpPosition.x = _scrollDownPosition.x;
        _scrollUpPosition.y = _textRect.y + _textStyle.padding.top;
        _scrollUpPosition.width = ScrollButtonStyle.width;
        _scrollUpPosition.height = ScrollButtonStyle.height;


        //Set background texture, it creates a black semi-transparent texture if no texture has been
        //passed from the inspector
        if (MsgBackground)
        {
            _style.normal.background = MsgBackground;
            _style.border.left = _style.border.right = _style.border.top = _style.border.bottom = 10;
        }
        else
        {
            Texture2D tmpTexture = new Texture2D(1, 1);
            tmpTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.30f));
            tmpTexture.Apply();
            _style.normal.background = tmpTexture;
        }

        _okButtonStyle.alignment = TextAnchor.LowerLeft;
        _okButtonStyle.imagePosition = ImagePosition.ImageAbove;
        _buttonContent.image = OkButtonStyle.Normal;
        _buttonContent.text = Global.Instance.GetTextFromXml("help_box_ok_button");
        _buttonContent.tooltip = "Go to the next help message.";
        _okButtonStyle.font = textStyle.font;
        _okButtonStyle.fontSize = 14;
        _okButtonStyle.fontStyle = FontStyle.Bold;
        _okButtonStyle.border.left = _okButtonStyle.border.right = _okButtonStyle.border.bottom = _okButtonStyle.border.top = 10;
        _okButtonStyle.margin.left = _okButtonStyle.margin.right = _okButtonStyle.margin.bottom = _okButtonStyle.margin.top = 0;
        _okButtonStyle.normal.textColor = OkButtonStyle.TextColor;
        _okButtonStyle.hover.textColor = OkButtonStyle.TextColor;
        _okButtonStyle.active.textColor = OkButtonStyle.TextColor;
        _okPosition.width = OkButtonStyle.Width;
        _okPosition.height = OkButtonStyle.Height;
        _okPosition.x = (x + Rect.width) - OkButtonStyle.Width;
        _okPosition.y = y + (Rect.height - OkButtonStyle.Height) / 2;

        if (!_initizialized)
            Visible = true;

        _initizialized = true;
    }

    private void ScrollDown()
    {
        if (_scrollVector.y < (_realRect.height - _textRect.height))
        {
            _scrollVector.y += _rowHeight;
        }
    }


    private void ScrollUp()
    {
        if (_scrollVector.y > _rowHeight)
        {
            _scrollVector.y -= _rowHeight;
        }
        else if (_scrollVector.y > 0)
        {
            _scrollVector.y = 0;
        }
    }

}
