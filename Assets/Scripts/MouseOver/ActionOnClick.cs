using UnityEngine;
using System.Collections;

public class ActionOnClick : MonoBehaviour 
{
	private static ActionOnClick _instance; //singleton
			
	public static ActionOnClick Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (ActionOnClick)GameObject.FindObjectOfType(typeof(ActionOnClick));

                if (!_instance)
                {
                    Debug.LogError("ActionOnClick instance could not be found, make sure to add a patient to the scene, and add the ActionOnClick script to it");
                } 
            }

            return _instance;
        }
    }
	
	// Run when ever the user clicks an action
	public void Action(string objectName)
	{
		if(!Util.AnyVisibleResource<Message>() && !Util.AnyVisibleResource<HUD>() && !Util.AnyVisibleResource<HUDWalker>() && !Util.AnyVisibleResource<HUDBed>() && !Util.AnyVisibleResource<HUDTiled>())
		{
			// Sends a message to the simulation script and calls the SimCallback function in that script
			GameObject go = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
			if(go) go.SendMessage("SimCallback", objectName);
			else Debug.LogError("GameObject " + States.Instance.GetStateValue("actionCallbackGameObjectName") + " could not be found for callback");
		}
	}
}
