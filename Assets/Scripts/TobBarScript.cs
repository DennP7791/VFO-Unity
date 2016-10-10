using UnityEngine;
using System.Collections;

/// <summary>
///     By requirements:
///     TopBar is fixed size, always centered with respect to the horizontal axes
/// </summary>
public class TobBarScript : MonoBehaviour
{

    [System.Serializable]
    public class TextStyle
    {
        public Font Font;
        public FontStyle Style;
        public int Size;
        public Color Color;
        public RectOffset Padding;
    }

    [System.Serializable]
    public class ToolTipStyle
    {
        public Texture2D Texture;
        public Rect TexturePosition;
        public Font Font;
        public FontStyle Style;
        public int Size;
        public Color Color;
        public RectOffset Padding;
    }

    #region Inspector Parameters

    public Texture2D TopBarTexture;
    public bool Visible = true;
    public int Depth = 0;
    public string Text = "";
    public TextStyle textStyle;
    public ToolTipStyle toolTipStyle;

    #endregion

    //--------------------------------------------------------------------

    private GUIStyle _style = new GUIStyle();
    private GUIStyle _titleStyle = new GUIStyle();
    private GUIStyle _toolTipStyle = new GUIStyle();
    private GUIStyle _iconStyle = new GUIStyle();
    private Rect _topBarRect;
    private Rect _titleRect;
    private Rect _toolTipRect;
    private Rect _iconRect;
    private float _textureCenter;
    private string _toolTipText = "";
    private float screenHeight = Screen.height;
    private float screenWidth = Screen.width;

    public string ToolTipText
    {
        set { _toolTipText = value; }
        get { return _toolTipText; }
    }

    void OnGUI()
    {
        if (Visible && TopBarTexture)
        {
            _topBarRect.x = Screen.width / 2 - _textureCenter;
            _topBarRect.y = 0;

            if (Global.Instance.ProgramLanguage == "sv-SE") //MC Added 20-07-2016
                Text = "Virtuell Förflyttning";
            else
                Text = "Virtuel Forflytning";

            string title = Text;
            ExerciseCollections.ExerciseCategory ec = Global.Instance.categoryCollection[SceneLoader.Instance.CurrentCategory];
            if (ec != null)
            {
                ExerciseCollections.Exercise ex = ec[SceneLoader.Instance.CurrentID];
                if (ex != null)
                    title += "\n" + ex.Name;
            }

            

            GUI.depth = Depth;
            GUI.Box(_topBarRect, "", _style);
            GUI.Box(_titleRect, title, _titleStyle);
            if (_toolTipText.Length > 0)
            {
                GUI.Box(_iconRect, toolTipStyle.Texture, _iconStyle);
                GUI.TextArea(_toolTipRect, _toolTipText, _toolTipStyle);
            }
        }
    }

    private void initialize2()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        if (TopBarTexture)
        {
            _style.normal.background = TopBarTexture;
            _style.border.bottom = 10;
            _textureCenter = Screen.width / 2f;
            _topBarRect = new Rect(0f, 0f, Screen.width, 45);
            _titleRect = new Rect(_topBarRect.x, _topBarRect.y, _topBarRect.width * 0.5f, _topBarRect.height);
            _iconRect = new Rect(_titleRect.width, _topBarRect.y, toolTipStyle.TexturePosition.width, toolTipStyle.TexturePosition.height);
            _toolTipRect = new Rect(_iconRect.x + _iconRect.width, _topBarRect.y, _topBarRect.width - (_titleRect.width + _iconRect.width), _topBarRect.height);
        }
    }

	// Use this for initialization
	void Start () {
        if (TopBarTexture)
        {
            initialize2();
        }
        else
        {
            Debug.LogWarning(
                "The variable 'TopBarTexture' of "+this.name+" has not been assigned.\n" +
                "You probably need to Assign it in the inspector.");
        }

        if (textStyle.Font)
            _titleStyle.font = textStyle.Font;
        if (textStyle.Color != null)
            _titleStyle.normal.textColor = textStyle.Color;
        if (textStyle.Size != 0)
            _titleStyle.fontSize = textStyle.Size;
        if (textStyle.Style != null)
            _titleStyle.fontStyle = textStyle.Style;
        _titleStyle.padding = textStyle.Padding;
        _titleStyle.alignment = TextAnchor.UpperLeft;

        _toolTipStyle.font = toolTipStyle.Font;
        _toolTipStyle.normal.textColor = toolTipStyle.Color;
        _toolTipStyle.fontSize = toolTipStyle.Size;
        _toolTipStyle.fontStyle = toolTipStyle.Style;
        _toolTipStyle.padding = toolTipStyle.Padding;
        _toolTipStyle.alignment = TextAnchor.UpperLeft;
        _toolTipStyle.wordWrap = true;

        _iconStyle.alignment = TextAnchor.UpperCenter;
        _iconStyle.padding.top = 5;
        _iconStyle.padding.bottom = 0;
        _iconStyle.padding.left = 5;
        _iconStyle.padding.right = 5;

	}
	
	// Update is called once per frame
	void Update () {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            initialize2();
        }
	}
}
