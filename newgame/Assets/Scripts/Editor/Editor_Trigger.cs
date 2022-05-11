using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trigger))]
public class Editor_Trigger : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Trigger script = (Trigger)target;
        if (GUILayout.Button("GenerateID"))
        {
            script.GetTriggerID();
        }
    }
}
