using System.Collections.Generic;
using UnityEditor;
public static class SortingLayerHelper
{
    public static string[] GetSortingLayerNames()
    {
        List<string> sortingLayerNames = new List<string>();
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty sortingLayers = tagManager.FindProperty("m_SortingLayers");

        for (int i = 0; i < sortingLayers.arraySize; i++)
        {
            SerializedProperty sortingLayer = sortingLayers.GetArrayElementAtIndex(i);
            string layerName = sortingLayer.FindPropertyRelative("name").stringValue;
            sortingLayerNames.Add(layerName);
        }

        return sortingLayerNames.ToArray();
    }
}
