using UnityEngine;
using System.Collections;

public class SlideMatWheelChair : MonoBehaviour
{

    public enum Position
    {
        NONE = 0,
        LEFT = 1,
        RIGHT = 2,
    }

    // Use this for initialization
    void Start()
    {
        Util.ToggleSubElementRenderer(this.gameObject, "slidemat_right");
        Util.ToggleSubElementRenderer(this.gameObject, "slidemat_left");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(Position pos)
    {

        switch (pos)
        {
            case Position.NONE:
                break;
            case Position.RIGHT:
                //position += -relZ + relX;
                Util.ToggleSubElementRenderer(this.gameObject, "slidemat_right");
                break;
            case Position.LEFT:
                //position += -relZ - relX;
                Util.ToggleSubElementRenderer(this.gameObject, "slidemat_left");
                break;
            default:
                Debug.LogWarning("Unhandled Helper Position: '" + pos.ToString() + "'.");
                return;
        }
    }

}
