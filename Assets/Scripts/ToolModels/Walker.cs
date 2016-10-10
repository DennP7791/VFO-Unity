using UnityEngine;
using System.Collections;

public class Walker : MonoBehaviour {


    public enum Position
    {
        ADJACENT = 0,
        CLOSE = 1,
        FAR = 2
    }

    //Position at the bed corners

    private Vector3 _bedBounds = new Vector3(1.007101f, 1.75f, 2.299285f);
    private float[] _distances = new float[] { 0.0f, 0.3f, 0.6f };

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(Position pos)
    {
		GameObject simObj = GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName"));
			if(simObj)
				simObj.SendMessage("SimCallback", "walkerPlaced_" + pos.ToString());
		
        GameObject go = GameObject.Find("Bed");
        GameObject frame = null;
        if (!go)
        {
            Debug.LogError("Missing Bed in the scene");
            return;
        }
        _bedBounds = new Vector3(
            1.007101f * go.transform.localScale.x,
            1.37f * go.transform.localScale.y,
            2.299285f * go.transform.localScale.z);

        frame = Util.FindSubElement(go, "pos_object");
        if (!frame)
        {
            Debug.LogError("Missing Bed_frame");
            return;
        }
        if (!frame.GetComponent<Collider>())
            frame.AddComponent<BoxCollider>();
        frame.GetComponent<Collider>().enabled = true;

        float Dist = 0.15f;
        Vector3 position = frame.GetComponent<Collider>().bounds.center;
		
        Vector3 lookAt = frame.GetComponent<Collider>().bounds.center;
        Vector3 x = go.transform.right * (_bedBounds.x * 0.5f + Dist);
        Vector3 y = go.transform.up * (_bedBounds.y * 0.5f);
        Vector3 z = go.transform.forward * (_bedBounds.z * 0.5f + Dist);
        position -= y;
        position += x;

        int idx = (int)pos;
        if (idx < _distances.Length)
        {
            position += go.transform.right * _distances[idx];
            position.x += 0.1463239f;
            position.z += 0.11655335f;
        }
        else
        {
            Debug.LogWarning("Unhandled Walker Position: '" + pos.ToString() + "'.");
            return;
        }

        this.transform.position = position;
        this.transform.Rotate(go.transform.eulerAngles, Space.World);
        //this.transform.LookAt(lookAt, go.transform.up);
    }

}
