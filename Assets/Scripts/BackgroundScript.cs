using UnityEngine;
using System.Collections;

public class BackgroundScript : BaseWindow
{
    //
    #region Inspector Parameters

    private Texture2D BackgroundImage;
    public Texture2D BackgroundImage_dk;
    public Texture2D BackgroundImage_se;
    public Texture2D BackgroundFill;
    public bool Visible = true;
    
    #endregion

    //--------------------------------------------------------------------

    private Rect _backgroundRect;
    private GUIStyle _style;
    private GUIStyle _fillstyle;

    public override void WinOnGUI()
    {
        if (Visible && BackgroundFill)
        {
            Box(new Rect(0, 0, Screen.width, Screen.height), "", _fillstyle);
        }
        if (Visible && BackgroundImage)
        {
            Box(new Rect((((Screen.width - 400.0f) * 0.5f) + 400f) - BackgroundImage.width * 0.5f , 105.0f, BackgroundImage.width, BackgroundImage.height), "", _style);
        }
    }

	// Use this for initialization
	public override void WinStart ()
    {
        if (Global.Instance.ProgramLanguage == "sv-SE")
            BackgroundImage = BackgroundImage_se;
        else
            BackgroundImage = BackgroundImage_dk;

        Util.InstantiateResource<LinkMenu1>("LinkMenu1");
        GameObject menu = GameObject.Find("MainMenu");
        if (menu)
        {
            GameObject bgo = Util.FindSubElement(menu, "BottomBar");
            if (bgo)
            {
                bgo.GetComponent<BottomBarScript>().enabled = true;
            }
            GameObject tgo = Util.FindSubElement(menu, "TopBar");
            if (tgo)
            {
                tgo.GetComponent<TobBarScript>().enabled = true;
            }
        }
        BottomBarScript.SetButtonDisabled(0, false);
        BottomBarScript.SetButtonDisabled(3, false);

        if (BackgroundFill && BackgroundImage)
        {
            _style = new GUIStyle();
            _fillstyle = new GUIStyle();

            _fillstyle.normal.background = BackgroundFill;
            _style.normal.background = BackgroundImage;
        }
        else
        {
            Debug.LogWarning(
                "The variable 'BackgroundImage' of "+this.name+" has not been assigned.\n" +
                "You probably need to Assign it in the inspector.");
        }
	}
	
	// Update is called once per frame
	public override void WinUpdate () {
	
	}
}
