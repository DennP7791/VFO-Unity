using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown()
	{
		GameObject go = GameObject.Find("borger");
		if(go) go.GetComponent<MainTest>().MouseOverPoint(gameObject.name);
		else Debug.LogError("couldn't find borger game object");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
