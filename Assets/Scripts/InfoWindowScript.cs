using UnityEngine;
using System.Collections;

public class InfoWindowScript : MonoBehaviour {

    public int Id = 10;
    public Font font;
    public int fontSize = 30;
    public Color textColor = Color.white;
    public Rect Rect = new Rect(0,100,100,200);
    public bool Visible = false;
    public string Text = "";

    //private int Id = 10;
    private GUIStyle style;
    private Vector2 scrollPosition = Vector2.zero;

    
	// Use this for initialization
	void Start () {

        style = new GUIStyle();
        style.wordWrap = true;
        style.normal.textColor = textColor == Color.clear ? Color.white : textColor;
        style.fontSize = fontSize;
        if (font)
            style.font = font;
        else
        {
            Debug.LogWarning(
                "The variable 'Font' of InfoWindowScript has not been assigned in " +this.name+". "+
                "You probably need to Assign it in the inspector.\n" + 
                "The default skin font (fixed size) will be assigned.");
        }

        Text = 
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non ornare erat. "+
            "Nullam malesuada sem luctus tellus fringilla ultricies. Vivamus id nibh est. "+
            "Suspendisse consectetur massa ac est congue vestibulum. Phasellus eu mauris diam, "+
            "eget iaculis metus. Quisque in magna id lorem facilisis rutrum. Phasellus mattis "+
            "turpis non eros consectetur auctor. Class aptent taciti sociosqu ad litora torquent "+
            "per conubia nostra, per inceptos himenaeos. Donec aliquam lacinia leo, eu lacinia "+
            "arcu convallis sed. Proin pharetra vestibulum libero a varius." +
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non ornare erat. " +
            "Nullam malesuada sem luctus tellus fringilla ultricies. Vivamus id nibh est. " +
            "Suspendisse consectetur massa ac est congue vestibulum. Phasellus eu mauris diam, " +
            "eget iaculis metus. Quisque in magna id lorem facilisis rutrum. Phasellus mattis " +
            "turpis non eros consectetur auctor. Class aptent taciti sociosqu ad litora torquent " +
            "per conubia nostra, per inceptos himenaeos. Donec aliquam lacinia leo, eu lacinia " +
            "arcu convallis sed. Proin pharetra vestibulum libero a varius."+
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non ornare erat. "+
            "Nullam malesuada sem luctus tellus fringilla ultricies. Vivamus id nibh est. "+
            "Suspendisse consectetur massa ac est congue vestibulum. Phasellus eu mauris diam, "+
            "eget iaculis metus. Quisque in magna id lorem facilisis rutrum. Phasellus mattis "+
            "turpis non eros consectetur auctor. Class aptent taciti sociosqu ad litora torquent "+
            "per conubia nostra, per inceptos himenaeos. Donec aliquam lacinia leo, eu lacinia "+
            "arcu convallis sed. Proin pharetra vestibulum libero a varius." +
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non ornare erat. " +
            "Nullam malesuada sem luctus tellus fringilla ultricies. Vivamus id nibh est. " +
            "Suspendisse consectetur massa ac est congue vestibulum. Phasellus eu mauris diam, " +
            "eget iaculis metus. Quisque in magna id lorem facilisis rutrum. Phasellus mattis " +
            "turpis non eros consectetur auctor. Class aptent taciti sociosqu ad litora torquent " +
            "per conubia nostra, per inceptos himenaeos. Donec aliquam lacinia leo, eu lacinia " +
            "arcu convallis sed. Proin pharetra vestibulum libero a varius.";
	}

    void OnGUI()
    {
        if (Visible)
        {
            //TODO: disable background button.
            //TODO: change Guistyle box with something appropriate.
            Rect = GUI.Window(Id, Rect, doFunc, "", GUI.skin.textField);
        }
    }

    
    void doFunc(int id)
    {

        GUI.BringWindowToFront(Id);
        GUI.FocusWindow(Id);
        //TODO: remove magic numbers
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, 
            GUILayout.Width(Rect.width-25), 
            GUILayout.Height(Rect.height-7));
        GUILayout.Label(Text, font?style:GUI.skin.label);
        GUILayout.EndScrollView();

    }

	// Update is called once per frame
	void Update () {
	
	}
}
