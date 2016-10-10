using UnityEngine;
using System.Collections;
using UnityEditor;

public class CopyPasteTransform : EditorWindow
{

    static CopyPasteTransform window;

    Material myMaterial;

    static Transform copyTransform;


    // Add menu named "My Window" to the Window menu
    [MenuItem("MyScripts/Copy paste ")]
    public static void Init()
    {
        
        // Get existing open window or if none, make a new one:
        window = (CopyPasteTransform)EditorWindow.GetWindow(typeof(CopyPasteTransform));
        
    }

    void OnGUI()
    {       

        if (GUILayout.Button("Copy"))
        {
            copyTransform = Selection.activeTransform;
        }

        if (GUILayout.Button("Paste"))
        {
            Selection.activeTransform.localPosition = copyTransform.localPosition;
            Selection.activeTransform.localRotation = copyTransform.localRotation;
            Selection.activeTransform.localScale = copyTransform.localScale;
        }

        if (copyTransform != null)
        GUILayout.Label("Copy transform:\nPosition: " + copyTransform.localPosition + "\nRotation: " + copyTransform.localRotation + "\nScale: " + copyTransform.localScale);



    }



}
