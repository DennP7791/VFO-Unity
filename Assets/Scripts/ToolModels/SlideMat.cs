using UnityEngine;
using System.Collections;

public class SlideMat : MonoBehaviour {

    public enum Position
    {
        TOPLEFT = 0,
        TOPRIGHT = 1,
        BOTTOMLEFT = 2,
        BOTTOMRIGHT = 3
    }

    //Distance from from each side towards the intern
    public Vector3 Offset = Vector3.zero;

    private Vector3 _bedBounds = Vector3.zero;
    private Vector3 _matBounds = Vector3.zero;

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
        this.gameObject.GetComponent<Renderer>().enabled = false;
        GameObject go = GameObject.Find("Bed");
        if (!go)
        {
            Debug.LogError("Missing Bed in the scene");
            return;
        }
        //_bedBounds = new Vector3(
        //    0.7f * go.transform.localScale.x,
        //    0.26f * go.transform.localScale.y,
        //    1.4f * go.transform.localScale.z);

        //foreach (Transform t in go.transform)
        //{
        //    if (t.gameObject.name.Equals("Bed_Frame"))
        //    {
        //        frame = t.gameObject;
        //    }
        //}
        //if (!frame)
        //{
        //    Debug.LogError("Missing Bed_frame");
        //    return;
        //}

        //this.gameObject.AddComponent<BoxCollider>();
        //Vector3 size = this.gameObject.collider.bounds.size * 0.5f;
        //this.gameObject.collider.enabled = false;
        
        //Vector3 position = frame.transform.position;
        //Vector3 x = go.transform.right * _bedBounds.x * 0.5f;
        //Vector3 y = go.transform.up * _bedBounds.y;
        //Vector3 z = go.transform.forward * _bedBounds.z * 0.5f;
        //position += y;

        //position += go.transform.right * Offset.x
        //          + go.transform.up * Offset.y
        //          + go.transform.forward * Offset.z;

        ////X and Z relative to the object center;
        //Vector3 relZ = z - go.transform.forward * size.z;
        //Vector3 relX = x - go.transform.right * size.x;


        switch (pos)
        {
            case Position.TOPRIGHT:
                //position += relZ + relX;
                Util.ToggleSubElementRenderer(go, "up_left_slide");
                break;
            case Position.BOTTOMRIGHT:
                //position += -relZ + relX;
                Util.ToggleSubElementRenderer(go, "bottom_right_slide");
                break;
            case Position.BOTTOMLEFT:
                //position += -relZ - relX;
                Util.ToggleSubElementRenderer(go, "bottom_left_slide");
                break;
            case Position.TOPLEFT:
                //position += relZ - relX;
                Util.ToggleSubElementRenderer(go, "up_right_slide");
                break;
            default:
                Debug.LogWarning("Unhandled Helper Position: '" + pos.ToString() + "'.");
                return;
        }

        //this.transform.position = position;
        //this.transform.Rotate(go.transform.eulerAngles, Space.World);
    }

}
