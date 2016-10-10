using UnityEngine;
using System.Collections;

public class Window2 : BaseWindow
{

    // Use this for initialization

    public override void WinOnGUI()
    {
        Button(new Rect(50, 0, 100, 100), "asd2");
    }

    public override void WinUpdate()
    {
        Debug.Log("WinUpdate");
    }

    public override void WinStart()
    {
        this.Position = new Rect(100, 100, 200, 200);
        Debug.Log("WinStart");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
