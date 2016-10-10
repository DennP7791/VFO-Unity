using UnityEngine;
using System.Collections;

public class RotateDummy : MonoBehaviour {
	
	public Texture2D leftArrow;
	public Texture2D rightArrow;
	
	public Rect leftArrowRect;
	public Rect rightArrowRect;
	
	// Use this for initialization
	void Start () {
	}
	
	void OnGUI()
	{
        leftArrowRect.y = Screen.height - 115;
        rightArrowRect.y = Screen.height - 115;
        leftArrowRect.x = (int)(180.0f * ((float)Screen.width / 980.0f));
        rightArrowRect.x = (int)(110.0f * ((float)Screen.width / 980.0f));
		GUI.depth = int.MaxValue;
		GUI.DrawTexture(leftArrowRect, leftArrow);
		GUI.DrawTexture(rightArrowRect, rightArrow);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButton(0))
		{
			if(leftArrowRect.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y)))
			{
				gameObject.transform.RotateAroundLocal(new Vector3(0, 1, 0), -1.8f * Time.deltaTime);
			}
			else if(rightArrowRect.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y)))
			{
				gameObject.transform.RotateAroundLocal(new Vector3(0, 1, 0), 1.8f * Time.deltaTime);
			}
		}
	}
}
