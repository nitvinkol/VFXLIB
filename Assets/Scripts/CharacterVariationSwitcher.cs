using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CharacterVariation
{
    public GameObject[] variationGameObjects;
}

[ExecuteInEditMode]
public class CharacterVariationSwitcher : MonoBehaviour
{
    [Header("Variation Settings")]
    public CharacterVariation[] characters;

    [Space(10)] // Add some spacing

    [Header("Switch Variation")]
    public int selectedVariationIndex;

    public void ShowCharacterVariation()
    {
        // Disable all child game objects of the current game object
        DisableAllChildGameObjects(transform);

        // Enable the selected character and variation
        if (selectedVariationIndex < characters.Length)
        {
            var selectedCharacter = characters[selectedVariationIndex];

            foreach (var go in selectedCharacter.variationGameObjects)
            {
                go.SetActive(true);
            }
        }
    }

    private void DisableAllChildGameObjects(Transform parent)
    {
        // Disable all child game objects of the given parent transform
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CharacterVariationSwitcher))]
public class CharacterVariationSwitcherEditor : Editor
{
    SerializedProperty selectedCharacterIndexProp;
    SerializedProperty charactersProp;

    void OnEnable()
    {
        selectedCharacterIndexProp = serializedObject.FindProperty("selectedVariationIndex");
        charactersProp = serializedObject.FindProperty("characters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(20f);
        GUIStyle centeredLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
        centeredLabelStyle.fontSize = 16;
        EditorGUILayout.LabelField("Character Variation Switcher", centeredLabelStyle, GUILayout.ExpandWidth(true));

        // Validate selectedVariationIndex to ensure it's within a valid range
        selectedCharacterIndexProp.intValue = Mathf.Clamp(selectedCharacterIndexProp.intValue, 0, charactersProp.arraySize - 1);

        EditorGUILayout.PropertyField(selectedCharacterIndexProp);
        if (GUILayout.Button("Show Variation", GUILayout.Height(40f)))
        {
            ((CharacterVariationSwitcher)target).ShowCharacterVariation();
        }
        EditorGUILayout.Space(10f);

        // Display the CharacterVariation array with custom labels
        for (int i = 0; i < charactersProp.arraySize; i++)
        {
            SerializedProperty characterElement = charactersProp.GetArrayElementAtIndex(i);
            SerializedProperty variationGameObjectsProp = characterElement.FindPropertyRelative("variationGameObjects");
            GUIContent elementLabel = new GUIContent("Character " + i.ToString());

            EditorGUILayout.PropertyField(variationGameObjectsProp, elementLabel, true);
        }

        EditorGUILayout.Space(10f);

        // Add a button to increase the array size
        if (GUILayout.Button("Add Character Variation"))
        {
            int newIndex = charactersProp.arraySize;
            charactersProp.InsertArrayElementAtIndex(newIndex);
        }

        // Add a button to decrease the array size
        if (GUILayout.Button("Remove Character Variation") && charactersProp.arraySize > 0)
        {
            charactersProp.DeleteArrayElementAtIndex(charactersProp.arraySize - 1);
        }

        // Add a button to clear the array
        if (GUILayout.Button("Clear Character Variations") && charactersProp.arraySize > 0)
        {
            charactersProp.ClearArray();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif