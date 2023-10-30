using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SortingLayerSorter : EditorWindow
{
    private GameObject targetGameObject;
    private string[] sortingLayers;
    private int selectedSortingLayerIndex;
    private int startSortingOrder;
    private int sortingOrderIncrement = 1;
    private int orderDirectionIndex = 0;

    private string[] orderDirections = { "Ascending", "Descending" };

    private bool ignoreZDepth = false;

    [MenuItem("NVK_Tools/Extra Tools/Sorting Layer Sorter")]
    public static void ShowWindow()
    {
        GetWindow<SortingLayerSorter>("Sorting Layer Sorter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sorting Layer Sorter", EditorStyles.boldLabel);

        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target Game Object", targetGameObject, typeof(GameObject), true);

        if (targetGameObject != null)
        {
            sortingLayers = SortingLayerHelper.GetSortingLayerNames();
            selectedSortingLayerIndex = EditorGUILayout.Popup("Select Sorting Layer", selectedSortingLayerIndex, sortingLayers);

            startSortingOrder = EditorGUILayout.IntField("Start Sorting Order", startSortingOrder);
            sortingOrderIncrement = EditorGUILayout.IntField("Sorting Order Increment", sortingOrderIncrement);

            orderDirectionIndex = EditorGUILayout.Popup("Sort Order", orderDirectionIndex, orderDirections);

            ignoreZDepth = EditorGUILayout.Toggle("Ignore Z-Depth", ignoreZDepth);

            if (GUILayout.Button("Sort"))
            {
                SortChildren(targetGameObject, selectedSortingLayerIndex, startSortingOrder, sortingOrderIncrement, orderDirectionIndex, ignoreZDepth);
            }
        }
    }

    private void SortChildren(GameObject parent, int sortingLayerIndex, int startSortingOrder, int orderIncrement, int directionIndex, bool ignoreZ)
    {
        if (parent == null)
            return;

        int currentSortingOrder = startSortingOrder;
        int direction = (directionIndex == 0) ? 1 : -1; // 1 for ascending, -1 for descending

        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>();
        Dictionary<float, int> zToSortingOrder = new Dictionary<float, int>();

        foreach (var renderer in renderers)
        {
            if (ignoreZDepth)
            {
                renderer.sortingLayerID = SortingLayer.NameToID(sortingLayers[sortingLayerIndex]);
                renderer.sortingOrder = currentSortingOrder;
                currentSortingOrder += orderIncrement * direction;
            }
            else
            {
                float z = renderer.transform.position.z;

                if (!zToSortingOrder.ContainsKey(z))
                {
                    zToSortingOrder[z] = currentSortingOrder;
                    currentSortingOrder += orderIncrement * direction;
                }

                renderer.sortingLayerID = SortingLayer.NameToID(sortingLayers[sortingLayerIndex]);
                renderer.sortingOrder = zToSortingOrder[z];
            }
        }
    }
}
