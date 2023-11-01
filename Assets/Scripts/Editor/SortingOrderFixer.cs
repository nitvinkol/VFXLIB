using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SortingOrderFixer : EditorWindow
{
    private GameObject targetGameObject;
    private string[] sortingLayers;
    private int selectedSortingLayerIndex;
    private bool ignoreZDepth = false;
    private int startingSortingOrder = 0; // Added variable for the starting sorting order

    [MenuItem("NVK_Tools/Extra Tools/Sorting Order Fixer")]
    public static void ShowWindow()
    {
        GetWindow<SortingOrderFixer>("Sorting Order Fixer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sorting Order Fixer", EditorStyles.boldLabel);

        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target Game Object", targetGameObject, typeof(GameObject), true);

        if (targetGameObject != null)
        {
            sortingLayers = SortingLayerHelper.GetSortingLayerNames();
            selectedSortingLayerIndex = EditorGUILayout.Popup("Select Sorting Layer", selectedSortingLayerIndex, sortingLayers);
            ignoreZDepth = EditorGUILayout.Toggle("Ignore Z-Depth", ignoreZDepth);

            // Added input field for the starting sorting order
            startingSortingOrder = EditorGUILayout.IntField("Starting Sorting Order", startingSortingOrder);

            if (GUILayout.Button("Fix Sorting Orders"))
            {
                FixSortingOrders(targetGameObject, selectedSortingLayerIndex, ignoreZDepth, startingSortingOrder);
            }
        }
    }

    private void FixSortingOrders(GameObject parent, int sortingLayerIndex, bool ignoreZ, int startSortingOrder)
    {
        if (parent == null)
            return;

        int currentSortingOrder = startSortingOrder; // Initialize with the starting sorting order
        Renderer[] rendererComponents = parent.GetComponentsInChildren<Renderer>();

        // Sort renderers based on their current sorting order
        List<Renderer> sortedRenderers = new List<Renderer>(rendererComponents);
        sortedRenderers.Sort((a, b) => a.sortingOrder.CompareTo(b.sortingOrder));

        foreach (var renderer in sortedRenderers)
        {
            if (!ignoreZ)
            {
                float z = renderer.transform.position.z;
                renderer.sortingLayerID = SortingLayer.NameToID(sortingLayers[sortingLayerIndex]);
                renderer.sortingOrder = currentSortingOrder;
                currentSortingOrder++;
            }
            else
            {
                renderer.sortingLayerID = SortingLayer.NameToID(sortingLayers[sortingLayerIndex]);
                renderer.sortingOrder = currentSortingOrder;
                currentSortingOrder++;
            }
        }
    }
}
