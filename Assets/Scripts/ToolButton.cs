using UnityEngine;
using System.Collections;

[System.Serializable]
public class ToolButton
{
    public string Function = "";
    public string Text = "";
    public Texture2D Texture;

    private Rect _rect = new Rect(0.0f, 0.0f, 50f, 50f);
    private GUIStyle _style = new GUIStyle();
    private GUIStyle _selectedStyle = null;
    private GUIContent _content = new GUIContent();
    private bool _selected = false;
    private BaseWindow _parent = null;
    private bool _correct = false;
    private GameObject _targetGO = null;
    private MonoBehaviour _targetMB = null;

    public GameObject Target
    {
        get { return _targetGO; }
        set { _targetGO = value;
        if (_targetGO)
            _targetMB = _targetGO.GetComponent<MonoBehaviour>();
        }
    }

    public bool Correct
    {
        get { return _correct; }
        set { _correct = value; }
    }

    public BaseWindow Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            Click();
        }
    }

    public GUIStyle Style
    {
        set { _style = value; }
    }

    public GUIStyle SelectedStyle
    {
        set { _selectedStyle = value; }
    }

    public Rect rect
    {
        get { return _rect;  }
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

    public bool Draw()
    {
        _content.text = Text;
        if (Texture) _content.image = Texture;

        if (_targetGO == null)
            _selected = false;
        if (_targetMB)
        {
            if (!_targetMB.enabled)
                _selected = false;
        }

        GUIStyle tmpStyle = Selected && _selectedStyle != null ?_selectedStyle : _style;

        bool result = _parent == null
            ? GUI.Button(_rect, _content, tmpStyle)
            : _parent.Button(_rect, _content, tmpStyle);

        if (result)
        {
            Selected = !Selected;
        }
        return result;
    }

    private void Click()
    {
        if (_correct)
        {
            if (Function.Length > 0)
            {
                // Until we move it all into other callbacks, check if the simulation currently has stopped the user with an error
                if (!States.Instance.GetStateValueB("showingErrorMessage"))
                {
                    Debug.Log("Function:" + Function);
                    Global.Instance.SendMessage(Function, this);
                }
            }
        }
        else if(_selected)
        {
            Util.MessageBox(new Rect(0, 0, 300, 200), Global.Instance.GetTextFromXml("tool_button_not_needed_tool"), Message.Type.Error, true, true);
            //it shouldn't appear as selected
            _selected = false;
        }
    }
}
