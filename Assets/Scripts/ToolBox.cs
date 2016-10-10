using UnityEngine;
using System.Collections;

//ToolBox

public class ToolBox : BaseWindow
{
    #region Support Class
    [System.Serializable]
    public class ScrollButton
    {
        public float width = 30f;
        public float height = 30f;
        public ScrollButtonTextures upTextures;
        public ScrollButtonTextures downTextures;

        [System.Serializable]
        public class ScrollButtonTextures
        {
            public Texture2D normal;
            public Texture2D hover;
            public Texture2D active;
        }
    }


    [System.Serializable]
    public class ButtonStyle
    {
        public Texture2D textureNormal;
        public Texture2D textureHover;
        public Color selectionColor = Color.black;
        public Font font;
        public int fontSize = 18;
        public Color fontColor = Color.blue;
    }
    #endregion //Support Class

    //---------------------------------------------------------------

    #region Inspector Attributes
    public int depth = 0;
    public bool visible = true;
    public Texture2D panelBackground;
    public float horizontalPadding = 10.0f;
    public float verticalPadding = 10.0f;
    public int buttonPerPage = 4;
    public Rect rect = new Rect(100, 100, 200, 420);
    public Color color = new Color(1f, 1f, 1f, 0.15f);
    public ScrollButton scrollButtons;
    public ButtonStyle buttonStyle;
    #endregion //Inspector Attributes

    private float _rowHeight = 0.0f;
    private float _btnWidth  = 1.0f;
    private float _btnHeight = 1.0f;
    private Rect _realRect = new Rect();
    private Rect _viewRect = new Rect();
    private Rect _downRect, _upRect;
    private GUIStyle _downScroll, _upScroll;
    private GUIStyle _panelStyle = new GUIStyle();
    private GUIStyle _btnStyle = new GUIStyle();
    private GUIStyle _selBtnStyle = new GUIStyle();
    private Vector2 _scrollVector = Vector2.zero;
    private ToolButton[] buttons = new ToolButton[0];
    private int _idx = -1;

    private float screenWidth;
    private float screenHeight;

    //---------------------------------------------------------------

    #region Unity Methods
    // Use this for initialization
    public override void WinStart()
    {
        _idx = -1;
        Initialize();
    }


    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize();
        }
    }


    public override void WinOnGUI()
    {
        if (buttons.Length == 0 || buttons.Length != Global.Instance.toolButtonArray.Length)
        {
            Initialize();
        }
        if (visible)
        {
            Content();
        }
    }


    void Content()
    {
        Box(new Rect(0, 0, Position.width, Position.height), "", _panelStyle);

        bool upVisible = false;
        bool downVisible = false;

        //check the beginning and end of the scrollable list
        if (_scrollVector.y > 0f)
        {
            upVisible = true;
        }
        if (_scrollVector.y < _realRect.height - _viewRect.height)
        {
            downVisible = true;
        }

        // - SCROLLVIEW BEGIN -------------------------------------
        BeginScrollView(_viewRect, _scrollVector, _realRect,
           GUIStyle.none, GUIStyle.none);

        // ----> Buttons Code
        if (buttons != null)
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Draw())
                    _idx = i;
                if (i != _idx && buttons[i].Selected)
                    buttons[i].Selected = false;
            }

        EndScrollView();
        // - SCROLLVIEW END ---------------------------------------

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (upVisible)
            if (Button(_upRect, "", _upScroll) || scrollDelta > 0)
            {
                ScrollUp();
            }

        if (downVisible)
            if (Button(_downRect, "", _downScroll) || scrollDelta < 0)
            {
                ScrollDown();
            }
    }
    #endregion //Unity Methods

    #region NonUnity Methods
    void Initialize()
    {
        // Calculate height of menu
        rect.height = Screen.height - 90.0f;
        buttonPerPage = Mathf.CeilToInt((4.0f / 540.0f) * rect.height);
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        this.Position = rect;

        buttons = Global.Instance.toolButtonArray;

        Color tmpColor = new Color(0, 0, 0, 0.15f);

        //Viewable Area Rect
        float vPadding = scrollButtons.height + scrollButtons.height * 0.25f;
        _viewRect = new Rect(0.0f, vPadding, rect.width, rect.height - vPadding * 2);

        _btnWidth = _viewRect.width - horizontalPadding*2;
        _btnHeight = _viewRect.height / buttonPerPage - verticalPadding;
        _rowHeight = _btnHeight + verticalPadding;

        //Create Hover Texture (only used if no other texture is assigned via inspector)
        Texture2D hoverTexture = new Texture2D(1, 1);
        hoverTexture.SetPixel(0, 0, tmpColor);
        hoverTexture.Apply();

        //Create Panel Style
        _panelStyle.normal.background = panelBackground ? panelBackground : hoverTexture;
        _panelStyle.border.left = _panelStyle.border.right = _panelStyle.border.bottom = _panelStyle.border.top = 10;

        //Create button style
        _btnStyle.imagePosition = ImagePosition.ImageAbove;
        _btnStyle.alignment = TextAnchor.UpperCenter;
        int padding = (int)(_btnHeight * 0.1);
        _btnStyle.padding = new RectOffset(padding, padding, padding, padding);
        _btnStyle.font = buttonStyle.font;
        _btnStyle.fontStyle = FontStyle.Bold;
        _btnStyle.fontSize = buttonStyle.fontSize;
        _btnStyle.normal.textColor = buttonStyle.fontColor;
        _btnStyle.hover.textColor = buttonStyle.fontColor;
        _btnStyle.active.textColor = buttonStyle.fontColor;
        _btnStyle.normal.background = buttonStyle.textureNormal;
        _btnStyle.hover.background = buttonStyle.textureHover;
        _btnStyle.active.background = buttonStyle.textureHover;
        _btnStyle.border.left = _btnStyle.border.right = _btnStyle.border.bottom = _btnStyle.border.top = 10;

        //Create button style
        _selBtnStyle.imagePosition = ImagePosition.ImageAbove;
        _selBtnStyle.alignment = TextAnchor.UpperCenter;
        _selBtnStyle.padding = new RectOffset(padding, padding, padding, padding);
        _selBtnStyle.font = buttonStyle.font;
        _selBtnStyle.fontStyle = FontStyle.Bold;
        _selBtnStyle.fontSize = buttonStyle.fontSize;
        _selBtnStyle.normal.textColor = buttonStyle.fontColor;
        _selBtnStyle.hover.textColor = buttonStyle.fontColor;
        _selBtnStyle.active.textColor = buttonStyle.fontColor;
        _selBtnStyle.normal.background = buttonStyle.textureHover;
        _selBtnStyle.hover.background = buttonStyle.textureHover;
        _selBtnStyle.active.background = buttonStyle.textureHover;
        _selBtnStyle.border.left = _selBtnStyle.border.right = _selBtnStyle.border.bottom = _selBtnStyle.border.top = 10;

        //Scroll buttons
        _downScroll = new GUIStyle();
        if (scrollButtons.downTextures.normal)
            _downScroll.normal.background = scrollButtons.downTextures.normal;
        if (scrollButtons.downTextures.hover)
            _downScroll.hover.background = scrollButtons.downTextures.hover;
        if (scrollButtons.downTextures.active)
            _downScroll.active.background = scrollButtons.downTextures.active;
        _downRect = new Rect((rect.width - scrollButtons.width) / 2, 
            rect.height - scrollButtons.height, scrollButtons.width, scrollButtons.height);

        _upScroll = new GUIStyle();
        if (scrollButtons.upTextures.normal)
            _upScroll.normal.background = scrollButtons.upTextures.normal;
        if (scrollButtons.upTextures.hover)
            _upScroll.hover.background = scrollButtons.upTextures.hover;
        if (scrollButtons.upTextures.active)
            _upScroll.active.background = scrollButtons.upTextures.active;
        _upRect = new Rect((rect.width - scrollButtons.width) / 2, 0, scrollButtons.width, scrollButtons.height);

        CalculateRealRect();
    }


    void CalculateRealRect()
    {
        if (buttons.Length > 0)
        {
            //Button Positioning
            float x0 = 10f;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].x = x0;
                buttons[i].y = i != 0 ? buttons[i - 1].y + _btnHeight + verticalPadding : verticalPadding;
                buttons[i].width = _btnWidth;
                buttons[i].height = _btnHeight;
                buttons[i].Style = _btnStyle;
                buttons[i].SelectedStyle = _selBtnStyle;
            }
            _realRect = new Rect(0.0f, 0.0f, rect.width, buttons[buttons.Length - 1].y + _btnHeight);
        }
    }


    void SetButtons(ToolButton[] new_buttons)
    {
       buttons = new_buttons;
       Initialize();
    }


    private void ScrollDown()
    {
        if (_scrollVector.y < ((_realRect.height - _viewRect.height) - buttons[buttons.Length - 1].height))
        {
            _scrollVector.y += _rowHeight;
        }
    }

    public void EmptyToolBox()
    {
        Global.Instance.toolButtonArray = new ToolButton[0];
    }

    private void  ScrollUp()
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

    #endregion //NonUnity Methods
}

