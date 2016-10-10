using UnityEngine;
using System.Collections;

//Static toolbox, made for demo purposes

public class StaticToolBox : MonoBehaviour {

    //Copy pasted from BottomBarScript 
    public class Button
    {
        public string Function = "";
        public bool Toggle = false;
        public Texture2D normalTexture;
        public Texture2D hoverTexture;
        public Texture2D clickTexture;
        public Rect rect = new Rect(0.0f, 0.0f, 50f, 50f);
        public float maxSize = 1.5f;
        public float animSpeed = 3.0f;

        private GUIStyle _style = new GUIStyle();
        private float _currSize = 1.0f;
        private Rect _currRect = new Rect();
        private bool _first = true;


        public void Draw()
        {
            if (_first)
            {
                _first = false;
                _currRect.width = rect.width;
                _currRect.height = rect.height;
                _currRect.x = rect.x;
                _currRect.y = Screen.height - rect.y;
            }

            if (normalTexture)
                _style.normal.background = normalTexture;
            if (hoverTexture)
                _style.hover.background = hoverTexture;
            if (clickTexture)
                _style.active.background = clickTexture;

            if (GUI.Button(_currRect, "", _style) && Function.Length > 0)
                    Global.Instance.SendMessage(Function);

        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
