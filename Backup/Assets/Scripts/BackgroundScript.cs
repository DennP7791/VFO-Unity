using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour
{
    //
    #region Inspector Parameters

    public Texture2D BackgroundImage;
    public bool Visible = true;
    public int Depth = 100;
    
    #endregion

    //--------------------------------------------------------------------

    private Rect _backgroundRect;
    private Vector2 _bCenter; //Background Center
    private Vector2 _sCenter; //Screen Center
    private GUIStyle _style;

    void OnGUI()
    {
        if (Visible && BackgroundImage)
        {
            GUI.depth = Depth;
            _sCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            _backgroundRect.x = _sCenter.x - _bCenter.x;
            _backgroundRect.y = _sCenter.y - _bCenter.y;
            _style.normal.background = BackgroundImage;
            GUI.Box(_backgroundRect, "", _style);
        }
    }

	// Use this for initialization
	void Start () {
        if (BackgroundImage)
        {
            _style = new GUIStyle();
            _bCenter = new Vector2(BackgroundImage.width / 2, BackgroundImage.height / 2);
            _sCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            _backgroundRect = new Rect(
                _sCenter.x - _bCenter.x,
                _sCenter.y - _bCenter.y,
                BackgroundImage.width,
                BackgroundImage.height);
        }
        else
        {
            Debug.LogWarning(
                "The variable 'BackgroundImage' of "+this.name+" has not been assigned.\n" +
                "You probably need to Assign it in the inspector.");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
