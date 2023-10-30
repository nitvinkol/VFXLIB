using UnityEngine;
using UnityEditor;

public class SortingOrderUpdaterEditor : EditorWindow
{
    private int offsetValue = 0;
    private int selectedOption = 0;
    private string[] options = new string[] { "Increment", "Decrement" };

    [MenuItem("NVK_Tools/Extra Tools/Update Sorting Order")]
    static void ShowWindow()
    {
        GetWindow<SortingOrderUpdaterEditor>("Update Sorting Order");
    }

    void OnGUI()
    {
        GUILayout.Label("Update Sorting Order", EditorStyles.boldLabel);

        offsetValue = EditorGUILayout.IntField("Offset Value:", offsetValue);
        selectedOption = EditorGUILayout.Popup("Operation:", selectedOption, options);

        if (GUILayout.Button("Update Sorting"))
        {
            UpdateSortingOrder();
        }
    }

    private void UpdateSortingOrder()
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        int increment = (selectedOption == 0) ? 1 : -1;

        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder += increment * offsetValue;
        }

        Debug.Log("Sorting Order Updated for " + renderers.Length + " SpriteRenderers.");
    }
}
