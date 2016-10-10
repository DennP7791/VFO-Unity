using UnityEngine;
using System.Collections;
using UnityEditor;

public class MySelectionScripts : MonoBehaviour {

    [MenuItem ("MyScripts/Selection/Deactivate objects %#d")]
    static void DoDeactivate()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            go.active = false;
        }
    }

    [MenuItem("MyScripts/Selection/Activate objects %#a")]
    static void DoActivate()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            go.active = true;
        }
    }

}
