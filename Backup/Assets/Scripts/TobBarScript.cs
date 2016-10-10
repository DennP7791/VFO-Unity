using UnityEngine;
using System.Collections;

/// <summary>
///     By requirements:
///     TopBar is fixed size, always centered with respect to the horizontal axes
/// </summary>
public class TobBarScript : MonoBehaviour
{

    #region Inspector Parameters

    public Texture2D TopBarTexture;
    public bool Visible = true;
    public int Depth = 0;

    #endregion

    //--------------------------------------------------------------------

    private GUIStyle _style;
    private Rect _topBarRect;
    private float _textureCenter;

    void OnGUI()
    {
        if (Visible && TopBarTexture)
        {
            _topBarRect.x = Screen.width / 2 - _textureCenter;
            _topBarRect.y = 0;

            GUI.depth = Depth;
            GUI.Box(_topBarRect, "", _style);
        }
    }

	// Use this for initialization
	void Start () {
        if (TopBarTexture)
        {
            _style = new GUIStyle();
            if (TopBarTexture)
                _style.normal.background = TopBarTexture;
            _textureCenter = TopBarTexture.width / 2f;
            _topBarRect = new Rect(0f, 0f, TopBarTexture.width, TopBarTexture.height);
        }
        else
        {
            Debug.LogWarning(
                "The variable 'TopBarTexture' of "+this.name+" has not been assigned.\n" +
                "You probably need to Assign it in the inspector.");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
