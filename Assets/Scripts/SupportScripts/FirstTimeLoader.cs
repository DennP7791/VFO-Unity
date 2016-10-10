using UnityEngine;
using System.Collections;

public class FirstTimeLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneLoader.Instance.CurrentScene = 0;
        Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("data_loader_getting_data"), Message.Type.Info, false, true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
