using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainLearning : MonoBehaviour 
{
    public void InfoClicked(Message message, bool value)
	{
        Text.Instance.StopAudio();
		
        BottomBarScript.EnableInfoButton(true);
		Global.Instance.HasPreasurePointsRun = true;
		SceneLoader.Instance.StartContainer();
	}

	// Use this for initialization
	void Start () 
	{	
		BottomBarScript.EnableInfoButton(false);
		Message msg = Util.InfoWindow(new Rect(0, 0, 800, 470), Text.Instance.GetStringAndPlaySpeak(Global.Instance.InfoWindowText), true, Message.Type.Info, false, true, true, InfoClicked);
		
		msg.Depth = int.MaxValue - 50;
	}
}
