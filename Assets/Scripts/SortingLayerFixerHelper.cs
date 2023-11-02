using UnityEngine;
using UnityEditor;

public static class SortingLayerFixerHelper
{
    public static string[] GetSortingLayerNames()
    {
        string[] sortingLayerNames = new string[SortingLayer.layers.Length];
        for (int i = 0; i < SortingLayer.layers.Length; i++)
        {
            sortingLayerNames[i] = SortingLayer.layers[i].name;
        }
        return sortingLayerNames;
    }

    public static int GetSortingLayerID(string sortingLayerName)
    {
        for (int i = 0; i < SortingLayer.layers.Length; i++)
        {
            if (SortingLayer.layers[i].name == sortingLayerName)
            {
                return SortingLayer.layers[i].id;
            }
        }
        return 0; // Default layer
    }
}
