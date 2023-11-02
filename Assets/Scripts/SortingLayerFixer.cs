using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SortingLayerFixer : EditorWindow
{
    private GameObject targetObject;
    private int selectedSortingLayer = 0;

    [MenuItem("NVK_Tools/Sorting Layer Fixer")]
    public static void ShowWindow()
    {
        GetWindow<SortingLayerFixer>("Sorting Layer Fixer");
    }

    private void OnGUI()
    {
        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true) as GameObject;

        if (targetObject != null)
        {
            string[] sortingLayerNames = SortingLayerFixerHelper.GetSortingLayerNames();
            selectedSortingLayer = EditorGUILayout.Popup("Select Sorting Layer", selectedSortingLayer, sortingLayerNames);

            if (GUILayout.Button("Fix"))
            {
                FixSortingLayers(targetObject, sortingLayerNames[selectedSortingLayer]);
            }
        }
    }

    private void FixSortingLayers(GameObject obj, string targetSortingLayerName)
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(obj.GetComponentsInChildren<SpriteRenderer>(true));

        spriteRenderers.Sort((a, b) =>
        {
            int sortingLayerCompare = a.sortingLayerID.CompareTo(b.sortingLayerID);
            if (sortingLayerCompare != 0)
            {
                return sortingLayerCompare;
            }
            return a.sortingOrder.CompareTo(b.sortingOrder);
        });

        int currentSortingOrder = 0;
        int currentSortingLayerID = SortingLayerFixerHelper.GetSortingLayerID(targetSortingLayerName);
        int previousSortingLayerID = spriteRenderers[0].sortingLayerID;

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            Undo.RecordObject(renderer, "Fix Sorting Layers");
            if (renderer.sortingLayerID != previousSortingLayerID)
            {
                currentSortingOrder = 0;
                previousSortingLayerID = renderer.sortingLayerID;
            }
            renderer.sortingLayerID = currentSortingLayerID;
            renderer.sortingOrder = currentSortingOrder;
            currentSortingOrder++;
        }
    }
}
