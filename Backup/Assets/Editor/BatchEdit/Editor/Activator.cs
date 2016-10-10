using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;

[CustomEditor(typeof(Transform))]
public class Activator : Editor
{
    void OnEnable()
    {
        BatchEditing.BatchEditingInterface.Enable();
    }

    public override void  OnInspectorGUI()
    {
        Transform xTarget = target as Transform;
        Vector3 vOldPosition = xTarget.localPosition;
        Vector3 vOldRotation = xTarget.localEulerAngles;
        Vector3 vOldScale = xTarget.localScale;
        Vector3 vNewPosition = EditorGUILayout.Vector3Field("Position", vOldPosition);
        Vector3 vNewRotation = EditorGUILayout.Vector3Field("Rotation", vOldRotation);
        Vector3 vNewScale = EditorGUILayout.Vector3Field("Scale", vOldScale);

        BatchEditing.BatchEditingInterface.DoTransformSpecialCase(vOldPosition, vOldRotation, vOldScale, vNewPosition, vNewRotation, vNewScale);
    }
}
