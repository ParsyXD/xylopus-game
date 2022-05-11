using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldItem))]
public class WorldItemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WorldItem script = (WorldItem)target;
        if (GUILayout.Button("Generate item ID"))
        {
            script.GetItemId();
        }
    }
}