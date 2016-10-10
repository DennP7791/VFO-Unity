using UnityEngine;
using System.Collections;

//OLD not used...

public class BackgroundBehaviour : MonoBehaviour {

    GUITexture _image;
    Camera _camera;
    Rect _imageRect;
    Vector2 _start;
    Vector2 _target;
    Vector2 _position;

	// Use this for initialization
	void Start () {
        string name = this.gameObject.name;
        _image = GameObject.Find("/"+name+"/Image").GetComponent<GUITexture>();
        if (!_image)
        {
            Debug.LogError("No background image assigned.");
        }
        _imageRect = new Rect(0 - Screen.width / 2, 0 - Screen.height / 2, Screen.width, Screen.height);
        _image.pixelInset = _imageRect;
        _start = new Vector2(0, 0);
        _position = new Vector2(0, 0);
        _target = new Vector2(0, 50);
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    Debug.Log("it works: "+_imageRect.y);
        //    _position = Vector2.Lerp(_start, _target, 0.1f);
        //    _imageRect.y += _position.y;
        //    //_imageRect.y += _position.y;
        //    Debug.Log("start: " + _position.ToString());
        //    Debug.Log("end: " + _target.ToString());
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    _imageRect.y += 5;
        //}
        _image.pixelInset = _imageRect;
	}

    void PullUp()
    {

    }

    void PullDown()
    {

    }
}
