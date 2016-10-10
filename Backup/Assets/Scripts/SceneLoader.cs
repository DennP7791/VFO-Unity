using UnityEngine;
using System.Collections;

//Singleton class intended for Scene loading
//Its functions are supposed to be called by the links in LinkMenu Script (initialized via the inspector);

public class SceneLoader : MonoBehaviour {

    private static SceneLoader _instance;

    private uint _current_scene = 0;


    public uint CurrentScene
    {
        get
        {
            return _current_scene;
        }
        set
        {
            _current_scene = value;

            //code for handling the change here...
        }
    }

    public static SceneLoader Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (SceneLoader)GameObject.FindObjectOfType(typeof(SceneLoader));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SceneLoader";
                    _instance = (SceneLoader)container.AddComponent(typeof(SceneLoader));
                }
            }

            return _instance;
        }
    }

    //Example funtions 
    public void Scene0()
    {
        _current_scene = 0;
        Debug.Log("loading scene" + _current_scene);
    }

    //The scene has to be added to File->(Build Settings)->(Scenes In Build)
    public void Scene1()
    {
        _current_scene = 1;
        Application.LoadLevel("scene01");
        GameObject tmp; 
        tmp = GameObject.Find("Background");
        if (tmp) tmp.active = false;
        tmp = GameObject.Find("LinkMenu");
        if (tmp) tmp.active = false;

        Debug.Log("loading scene" + _current_scene);
    }

    public void Scene2()
    {
        _current_scene = 2;
        Debug.Log("loading scene" + _current_scene);
    }

    public void Scene3()
    {
        _current_scene = 3;
        Debug.Log("loading scene" + _current_scene);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
