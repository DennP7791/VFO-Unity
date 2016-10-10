using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchMenu : BaseWindow {

    private Rect searchMenu = new Rect(0f, 45, 1148, 524);
    private SearchField searchField;
    private GUIStyle _menuStyle = new GUIStyle();
    private GUIStyle _searchFieldStyle = new GUIStyle();
    private GUIStyle _toggleBtnStyle = new GUIStyle();
    private ToggleButton[] toggles = new ToggleButton[0];

    public Texture2D background;
    public Texture2D searchFieldBackground;
    public Texture2D toggleNormal;
    public Texture2D toggleHover;
    public Texture2D toggleActive;
    public Font font;

    public override void WinStart()
    {
        Initialize();
    }

    public override void WinOnGUI()
    {
        Position = searchMenu;
        Box(new Rect(0, 0, Position.width, Position.height), "", _menuStyle);
        AnimateSearchMenu();
        Content();
    }

    public override void WinUpdate()
    {
        
    }

    [System.Serializable]
    public class SearchField
    {
        public string Text = "";
        public Texture2D Texture;

        private Rect _rect = new Rect(10f, 10f, 300f, 25f);
        private int _maxLenght = 50;
        private GUIContent _content = new GUIContent();
        private GUIStyle _style = new GUIStyle();
        private BaseWindow _winParent;

        public BaseWindow WinParent
        {
            get { return _winParent; }
            set { _winParent = value; }
        }

        public int maxLenght
        {
            get { return _maxLenght; }
            set { _maxLenght = value; }
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
    }

    [System.Serializable]
    public class ToggleButton
    {
        private int _id;
        private string _category;
        private bool _enabled = false;
        private Rect _rect;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
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
    }

    void AnimateSearchMenu()
    {
        Rect targetRect = new Rect(50, 95, 1048, 420);
        if (searchMenu.x < targetRect.x)
        {
            searchMenu.x++;
            searchMenu.width--;
        }
        if (searchMenu.y < targetRect.y)
        {
            searchMenu.y++;
            searchMenu.height--;
        }
        if (searchMenu.height > targetRect.height)
        {
            searchMenu.height--;
        }
        if (searchMenu.width > targetRect.width)
        {
            searchMenu.width--;
        }
    }

    void Content()
    {
        //Draw the search field
        searchField.x = searchMenu.x + 10f;
        searchField.y = searchMenu.y + 10f;
        searchField.Text = GUI.TextField(new Rect(searchField.x ,searchField.y, searchField.width, searchField.height), searchField.Text, searchField.maxLenght, _searchFieldStyle);

        //Draw the search button relative to the searchfield
        if (GUI.Button(new Rect(searchField.x + searchField.width + 10, searchField.y, 25, 25), "Søg", _searchFieldStyle))
        {
            //TODO: replace with search logic!
            Debug.Log(searchField.Text);
        }

        //Draw the toggle buttons relative to the searchfield, and eachother
        Rect tr = new Rect(searchField.x, searchField.y + searchField.height + 10, searchField.height, searchField.height);
        for (int i = 0; i < toggles.Length; i++)
        {
            //TODO: remove blank spaces before string, and figure out solution with style instead!
            toggles[i].Enabled = GUI.Toggle(new Rect(tr.x, tr.y + i * (tr.height + 10), tr.width, tr.height), toggles[i].Enabled, "      " + toggles[i].Category, _toggleBtnStyle);
        }
    }

    void Initialize()
    {
        searchField = new SearchField()
        {
            Text = "test",
            WinParent = this,
        };

        List<ToggleButton> tmpList = new List<ToggleButton>();
        foreach (VideoCategory vc in Global.Instance.videoCategories)
        {
            Debug.Log("LOOK AT ME!" + vc.Name);
            var tmpToggle = new ToggleButton
            {
                Id = vc.Id,
                Category = vc.Name
            };
            tmpList.Add(tmpToggle);
        }
        toggles = tmpList.ToArray();

        Color tmpColor = new Color(0f, 0f, 0f, 0f);
        Texture2D hoverTexture = new Texture2D(1, 1);
        hoverTexture.SetPixel(0, 0, tmpColor);
        hoverTexture.Apply();

        //Create Search Style
        _menuStyle.normal.background = background ? background : hoverTexture;
        _menuStyle.border.left = _menuStyle.border.right = _menuStyle.border.bottom = _menuStyle.border.top = 10;

        //Create SearchField Style
        _searchFieldStyle.normal.background = searchFieldBackground ? searchFieldBackground : hoverTexture;
        _searchFieldStyle.border.left = _searchFieldStyle.border.right = _searchFieldStyle.border.bottom = _searchFieldStyle.border.top = 10;
        _searchFieldStyle.normal.textColor = Color.white;
        _searchFieldStyle.fontSize = 16;
        _searchFieldStyle.font = font;
        _searchFieldStyle.alignment = TextAnchor.MiddleLeft;

        //Create ToggleButton Style
        _toggleBtnStyle.normal.background = toggleNormal;
        _toggleBtnStyle.hover.background = toggleHover;
        _toggleBtnStyle.active.background = toggleHover;
        _toggleBtnStyle.onNormal.background = toggleActive;
        _toggleBtnStyle.onHover.background = toggleActive;
        _toggleBtnStyle.onActive.background = toggleActive;
        _toggleBtnStyle.alignment = TextAnchor.MiddleLeft;
    }
}
