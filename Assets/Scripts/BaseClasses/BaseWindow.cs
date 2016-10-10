using UnityEngine;
using System.Collections;

public abstract class BaseWindow : MonoBehaviour {

    public Rect Position = new Rect();

	// Use this for initialization
    private int _id = -1;
    private int _depth = -1;
    //private GUIStyle _style;
    private bool _started = false;
    private bool _inFrontOnEnable = false;
    private bool _inFrontOnClick = false;

    public int ID { get { return _id; } }
    public int Depth { get { return _depth; } set { _depth = value; } }
    public bool InFrontOnEnable { get { return _inFrontOnEnable; } set { _inFrontOnEnable = value; } }
    public bool InFrontOnClick { get { return _inFrontOnClick; } set { _inFrontOnClick = value; } }

    public abstract void WinStart();
    public abstract void WinOnGUI();
    public abstract void WinUpdate();

	void Start () {
        if(_id == -1)
            _id = WindowHandler.Register2(this);
        Debug.Log(
            "Base Start: " + this.gameObject.name +
            ", ID: "+ _id + ", Depth: "+_depth
            );

        WinStart();
        _started = true;

        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true, Screen.currentResolution.refreshRate); //MC Added 21-07-2016 - Adapt to current resolution of device
	}

    void OnGUI()
    {
        GUI.depth = _depth;
        WinOnGUI();

        //It works as a layer preventing windows below to capture the event;
        //They still get the mouse over event though
        if (GUI.Button(Position, "", GUIStyle.none) && _inFrontOnClick)
        {
            WindowHandler.BringWindowToFront(this);
        }


        ////Prevents anything below the current depth to riceive events
        //if (Event.current.type != EventType.Layout
        //    && Event.current.type != EventType.Repaint)
        //{
        //    Event.current.Use();
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        WinUpdate();
	}

    void OnEnable()
    {
        if(_inFrontOnEnable)
            WindowHandler.BringWindowToFront(this);
    }

    public void Destroy()
    {
        Debug.Log("Base Destroying: " + this.gameObject.name);
        GameObject.Destroy(this.gameObject);
    }

    public void Toggle()
    {
        Debug.Log("Base Toggling: " + this.gameObject.name);
        this.enabled = !this.enabled;
    }

    private void SetBoundaries(ref Rect position)
    {
        position.x += Position.x;
        position.y += Position.y;
        float diffWidth = (position.x + position.width) - (Position.x + Position.width);
        float diffHeight = (position.y + position.height) - (Position.y + Position.height);


        //TODO: this causes bug when the size is < 0
        //if (diffWidth > 0f)
        //{
        //    float tmp = position.width - diffWidth;
        //    if (tmp > 0f)
        //        position.width = tmp;
        //}
        //if (diffHeight > 0f)
        //{
        //    float tmp = position.height - diffHeight;
        //    if (tmp > 0f)
        //        position.height = tmp;
        //}
    }

    public bool RepeatButton(Rect position, string text, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.RepeatButton(position, text, style);
    }

    public bool Button(Rect position, string text)
    {
        SetBoundaries(ref position);
        return GUI.Button(position, text);
    }

    public bool Button(Rect position, string text, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.Button(position, text, style);
    }

    public bool Button(Rect position, GUIContent content, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.Button(position, content, style);
    }

    public bool Button(Rect position, Texture image, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.Button(position, image, style);
    }

    public void Box(Rect position, string text, GUIStyle style)
    {
        SetBoundaries(ref position);
        GUI.Box(position, text, style);
    }

    public void Box(Rect position, Texture image, GUIStyle style)
    {
        SetBoundaries(ref position);
        GUI.Box(position, image, style);
    }

    public void Label(Rect position, string text, GUIStyle style)
    {
        SetBoundaries(ref position);
        GUI.Label(position, text, style);
    }

    public void Label(Rect position, string text)
    {
        SetBoundaries(ref position);
        GUI.Label(position, text);
    }

    public bool Toggle(Rect position, bool value, string text)
    {
        SetBoundaries(ref position);
        return GUI.Toggle(position, value, text);
    }

    public string TextField(Rect position, string text)
    {
        SetBoundaries(ref position);
        return GUI.TextField(position, text);
    }

    public string PasswordField(Rect position, string text, char maskChar)
    {
        SetBoundaries(ref position);
        return GUI.PasswordField(position, text, maskChar);
    }

    public Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
    {
        SetBoundaries(ref position);
        return GUI.BeginScrollView(position, scrollPosition, viewRect);
    }

    public Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
    {
        SetBoundaries(ref position);
        return GUI.BeginScrollView(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
    }

    public void DrawTexture(Rect position, Texture image)
    {
        SetBoundaries(ref position);
        GUI.DrawTexture(position, image);
    }

    public void EndScrollView()
    {
        GUI.EndScrollView();
    }

    public void EndScrollView(bool handleScrollWheel)
    {
        GUI.EndScrollView(handleScrollWheel);
    }

    public string TextArea(Rect position, string text)
    {
        SetBoundaries(ref position);
        return GUI.TextArea(position, text);
    }

    public string TextArea(Rect position, string text, int maxLength)
    {
        SetBoundaries(ref position);
        return GUI.TextArea(position, text, maxLength);
    }

    public string TextArea(Rect position, string text, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.TextArea(position, text, style);
    }

    public string TextArea(Rect position, string text, int maxLength, GUIStyle style)
    {
        SetBoundaries(ref position);
        return GUI.TextArea(position, text, maxLength, style);
    }
}
