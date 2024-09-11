using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlankArray))]
public class PlankArrayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlankArray plankArray = (PlankArray)target;

        if (GUILayout.Button("Generate Planks"))
        {
            plankArray.GeneratePlanks();
        }

        if (GUILayout.Button("Clear Planks"))
        {
            plankArray.ClearPlanks();
        }
    }
}
