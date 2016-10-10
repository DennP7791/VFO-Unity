using System;
using UnityEngine;
using System.Collections;

public class Message : BaseWindow
{

    public delegate void CallBack(Message message, bool value);

    #region Support Classes
    /// <summary>
    ///  Type of the Message
    /// </summary>
    public enum Type
    {
        Info,
        Warning,
        Error,
    };

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

        public Texture2D this[Type type]
        {
            get
            {
                switch (type)
                {
                    case Type.Info:
                        return Info;
                    case Type.Warning:
                        return Warning;
                    case Type.Error:
                        return Error;
                    default:
                        return Info;
                }
            }
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
    private Type _type = Type.Info;
    private CallBack _callback = null;
    private bool _initizialized = false;

    private GUIStyle _style = new GUIStyle();
    private GUIStyle _iconStyle = new GUIStyle();
    private GUIStyle _textStyle = new GUIStyle();
    private GUIStyle _okButtonStyle = new GUIStyle();
    private GUIStyle _backgroundStyle = new GUIStyle();
    private Rect _textRect = new Rect();
    private Rect _iconRect = new Rect();
    private Rect _okPosition = new Rect();
    private Rect _cancelPosition = new Rect();
    private Rect _backgroundPosition = new Rect();

    private float screenWidth;
    private float screenHeight;

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }

    public Type type
    {
        get { return _type; }
        set { _type = value; }
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

        if (Button(new Rect(0, 0, Position.width, Position.height), "", GUIStyle.none))
        {
            if (_callback != null)
                _callback(this, true);
            Destroy();
        }
    }


    void Content()
    {

        if (FullScreen)
        {
            this.Position = _backgroundPosition;
            Box(new Rect(0, 0, Position.width, Position.height), "", _backgroundStyle);
            Box(Rect, "", _style);
        }
        else
        {
            this.Position = Rect;
            Box(new Rect(0, 0, Position.width, Position.height), "", _style);
        }

        if (!Editable)
        {
            Label(_textRect, _text, _textStyle);
        }
        else
        {
            _text = TextArea(_textRect, _text, 800);
        }

        Box(_iconRect, icons[_type], _iconStyle);
        if (Cancellable)
        {
            if (Button(_okPosition, Global.Instance.GetTextFromXml("message_box_ok"), _okButtonStyle))
            {
                if (_callback != null)
                    _callback(this, true);
                Destroy();
            }
            if (!OnlyOk)
                if (Button(_cancelPosition, Global.Instance.GetTextFromXml("message_box_undo"), _okButtonStyle))
                {
                    if (_callback != null)
                        _callback(this, false);
                    Destroy();
                }
        }
    }

    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            if (_initizialized)
                Initialize(_callback);
        }
    }

    public void Initialize()
    {
        Initialize(_callback);
    }

    public void Initialize(CallBack callBack)
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Rect = new Rect((Screen.width - Rect.width) * 0.5f, (Screen.height - Rect.height) * 0.5f, Rect.width, Rect.height);

        float x = 0f;
        float y = 0f;
        if (FullScreen)
        {
            _backgroundStyle.normal.background = Background;
            _backgroundPosition.x = _backgroundPosition.y = 0f;
            _backgroundPosition.width = Screen.width;
            _backgroundPosition.height = Screen.height;
            x = Rect.x = (Screen.width - Rect.width) * 0.5f;
            y = Rect.y = (Screen.height - Rect.height) * 0.5f;
        }

        _callback = callBack;

        icons.Initialize();
        _textRect.x = x;
        _textRect.y = y;
        _textRect.width = Rect.width - (Editable ? 0.0f : icons.Width);
        _textRect.height = Rect.height;

        if (Editable)
            _iconRect = new Rect(0f, 0f, 0f, 0f);
        else
        {
            _iconRect.x = x + _textRect.width;
            _iconRect.y = y;
            _iconRect.width = icons.Width;
            _iconRect.height = icons.Height;
            _iconStyle.padding = icons.padding;
        }

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

        float spacing = 5f;
        if (Cancellable)
        {
            _okButtonStyle.alignment = TextAnchor.MiddleCenter;
            _okButtonStyle.normal.background = OkButtonStyle.Normal;
            _okButtonStyle.hover.background = OkButtonStyle.Hover;
            _okButtonStyle.font = textStyle.font;
            _okButtonStyle.fontSize = 16;
            _okButtonStyle.fontStyle = FontStyle.Bold;
            _okButtonStyle.border.left = _okButtonStyle.border.right = _okButtonStyle.border.bottom = _okButtonStyle.border.top = 10;
            _okButtonStyle.normal.textColor = OkButtonStyle.TextColor;
            _okButtonStyle.hover.textColor = OkButtonStyle.TextColor;
            _okButtonStyle.active.textColor = OkButtonStyle.TextColor;
            _cancelPosition.width = _okPosition.width = OkButtonStyle.Width;
            _cancelPosition.height = _okPosition.height = OkButtonStyle.Height;
            _cancelPosition.x = (x + Rect.width) - (OkButtonStyle.Width * 2f + spacing + textStyle.padding.right);
            _cancelPosition.y = (y + Rect.height) - OkButtonStyle.Height - textStyle.padding.bottom;
            _okPosition.x = _cancelPosition.x + OkButtonStyle.Width + spacing;
            _okPosition.y = _cancelPosition.y;

            if (Editable)
            {
                _textRect.x += textStyle.padding.left;
                _textRect.y += textStyle.padding.top;
                _textRect.width -= textStyle.padding.right * 2.0f;
                _textRect.height -= ((y + Rect.height) - (_cancelPosition.y - textStyle.padding.bottom * 2.0f));
            }
        }

        if (!_initizialized)
            Visible = true;

        _initizialized = true;
    }

}
