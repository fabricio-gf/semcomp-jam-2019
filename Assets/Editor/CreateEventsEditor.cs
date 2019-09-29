using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateEvents))]
public class CreateEventsEditor : Editor
{
    bool deleteAll;
    bool areYouSure;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();

        CreateEvents myScript = (CreateEvents)target;

        deleteAll = GUILayout.Toggle(deleteAll, "Delete all scriptable objects in location?");
        if (deleteAll)
        {
            areYouSure = GUILayout.Toggle(areYouSure, "Are you sure?");
        }

        if(GUILayout.Button("Create Scriptable Objects"))
        {
            myScript.CreateScriptableObjects(areYouSure);
        }

        /*if(GUILayout.Button("Update Store Objects"))
        {
            myScript.UpdateStore();
        }*/
    }
}
