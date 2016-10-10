using UnityEngine;
using System.Collections;

//Static toolbox, made for demo purposes

public class StaticToolBox : MonoBehaviour {

    
    [System.Serializable]
    public class Button
    {
        public string Function = "";
        public string Text = "";
        public Texture2D Texture;
        public Rect rect = new Rect(0.0f, 0.0f, 50f, 50f);

        private GUIStyle _style = new GUIStyle();
        private GUIContent _content = new GUIContent();
        private Rect _bRect = new Rect();
        private bool _selected = false;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public void Draw()
        {
            GUI.backgroundColor = new Color(0f, 0f, 0f, _selected ? 0.3f : 0f);

            _style.imagePosition = ImagePosition.ImageAbove;
            _style.alignment = TextAnchor.UpperCenter;
            _style.font = GUI.skin.font;
            _style.fontSize = GUI.skin.label.fontSize;
            _style.normal.textColor = GUI.skin.label.normal.textColor;

            _content.text = Text;
            if(Texture) _content.image = Texture;

            _bRect.height = rect.height * 0.90f;
            _bRect.width = rect.width;
            _bRect.x = rect.x;
            _bRect.y = rect.y + (rect.height - _bRect.height) * 0.5f;

            GUI.Box(rect, "", GUI.skin.textArea);
            if (GUI.Button(_bRect, _content, _style) && Function.Length > 0)
                Global.Instance.SendMessage(Function);
        }
    }


    public bool visible = true;
    public int id = 1111;
    public int depth = 0;
    public Rect rect = new Rect(100, 100, 200, 420);
    public Color color = new Color(1f, 1f, 1f, 0.15f);
    public Font font;
    public int fontSize = 18;
    public Color fontColor = Color.blue;
    public Button[] buttons = null;

    int currSelection = -1;

	// Use this for initialization
	void Start () {

	}

    void OnGUI()
    {
        if (visible)
        {

            if(color != Color.clear)
                GUI.backgroundColor = color;
            rect = GUI.Window(id, rect, doFunc, "", GUI.skin.textField);
        }
    }

    void doFunc(int Id)
    {
        GUI.BringWindowToFront(id);
        GUI.FocusWindow(id);
        if (font)
            GUI.skin.label.font = font;
        if (fontColor != Color.clear)
            GUI.skin.label.normal.textColor = fontColor;
        GUI.skin.label.fontSize = fontSize;


        if (buttons != null)
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != currSelection)
                    buttons[i].Selected = false;
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = new Vector2(
                        Input.mousePosition.x - rect.x,
                        Screen.height - Input.mousePosition.y - rect.y);
                    if(buttons[i].rect.Contains(mousePos))
                    {
                        buttons[i].Selected = true;
                        currSelection = i;
                    }

                }
                buttons[i].Draw();
            }
    }


    // Update is called once per frame
    void Update()
    {
	    
	}
}
