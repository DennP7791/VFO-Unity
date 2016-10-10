using UnityEngine;
using System.Collections;

public class Remote : BaseWindow
{

    [System.Serializable]
    public class RButton
    {
        public string Callback;
        public bool Enabled = true;
        public Rect Position = new Rect();
        public Texture2D Active;
        public Texture2D Hover;
        public Texture2D Normal;
        public Texture2D Disabled;

        private GUIStyle _style = new GUIStyle();
        public GUIStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }
    }

    [System.Serializable]
    public class WireStruct
    {
        public Texture2D Texture;
        public RectOffset Offset = new RectOffset();
        public Rect Position { set; get; }
    }

    //the object that will receive the messages from the remote
    public string Receiver = ""; 
    public Texture2D Background;
    public WireStruct Wire;
    public RButton[] Buttons = new RButton[0];

    private GameObject _receiver;
    private GUIStyle _style = new GUIStyle();
    private GUIStyle _wireStyle = new GUIStyle();
    private bool _validReceiver = false;

    private float screenWidth;
    private float screenHeight;

    // Use this for initialization
    public override void WinStart()
    {
        Initialize();
    }

    private void Initialize()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        Position.x = Screen.width - 230.0f;

        if (_receiver = GameObject.Find(Receiver))
        {
            _validReceiver = true;
        }
        else
        {
            Debug.LogWarning(
                "There's no object " + Receiver + " in the scene, the remote is broken!");
        }

        if (Background)
        {
            _style.normal.background = Background;
        }
        else
        {
            Debug.LogWarning(
                "Background texture is not set, you probably forgot to set it in the inspector.");
        }

        foreach (RButton b in Buttons)
        {
            if (b.Enabled)
            {
                b.Style.normal.background = b.Normal;
                b.Style.active.background = b.Active;
                b.Style.hover.background = b.Hover;
            }
            else
            {
                b.Style.normal.background = b.Disabled;
                b.Style.active.background = b.Disabled;
                b.Style.hover.background = b.Disabled;
            }

        }

        if (Wire.Texture)
        {
            Rect tmp = new Rect();
            tmp.width = Wire.Texture.width;
            tmp.height = Wire.Texture.height;
            tmp.x = (Position.x + Position.width * 0.5f) - Wire.Offset.right + Wire.Offset.left;
            tmp.y = (Position.y + Position.height) - Wire.Offset.bottom + Wire.Offset.top;
            Wire.Position = tmp;
        }
        _wireStyle.normal.background = Wire.Texture;
    }

    public override void WinOnGUI()
    {
            GUI.Box(Wire.Position, "", _wireStyle);
            Content();
    }



    void Content()
    {
        Box(new Rect(0, 0, Position.width, Position.height), "", _style);

        foreach (RButton b in Buttons)
        {
            if (Button(b.Position, "", b.Style) && b.Enabled)
            {
                if (_validReceiver)
                {
                    Debug.Log("Sending message " + b.Callback + " to " + Receiver);
                    _receiver.SendMessage(b.Callback);
                }
                else
                {
                    Debug.LogWarning("Impossible to send message " + b.Callback + " there's no valid receiver in the scene");
                }
            }
        }
    }

    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize();
        }
    }

    public void EnableButton(string callback)
    {
        foreach (RButton b in Buttons)
        {
            if (b.Callback.Equals(callback))
            {
                b.Enabled = true;
                b.Style.normal.background = b.Normal;
                b.Style.active.background = b.Active;
                b.Style.hover.background = b.Hover;
            }
        }
    }

    public void DisableButton(string callback)
    {
        foreach (RButton b in Buttons)
        {
            if (b.Callback.Equals(callback))
            {
                b.Enabled = false;
                b.Style.normal.background = b.Disabled;
                b.Style.active.background = b.Disabled;
                b.Style.hover.background = b.Disabled;
            }
        }
    }

}
