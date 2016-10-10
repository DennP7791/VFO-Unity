using UnityEngine;
using System.Collections;

public class IdUpdater : MonoBehaviour {

    public void SedUserId(int id)
    {
        Global.Instance.UserId = id;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
