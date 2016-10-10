#define ISDEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExerciseCollections;
using System;

public class LinkMenu1 : BaseWindow
{

    public enum ButtonState
    {
        Expanded = 0,
        Collapsed = 1,
    }

    /// <summary>
    ///     It gives a convenient representation for the inspector
    /// </summary>
    [System.Serializable]
    public class CollapseButtonStyle
    {
        public Texture2D CollapsedNormal;
        public Texture2D CollapsedHover;
        public Texture2D ExpandedNormal;
        public Texture2D ExpandedHover;
        public bool AutoSize = true;
        public float vPadding;
        [System.Serializable]
        
        public class Size
        {
            public float Width;
            public float Height;
        }
    }

    [System.Serializable]
    public class CollapseButton
    {
        public Rect Position;
        public Texture2D Collapsed;
        public Texture2D Expanded;
        public Texture2D CollapsedHover;
        public Texture2D ExpandedHover;
        public bool IsCollapsed = true;
        public float vPadding;

        private GUIStyle _style;
        private BaseWindow _parent;

        public CollapseButton(BaseWindow parent, Rect rect, Texture2D expandedNormal, Texture2D expandedHover, Texture2D collapsedNormal, Texture2D collapsedHover, float Padding)
        {
            this._parent = parent;
            this.IsCollapsed = true;
            this._style = new GUIStyle();
            this.Position = rect;
            this.Collapsed = collapsedNormal;
            this.Expanded = expandedNormal;
            this.CollapsedHover = collapsedHover;
            this.ExpandedHover = expandedHover;
            this.vPadding = Padding;
            Initialize();
        }

        public CollapseButton(BaseWindow parent, Rect rect, Texture2D expanded, Texture2D collapsed, float vPadding)
            :this(parent, rect, expanded, null, collapsed, null, vPadding)
        {
        }

        public CollapseButton(BaseWindow parent, Rect rect, CollapseButtonStyle style, float vPadding)
            : this(parent, rect, style.ExpandedNormal, style.ExpandedHover, style.CollapsedNormal, style.CollapsedHover, vPadding)
        {
        }

        public CollapseButton(BaseWindow parent, Rect rect, CollapseButtonStyle style)
            : this(parent, rect, style, 0f)
        {
        }

        public CollapseButton(BaseWindow parent, Rect rect, Texture2D expanded, Texture2D collapsed)
            :this(parent, rect, expanded, collapsed, 0f)
        {
        }

        public bool Draw()
        {
            SetTexture();
            if (_parent.Button(Position, "", _style))
            {
                IsCollapsed = !IsCollapsed;
                Debug.Log(IsCollapsed);
                return true;
            }
            return false;
        }

        private void Initialize()
        {
            this.Position.y += vPadding;
            //Debug.Log("vPadding: " + vPadding);
            SetTexture();
        }

        private void SetTexture()
        {
            this._style.normal.background = IsCollapsed ? Collapsed : Expanded;
            this._style.hover.background = IsCollapsed ? CollapsedHover : ExpandedHover;
        }
    }

    [System.Serializable]
    public class ScoreBoxStyle
    {
        public Texture2D ScoreEnabledTexture = null;
        public Texture2D ScoreDisabledTexture = null;
    }

    [System.Serializable]
    public class ScoreBox
    {
        public ScoreBoxStyle ScoreStyle;
        public Rect Position;
        public int MaxScore;
        public float vPadding;

        private Rect[] _scoreRects = new Rect[0];
        private int _score;
        private BaseWindow _parent;

        public int Score
        {
            set { _score = value < MaxScore ? value : MaxScore; }
            get { return _score; }
        }

        public ScoreBox(BaseWindow parent, Rect Position, ScoreBoxStyle ScoreStyle, int Score, int MaxScore, float vPadding)
        {
            this._parent = parent;
            this.ScoreStyle = ScoreStyle;
            this.Position = Position;
            this.MaxScore = MaxScore;
            this.Score = Score;
            this.vPadding = vPadding;
            _scoreRects = new Rect[MaxScore];
            Initialize();
        }

        public ScoreBox(BaseWindow parent, Rect Position, ScoreBoxStyle ScoreStyle, int Score)
            : this(parent, Position, ScoreStyle, Score, Global.Instance.MaxStars)
        {
        }

        public ScoreBox(BaseWindow parent, Rect Position, ScoreBoxStyle ScoreStyle, int Score, float vPadding)
            :this(parent, Position, ScoreStyle, Score, Global.Instance.MaxStars, vPadding)
        {
        }

        public ScoreBox(BaseWindow parent, Rect Position, ScoreBoxStyle ScoreStyle, int Score, int MaxScore)
            :this(parent, Position, ScoreStyle, Score, MaxScore, 0)
        {
        }

        public void Shift(float dist)
        {
            for (int i = 0; i < _scoreRects.Length; i++)
            {
                _scoreRects[i].y += dist;
            }
        }

        private void Initialize()
        {
            
            float totalWidth = Position.width;
            float spacing = totalWidth * 0.005f;
            float width = totalWidth * 0.195f;
            float height = Position.height <= width ? Position.height : width;
            float grossWidth = width + spacing;
            float x = Position.x+spacing;
            float y = Position.y+vPadding;

            for (int i = 0; i < MaxScore; i++)
            {
                _scoreRects[i] = new Rect(x + grossWidth * i, y -3.0f, width , height);
            }
        }

        public bool Draw()
        {
            for(int i = 0; i< MaxScore; i++)
            {
                if( i < Score)
                    _parent.DrawTexture(_scoreRects[i], ScoreStyle.ScoreEnabledTexture);
                else
                    _parent.DrawTexture(_scoreRects[i], ScoreStyle.ScoreDisabledTexture);
            }
            return true;
        }
    }

    [System.Serializable]
    public class LinkButton
    {
        public int Id;
        public int Function;
        public string Text = "";
        public List<LinkButton> subLinks = new List<LinkButton>();
        public bool isSubLink = false;
        public Texture2D Texture;
        public ScoreBoxStyle ScoreStyle;

        //TODO: set expanded to false if there are more categories
        public bool Expanded = false;
        public CollapseButtonStyle collStyle;
        public float VPadding;
        public LinkButton Parent = null;

        private Rect _rect = new Rect(0.0f, 0.0f, 50f, 50f);
        private GUIStyle _style = new GUIStyle();
        private GUIContent _content = new GUIContent();
        private bool _selected = false;
        private ScoreBox _scoreBox;
        private CollapseButton _collButton;
        public float _expandedHeight = 0.0f;
        private int _score;
        private BaseWindow _winParent;

        public BaseWindow WinParent
        {
            get { return _winParent; }
            set { _winParent = value; }
        }

        public float ExpandedHeight
        {
            get { return _expandedHeight; }
        }

        public GUIStyle Style
        {
            set { _style = value; }
        }

        public Rect rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public float x
        {
            get { return _rect.x; }
            set { _rect.x = value; }
        }

        public float y
        {
            get { return _rect.y; }
            set { _rect.y = value; }
        }

        public float width
        {
            get { return _rect.width; }
            set { _rect.width = value; }
        }

        public float height
        {
            get { return _rect.height; }
            set { _rect.height = value; }
        }

        public int Score
        {
            set 
            {
                _score = value;
                if(_scoreBox != null)
                    _scoreBox.Score = value; 
            }

            get { return _score; }
        }

        public void Shift(float dist)
        {
            _rect.y += dist;
            if (_scoreBox != null)
            {
                _scoreBox.Shift(dist);
                _collButton.Position.y += dist;
            }

            foreach (LinkButton link in subLinks)
            {
                link.Shift(dist);
            }
        }

        public void InitializeScore()
        {
            _style.fontStyle = FontStyle.Normal;
            float size = _style.fontSize;
            float totalWidth = size * 5;
            float x = _rect.x + _rect.width  -_style.padding.right - totalWidth;
            float y = _rect.y + _style.padding.top;
            //_style.fontSize/3 is a vPadding that works perfectly with the current font

            // Eventually add so the y position is calculated by font height
            float vPadding = _style.fontSize * 0.25f;

            _scoreBox = new ScoreBox(_winParent,new Rect(x, y, totalWidth, _rect.height),ScoreStyle,_score,vPadding);

            float oldX = _rect.x;
            _rect.x += size + 2 + (isSubLink ? _style.fontSize*1f : 0f);

            // Eventually add so the y position is calculated by font height
            _collButton = new CollapseButton(_winParent, new Rect(oldX, y - 4.0f, size, size), collStyle, vPadding);

            _expandedHeight = subLinks.Count * (_rect.height + VPadding) + (_rect.height + VPadding);

            GUIStyle subStyle = new GUIStyle(_style);

            for (int i = 0; i < subLinks.Count; i++)
            {
                subLinks[i].ScoreStyle = ScoreStyle;
                subLinks[i].isSubLink = true;
                subLinks[i].x = oldX;
                subLinks[i].y = i != 0 ? subLinks[i - 1].y + _rect.height + VPadding : _rect.y + _rect.height + VPadding;
                subLinks[i].width = _rect.width;
                subLinks[i].height = _rect.height;
                subLinks[i].Style = subStyle;
                subLinks[i].collStyle = collStyle;
                subLinks[i].VPadding = VPadding;
                subLinks[i].InitializeScore();
            }

            if (!isSubLink)
                _style.fontStyle = FontStyle.Bold;

            GUIContent content = new GUIContent(Text);
            float tmpWidth = _style.CalcSize(new GUIContent(Text)).x;
            _rect.width = tmpWidth + tmpWidth / Text.Length;
        }

        public bool Draw()
        {
            _content.text = Text;
            if (Texture) _content.image = Texture;

            if(_scoreBox != null)
            {
                _scoreBox.Draw();
            }

            if (_winParent.Button(_rect, _content, _style))
            {
                if (isSubLink && Function != 0)
                {
                    //hardcoded Id for search button
                    if (Id == 9999)
                    {
                        if (!GameObject.Find("SearchMenu(Clone)"))
                        {
                            Util.InstantiateResource<SearchMenu>("SearchMenu");
                        }
                        else
                        {
                            GameObject.Destroy(GameObject.Find("SearchMenu(Clone)"));
                        }

                        //_searchVideo = true;
                    }
                    else
                    {
                        SceneLoader.Instance.CurrentCategory = Parent != null ? Parent.Id : -1;
                        Debug.Log("Loading Scene: " + Function +
                            "\nCategory: " + SceneLoader.Instance.CurrentCategory +
                            "\nExercise: " + SceneLoader.Instance.CurrentScene);
                        SceneLoader.Instance.CurrentScene = Function;
                        SceneLoader.Instance.CurrentID = Id;
                    }
                }
                else
                {
                    _collButton.IsCollapsed = !_collButton.IsCollapsed;
                    Expanded = !_collButton.IsCollapsed;
                    return true;
                }
            }
            
            if (Expanded)
            {
                foreach (LinkButton link in subLinks)
                {
                    link.Draw();
                }
            }
            if (subLinks.Count > 0)
            {
                if (_collButton.Draw())
                {
                    Expanded = !_collButton.IsCollapsed;
                    return true;
                }
            }
                return false;

        }

    }

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
    public class LinkStyle
    {
        public Color selectionColor = Color.black;
        public Font font;
        public int fontSize = 18;
        public Color fontColor = Color.blue;
        public ScoreBoxStyle scoreBoxStyle;
    }
    #endregion //Support Class

    //---------------------------------------------------------------

    #region Inspector Attributes
    public Texture2D scoreTexture;
    public bool visible = true;
    public Texture2D panelBackground;
    public Texture2D collapseButton;
    public float horizontalPadding = 10.0f;
    public float verticalPadding = 10.0f;
    public int buttonPerPage = 4;
    public Rect rect = new Rect(100, 100, 200, 420);
    public ScrollButton scrollButtons;
    public LinkStyle linkStyle;
    public CollapseButtonStyle collapseButtonStyle;
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
    private Vector2 _scrollVector = Vector2.zero;
    private LinkButton[] buttons = new LinkButton[0];
    private LinkButton[] qrvMenu = new LinkButton[0];
    //private static bool _searchVideo = false; Remove if SearchMenu work as a class of it own

    private float screenHeight = Screen.height;
    private float screenWidth = Screen.width;
    

    //---------------------------------------------------------------

    #region Unity Methods
    // Use this for initialization
    public override void WinStart()
    {
        Debug.Log("start");
        Initialize2();

    }


    // Update is called once per frame
    public override void WinUpdate()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            Initialize2();
        }
    }


    public override void WinOnGUI()
    {
        if (buttons.Length == 0 || buttons.Length != Global.Instance.toolButtonArray.Length)
        {
            //Initialize2();
        }

        Position = rect;
        Box(new Rect(0, 0, Position.width, Position.height), "", _panelStyle);
        Content();
        //if (_searchVideo)
        //{
        //    Position = searchMenu;
        //    Box(new Rect(0, 0, Position.width, Position.height), "", _searchStyle);
        //    AnimateSearchMenu();
        //    SearchMenuContent();
        //}

    }

    void Content()
    {
        bool upVisible = false;
        bool downVisible = false;

        //GUI.color = new Color(1, 1, 1, 1);
        //GUI.BringWindowToFront(id);
        //GUI.FocusWindow(id);

        //check the beginning and end of the scrollable list
        if (_scrollVector.y > 0f)
        {
            upVisible = true;
        }
        if (_scrollVector.y + _rowHeight < _realRect.height - _viewRect.height)
        {
            downVisible = true;
        }

        // - SCROLLVIEW BEGIN -------------------------------------
        if (Debug.isDebugBuild)
            BeginScrollView(_viewRect, _scrollVector, _realRect);
        else
            BeginScrollView(_viewRect, _scrollVector, _realRect,
                GUIStyle.none, GUIStyle.none);

        bool expanded = false;
        float dist = 0.0f;
        if (buttons != null)
            for (int i = 0; i < buttons.Length; i++)
            {
                if (expanded)
                {
                    buttons[i].Shift(dist);
                }
                else if (buttons[i].Draw())
                {
                    expanded = true;
                    dist = buttons[i].Expanded ? buttons[i].ExpandedHeight : -buttons[i].ExpandedHeight;
                    _realRect.height += dist;
                    //if (_realRect.height < _viewRect.height)
                    //    _scrollVector = Vector3.zero;
                }
            }
        if (qrvMenu != null)
            for (int i = 0; i < qrvMenu.Length; i++)
            {
                if (expanded)
                {
                    qrvMenu[i].Shift(dist);
                }
                else if (qrvMenu[i].Draw())
                {
                    expanded = true;
                    dist = qrvMenu[i].Expanded ? qrvMenu[i].ExpandedHeight : -qrvMenu[i].ExpandedHeight;
                    _realRect.height += dist;
                    //if (_realRect.height < _viewRect.height)
                    //    _scrollVector = Vector3.zero;
                }
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

    //Initializes video-controls (buttons) and adds them to qrvMenu before adding them to the UI. 
    void InitializeQrvMenu()
    {
        List<LinkButton> qrvList = new List<LinkButton>();
        LinkButton scanButton = new LinkButton
        {
            Id = 12345,
            WinParent = this,
            Text = "Scan QR-Code",
            Style = _btnStyle,
            Function = 1001,
            isSubLink = true
        };
        scanButton.Parent = scanButton;
        LinkButton recordButton = new LinkButton
        {
            Id = 12345,
            WinParent = this,
            Text = "upload/record video",
            Style = _btnStyle,
            Function = 1001,
            isSubLink = true
        };
        LinkButton searchButton = new LinkButton
        {
            Id = 9999,
            WinParent = this,
            Text = "search for video",
            Style = _btnStyle,
            Function = 1001,
            isSubLink = true
        };
        searchButton.Parent = searchButton;
        recordButton.Parent = recordButton;
        qrvList.Add(scanButton);
        qrvList.Add(searchButton);
        qrvList.Add(recordButton);
        qrvMenu = qrvList.ToArray();
    }

    void Initialize2()
    {
        Debug.Log("Initialize");

        // Calculate height of menu
        rect.height = Screen.height - 90.0f;
        buttonPerPage = (int)((19.0f / 540.0f) * rect.height);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        //Initializes Qrv Menu
        InitializeQrvMenu();

        //Test();
        List<LinkButton> tmpList = new List<LinkButton>();
        foreach (ExerciseCategory c in Global.Instance.categoryCollection)
        {
            LinkButton tmpButton = new LinkButton
            {
                WinParent = this,
                Id = c.ID,
                Text = c.Name,
                Score = (int)c.Score,
                ScoreStyle = linkStyle.scoreBoxStyle,
            };
            foreach (Exercise e in c)
            {
                tmpButton.subLinks.Add(
                    new LinkButton
                    {
                        WinParent = this,
                        Id = e.ID,
                        Parent = tmpButton,
                        Text = e.Name,
                        Score = (int) e.Score,
                        Function = e.Function,
                        ScoreStyle = linkStyle.scoreBoxStyle,
                    }
                );
            }
            tmpList.Add(tmpButton);
        }
        buttons = tmpList.ToArray();

        Color tmpColor = new Color(0f, 0f, 0f, 0f);

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

        if(!linkStyle.scoreBoxStyle.ScoreEnabledTexture)
        {
            linkStyle.scoreBoxStyle.ScoreEnabledTexture = hoverTexture;
            Debug.LogWarning("ScoreEnabledTexture was not assigned.");
        }
        if (!linkStyle.scoreBoxStyle.ScoreDisabledTexture)
        {
            linkStyle.scoreBoxStyle.ScoreDisabledTexture = hoverTexture;
            Debug.LogWarning("ScoreDisabledTexture was not assigned.");
        }

        //Create Panel Style
        _panelStyle.normal.background = panelBackground ? panelBackground : hoverTexture;
        _panelStyle.border.left = _panelStyle.border.right = _panelStyle.border.bottom = _panelStyle.border.top = 10;        

        //Create button style
        _btnStyle.imagePosition = ImagePosition.ImageAbove;
        _btnStyle.alignment = TextAnchor.UpperLeft;
        int padding = (int)(_btnHeight * 0.05);
        _btnStyle.padding = new RectOffset(padding, padding, padding, padding);
        _btnStyle.font = linkStyle.font;
        _btnStyle.fontSize = linkStyle.fontSize;
        _btnStyle.normal.textColor = linkStyle.fontColor;
        _btnStyle.hover.textColor = linkStyle.fontColor;
        _btnStyle.active.textColor = linkStyle.fontColor;
        _btnStyle.normal.background = hoverTexture;
        _btnStyle.hover.background = hoverTexture;
        _btnStyle.hover.textColor = linkStyle.selectionColor;
        _btnStyle.active.background = hoverTexture;
        _btnStyle.border.left = _btnStyle.border.right = _btnStyle.border.bottom = _btnStyle.border.top = 10;

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
        Debug.Log("calculate real rect");
        if (buttons.Length > 0)
        {
            //Button Positioning
            float x0 = horizontalPadding;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].x = x0;
                buttons[i].y = i != 0 ? buttons[i - 1].y + _btnHeight + verticalPadding : verticalPadding;
                buttons[i].width = _btnWidth;
                buttons[i].height = _btnHeight;
                buttons[i].Style = _btnStyle;
                buttons[i].collStyle = collapseButtonStyle;
                buttons[i].VPadding = verticalPadding;
                buttons[i].InitializeScore();
            }
            //TODO: remove + 60, added to prevent a bug in a particular case
            //_realRect = new Rect(0.0f, 0.0f, rect.width, buttons[buttons.Length - 1].y + _btnHeight + 60);

            float buttonsHeight = buttons[buttons.Length - 1].y;
            for (int i = 0; i < qrvMenu.Length; i++)
            {
                qrvMenu[i].x = x0;
                qrvMenu[i].y = i != 0 ? qrvMenu[i - 1].y + _btnHeight + verticalPadding : verticalPadding + buttonsHeight + 60;
                qrvMenu[i].width = _btnWidth;
                qrvMenu[i].height = _btnHeight;
                qrvMenu[i].Style = _btnStyle;
                qrvMenu[i].VPadding = verticalPadding;
            }
            _realRect = new Rect(0.0f, 0.0f, rect.width, qrvMenu[qrvMenu.Length - 1].y + _btnHeight + 100);
        }
    }

    private void ScrollDown()
    {
        if (_scrollVector.y < ((_realRect.height - _viewRect.height) - buttons[buttons.Length - 1].height))
        {
            _scrollVector.y += _rowHeight;
        }
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

