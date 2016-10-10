using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WindowHandler : MonoBehaviour {

    private static WindowHandler _instance; //singleton
    private Dictionary<int, GameObject> _windowsOld = new Dictionary<int, GameObject>();
    private Dictionary<int, BaseWindow> _windows = new Dictionary<int, BaseWindow>();
    private int _id = 1001;
    private int _depth = int.MaxValue-1001;
    // Singleton
    private static WindowHandler Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (WindowHandler)GameObject.FindObjectOfType(typeof(WindowHandler));
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "WindowHandler";
                    _instance = (WindowHandler)container.AddComponent(typeof(WindowHandler));
                }
            }
            return _instance;
        }
    }

    private int NextDepth()
    {
        return _depth > 1 ? _depth-- : _depth;
    }

    public int _Register2(BaseWindow window)
    {
        //it means that the depth has been externally assigned
        if (window.Depth == -1)
        {
            window.Depth = NextDepth();
        }
        _windows.Add(++_id, window);
        return _id;
    }

    public static int Register2(BaseWindow window)
    {
        return Instance._Register2(window);
    }

    public void _UnRegister2(int id)
    {
        _windows.Remove(id);
    }

    public static void UnRegister2(int id)
    {
        Instance._UnRegister2(id);
    }


    public int _Register(GameObject go)
    {
        _windowsOld.Add(++_id, go);
        return _id;
    }

    public static int Register(GameObject go)
    {
        return Instance._Register(go);
    }

    public void _UnRegister(int id)
    {
        _windowsOld.Remove(id);
    }

    public static void UnRegister(int id)
    {
        Instance._UnRegister(id);
    }

    public void _BringWindowToFront(BaseWindow win)
    {
        if(win.Depth > _depth || win.Depth == -1)
        {
            win.Depth = NextDepth();
        }
    }

    public static void BringWindowToFront(BaseWindow win)
    {
        Instance._BringWindowToFront(win);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
