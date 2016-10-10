using UnityEngine;
using System.Collections;

public class SkinTest : MonoBehaviour
{

    public GUISkin virtuelForflytning;
    private Rect rctWindow1;
    private Rect rctWindow2;
    private Rect rctWindow3;
    private Rect rctWindow4;
    private bool blnToggleState = false;
    private float fltSliderValue = 0.5f;
    private float fltScrollerValue = 0.5f;
    private Vector2 scrollPosition = Vector2.zero;

    public struct snNodeArray
    {
        public string itemType, itemName;
        public snNodeArray(string itemType, string itemName)
        {
            this.itemType = itemType;
            this.itemName = itemName;
        }
    }
    private snNodeArray[] testArray = new snNodeArray[20];

    void Awake()
    {
        rctWindow1 = new Rect(0, 0, 1280, 630);
        for (int i = 0; i < 19; i++)
        {
            testArray[i].itemType = "node";
            testArray[i].itemName = "Hello" + i;
        }
    }
    void OnGUI()
    {
        GUI.skin = virtuelForflytning;
        rctWindow1 = GUI.Window(0, rctWindow1, DoMyWindow, "", GUI.skin.GetStyle("background"));
		

   		rctWindow2 = GUI.Window(1, rctWindow2, DoMyWindow2, "Vaerktoejskasse", GUI.skin.GetStyle("Toolbox"));
   //     rctWindow3 = GUI.Window(2, rctWindow3, DoMyWindow4, "Compound Control - Toggle Listbox", GUI.skin.GetStyle("window"));
    //    GUI.skin = thisAmigaGUISkin;
   //     rctWindow4 = GUI.Window(3, rctWindow4, DoMyWindow, "Amiga500", GUI.skin.GetStyle("window"));
    }

    void gcListItem(string strItemName)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(strItemName);
        blnToggleState = GUILayout.Toggle(blnToggleState, "");
        GUILayout.EndHorizontal();
    }

    void gcListBox()
    {
        GUILayout.BeginVertical(GUI.skin.GetStyle("box"));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(160), GUILayout.Height(130));
        for (int i = 0; i < 20; i++)
        {
            gcListItem("I'm listItem number " + i);
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }



    void DoMyWindow(int windowID)
    {
        GUILayout.BeginVertical();
      //  GUILayout.Label("Im a Label");
        GUILayout.Space(8);
        
      	GUILayout.Box("Dette er en overskrift", virtuelForflytning.GetStyle("background"));
		 GUILayout.Space(360);
		GUILayout.Button("Im a Button");
	//GUILayout.Box("Broedtekst eksempel broedtekst eksempel  ", virtuelForflytning.GetStyle("Main_text"));
	//	GUILayout.Button("Kapitel", virtuelForflytning.GetStyle("Chapter_button"));
		//  GUILayout.TextField("Im a textfield");
      //GUILayout.TextArea("Im a textfield\nIm the second line\nIm the third line\nIm the fourth line");
      //  blnToggleState = GUILayout.Toggle(blnToggleState, "Im a Toggle button");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        //Sliders
        GUILayout.BeginHorizontal();
      //  fltSliderValue = GUILayout.HorizontalSlider(fltSliderValue, 0.0f, 1.1f, GUILayout.Width(128));
     //   fltSliderValue = GUILayout.VerticalSlider(fltSliderValue, 0.0f, 1.1f, GUILayout.Height(50));
        GUILayout.EndHorizontal();
        //Scrollbars
        GUILayout.BeginHorizontal();
     //   fltScrollerValue = GUILayout.HorizontalScrollbar(fltScrollerValue, 0.1f, 0.0f, 1.1f, GUILayout.Width(128));
     //   fltScrollerValue = GUILayout.VerticalScrollbar(fltScrollerValue, 0.1f, 0.0f, 1.1f, GUILayout.Height(90));
     //   GUILayout.Box("Im\na\ntest\nBox");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }
	
	
	
	    void DoMyWindow2(int windowID)
    {
        GUILayout.BeginHorizontal();
       		GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
			GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
		    GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
			GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
		    GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
			GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
		    GUILayout.Box("", virtuelForflytning.GetStyle("ToolboxField"));
			
        GUILayout.Space(8);
        
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        //Sliders
        GUILayout.BeginHorizontal();
     
        GUILayout.EndHorizontal();
        //Scrollbars
        GUILayout.BeginHorizontal();
    
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }
    
}
