﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPoint))]
public class SpawnPointEditor : Editor
{
    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        SpawnPoint sp = (SpawnPoint)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(sp.location, Quaternion.identity);

        Handles.Disc(Quaternion.Euler(0, 0, 0), newPosition, new Vector3(0, 1, 0), sp.radius, false, 1);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(sp, "Change spawn point location");
            sp.location = newPosition;
        }
    }
}
