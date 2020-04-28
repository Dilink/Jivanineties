using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

[CustomEditor(typeof(AbsorptionArea))]
public class AbsorptionAreaBoundaries : OdinEditor
{
    private AbsorptionArea area;

    new void OnEnable()
    {
        base.OnEnable();
        area = target as AbsorptionArea;
    }

    void OnSceneGUI()
    {
        if (!area.isEditingBoundaries)
        {
            return;
        }

        for (int i = 0; i < area.boundaries.Length; i++)
        {
            Handles.Label(area.boundaries[i] + Vector3.up * 1.5f, "" + (i + 1));

            EditorGUI.BeginChangeCheck();

            Vector3 newPosition = Handles.PositionHandle(area.boundaries[i], Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                area.boundaries[i] = newPosition;
            }

            if (area.boundaries.Length > 1)
            {
                Handles.DrawPolyLine(area.boundaries);
                Handles.DrawPolyLine(area.boundaries[0], area.boundaries[area.boundaries.Length - 1]);
            }
        }
    }
}
