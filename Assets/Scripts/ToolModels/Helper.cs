using UnityEngine;
using System.Collections;

public class Helper : MonoBehaviour {

    public enum Position
    {
        UPPERCENTER = 0,
        UPPERRIGHT = 1,
        CENTERRIGHT = 2,
        BOTTOMRIGHT = 3,
        BOTTOMCENTER = 4,
        BOTTOMLEFT = 5,
        CENTERLEFT = 6,
        UPPERLEFT = 7,
    }

    //Distance from the bed
    public float Dist = 0.2f;
    //Position at the bed corners
    public float CornerPosition = 0.5f;

    //The size of the bed's bounding volume at scale 1.0
    //WHY hardcoding them instead of using the collider object?
    //unity physics engine uses axes aligned bounding volumes
    //this means that the bounds will be different depending on the object orientation
    //note: the documentation doesn't mention it and I don't know if this optimization can be disabled.
    private Vector3 _bedBounds = new Vector3(1.007101f, 1.75f, 2.299285f);

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(Position pos)
    {
        GameObject go = GameObject.Find("Bed");
        GameObject frame = null;
        if (!go)
        {
            Debug.LogError("Missing Bed in the scene");
            return;
        }
        _bedBounds = new Vector3(
            1.007101f*go.transform.localScale.x, 
            1.37f*go.transform.localScale.y, 
            2.299285f*go.transform.localScale.z);

        frame = Util.FindSubElement(go, "pos_object");

        if (!frame)
        {
            Debug.LogError("Missing Pos_Object");
            return;
        }
        if (!frame.GetComponent<Collider>())
            frame.AddComponent<BoxCollider>();
        frame.GetComponent<Collider>().enabled = false;

        Vector3 position = frame.GetComponent<Collider>().bounds.center;
        Vector3 lookAt = frame.GetComponent<Collider>().bounds.center;
        Vector3 x = go.transform.right * (_bedBounds.x * 0.5f + Dist);
        Vector3 y = go.transform.up * (_bedBounds.y * 0.5f);
        Vector3 z = go.transform.forward * (_bedBounds.z * 0.5f + Dist);
        position -= y;

        switch (pos)
        {
            case Position.UPPERCENTER:
                lookAt = position;
                position += z;
                break;
            case Position.UPPERRIGHT:
                position += z * CornerPosition;
                lookAt = position;
                position += x;
                break;
            case Position.CENTERRIGHT:
                lookAt = position;
                position += x;
                break;
            case Position.BOTTOMRIGHT:
                position -= z * CornerPosition;
                lookAt = position;
                position += x;
                break;
            case Position.BOTTOMCENTER:
                lookAt = position;
                position -= z;
                break;
            case Position.BOTTOMLEFT:
                position -= z * CornerPosition;
                lookAt = position;
                position -= x;
                break;
            case Position.CENTERLEFT:
                lookAt = position;
                position -= x;
                break;
            case Position.UPPERLEFT:
                position += z * CornerPosition;
                lookAt = position;
                position -= x;
                break;
            default:
                Debug.LogWarning("Unhandled Helper Position: '" + pos.ToString()+"'.");
                return;
        }

        this.transform.position = position;
        this.transform.Rotate(go.transform.eulerAngles);
        this.transform.LookAt(lookAt , go.transform.up);

        DeleteToolFromToolBox();
    }

    public void DeleteToolFromToolBox()
    {
        int idx = -1;
        ToolButton[] buttons = Global.Instance.toolButtonArray;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].Text.Equals("Helper"))
            {
                idx = i;
            }
        }
        if (idx != -1)
        {
            ToolButton[] newButtons = new ToolButton[buttons.Length - 1];
            int j = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != idx)
                {
                    newButtons[j++] = buttons[i];
                }
            }
            Global.Instance.toolButtonArray = newButtons;
        }
    }

}
