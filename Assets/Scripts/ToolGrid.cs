using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolGrid : BaseWindow {

    [System.Serializable]
    public class CellStyle
    {
        public Font font;
        public int fontSize = 18;
        public Color fontColor = Color.blue;
        public Texture2D background;
        public Texture2D hover;
        public Texture2D checkMarkImage;
    }

    [System.Serializable]
    public class Tool
    {
        public string Function = "";
        public string Text = "";
        public Texture2D Texture;
    }

    [System.Serializable]
    public class GridCell
    {

        #region Inspector Attributes

        public bool isChecked = false;
        public Tool tool;

        #endregion // Inspector Attributes

        private Rect _rect = new Rect();
        private Rect _btnRect = new Rect();
        private bool _initialized = false;
        private GUIStyle _checkMarkStyle = new GUIStyle();
        private GUIStyle _btnStyle = new GUIStyle();
        private bool _correct = false;
        private BaseWindow _parent;

        public bool Correct
        {
            get { return _correct; }
            set { _correct = value; }
        }

        public void Initialize(BaseWindow parent, Rect rect, GUIStyle style, Texture2D checkImg)
        {
            _btnRect.x = rect.x;
            _btnRect.y = rect.y;
            _btnRect.width = rect.width;
            _btnRect.height = rect.height;
            _btnStyle = style;

            float dist = 3;
            float dim = rect.width > rect.height ? rect.height : rect.width;
            dim *= 0.25f;
            _rect.width = dim;
            _rect.height = dim;
            _rect.x = (rect.x + rect.width) - _rect.width - dist;
            _rect.y = rect.y + dist;
            _checkMarkStyle.normal.background = checkImg;
            _parent = parent;
            _initialized = true;
        }

        public void Draw()
        {
            
            if(!_initialized)
            {
                Debug.LogWarning("You cannot draw an unitialized cell.");
                return;
            }

            GUIContent content = new GUIContent();
            content.text = Text.Instance.GetString(tool.Text);
            if (tool.Texture) content.image = tool.Texture;

            if (_parent.Button(_btnRect, content, _btnStyle))
                isChecked = !isChecked;
            if (isChecked)
            {
                _parent.Box(_rect, "", _checkMarkStyle);
            }

        }
    }

	
	public Texture2D nextButton;
    public Texture2D nextButtonHover;
    public bool visible = true;
    public Rect rect = new Rect(300f, 100f, 500f, 500f);
    public int rows = 5;
    public int columns = 5;
    public float padding2 = 5f;
    public Texture2D background;
    public CellStyle cellStyle;
    public GridCell[] cells;

    private GUIStyle _btnStyle = new GUIStyle();
    private GUIStyle _panelStyle = new GUIStyle();
	private GUIStyle _nextButtonStyle = new GUIStyle();
    private float _buttonHeight = 10f;
    private float _buttonWidth = 10f;
    private Rect _buttonRect = new Rect();

    private float screenWidth;
    private float screenHeight;

	// Use this for initialization
	public override void WinStart () {

        Initialize2();
	}

    private void Initialize2()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        this.Position = new Rect(0, 0, Screen.width, Screen.height);

        _buttonWidth = (rect.width - padding2) / columns;
        _buttonHeight = (rect.height - padding2) / rows;

        Color tmpColor = new Color(0, 0, 0, 0f);

        if (!background)
        {
            Debug.LogWarning("No background texture assigned.");
            background = new Texture2D(1, 1);
            background.SetPixel(0, 0, tmpColor);
            background.Apply();
        }
        _panelStyle.normal.background = background;
        _panelStyle.border.left = _panelStyle.border.right = _panelStyle.border.top = _panelStyle.border.bottom = 10;

        //Create Texture (only used if no other texture is assigned via inspector)
        if (!cellStyle.background)
        {
            Debug.LogWarning("No cell background texture assigned.");
            cellStyle.background = background;
        }

        if (!cellStyle.checkMarkImage)
        {
            Debug.LogWarning("No check mark image assigned.");
            cellStyle.checkMarkImage = new Texture2D(1, 1);
            cellStyle.checkMarkImage.SetPixel(0, 0, Color.green);
            cellStyle.checkMarkImage.Apply();
        }

        //Create button style
        _btnStyle.imagePosition = ImagePosition.ImageAbove;
        _btnStyle.alignment = TextAnchor.LowerCenter;
        int padding = (int)(_buttonHeight * 0.1);
        _btnStyle.padding = new RectOffset(padding, padding, padding, padding);
        _btnStyle.font = cellStyle.font;
        _btnStyle.fontStyle = FontStyle.Bold;
        _btnStyle.fontSize = cellStyle.fontSize;
        _btnStyle.normal.textColor = cellStyle.fontColor;
        _btnStyle.hover.textColor = cellStyle.fontColor;
        _btnStyle.active.textColor = cellStyle.fontColor;
        _btnStyle.normal.background = cellStyle.background;
        _btnStyle.hover.background = cellStyle.hover ? cellStyle.hover : cellStyle.background;
        _btnStyle.active.background = cellStyle.hover;
        _btnStyle.border.left = _btnStyle.border.right = _btnStyle.border.bottom = _btnStyle.border.top = 10;

        if (nextButton)
        {
            _nextButtonStyle.normal.background = nextButton;
            _nextButtonStyle.alignment = TextAnchor.MiddleCenter;
            _nextButtonStyle.border.left = _nextButtonStyle.border.right = _nextButtonStyle.border.bottom = _nextButtonStyle.border.top = 10;
            _nextButtonStyle.font = cellStyle.font;
            _nextButtonStyle.fontStyle = FontStyle.Bold;
            _nextButtonStyle.fontSize = cellStyle.fontSize;
            _nextButtonStyle.normal.textColor = cellStyle.fontColor;
            _nextButtonStyle.hover.textColor = cellStyle.fontColor;
            _nextButtonStyle.active.textColor = cellStyle.fontColor;
            _nextButtonStyle.hover.background = nextButtonHover;
        }

        float newX = (Screen.width - rect.width) * 0.5f;

        for (int i = 0; i < cells.Length; i++)
        {
            Rect r = new Rect();
            r.x = (i % columns) * _buttonWidth + padding2 + newX;
            r.y = ((int)(i / columns)) * _buttonHeight + padding2 + rect.y;
            r.width = _buttonWidth - padding2;
            r.height = _buttonHeight - padding2;
            Debug.Log(r);
            cells[i].Initialize(this, r, _btnStyle, cellStyle.checkMarkImage);
        }

        _buttonRect = new Rect(newX, rect.y + rect.height + 5, +rect.width, 50);
    }

    public override void WinOnGUI()
    {
        Box(new Rect(0, 0, this.Position.width, this.Position.height), "", _panelStyle);

        foreach (var cell in cells)
        {
            cell.Draw();
        }

        if (Button(_buttonRect, Text.Instance.GetString("tool_grid_continue"), _nextButtonStyle))
        {
            if (!ValidateSelection())
            {
				string mText = Text.Instance.GetString("tool_grid_wrong_tool_test");
				if(Global.Instance.RunSimulationWithHelp)
				{
					mText = Text.Instance.GetString("tool_grid_wrong_tool_help");
				}
                Util.MessageBox(
                    new Rect(0, 0, 300, 200),
                    mText,
                    Message.Type.Error,
                    true,
                    true);
                return;
            }

            List<ToolButton> toolButtonList = new List<ToolButton>();
            foreach (var cell in cells)
            {
                if (cell.isChecked)
                {
                    ToolButton tmp = new ToolButton();
                    tmp.Function = cell.tool.Function;
                    tmp.Text = Text.Instance.GetString(cell.tool.Text);
                    tmp.Texture = cell.tool.Texture;
                    tmp.Correct = cell.Correct;
                    toolButtonList.Add(tmp);
                }
            }
            /*TODO:
             * Check toolbox content and if it meets the requirements
             * advance to the next scene.
             */


            Global.Instance.toolButtonArray = toolButtonList.ToArray();
            //SceneLoader.Instance.Scene6(); Uncommented to add sim scene callback

            //BottomBarScript.EnableToolBoxButton(true);

            States.Instance.PushState("ToolboxDone", "yes");
            GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
            if (go)
                go.SendMessage("SimCallback", "ToolboxDone"); // Sim scene callback
            else
                Debug.LogWarning("actionCallbackGameObjectName does not exist. Are you in the right scene?");

            Destroy();
        }

    }

	// Update is called once per frame
	public override void WinUpdate () 
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            Initialize2();
        }
	}

    bool ValidateSelection()
    {
        bool valid = true;
        foreach(GridCell cell in cells)
        {
            if (cell.Correct)
                valid &= cell.isChecked;
        }
        return valid;
    }

    public void SetToolCorrectness(string toolName, bool correct)
    {
        foreach (GridCell cell in cells)
        {
            if (cell.tool.Text.Equals(toolName))
            {
                cell.Correct = correct;
            }
        }
    }
}
