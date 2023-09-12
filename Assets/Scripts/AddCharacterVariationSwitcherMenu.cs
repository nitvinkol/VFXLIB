using UnityEditor;
using UnityEngine;

public static class AddCharacterVariationSwitcherMenu
{
    // Gives the option to right click on the gameobject and add this component

    [MenuItem("GameObject/NVK_Tools/Add Character Variation Switcher")]
    private static void AddCharacterVariationSwitcher()
    {
        GameObject selectedGameObject = Selection.activeGameObject;

        if (selectedGameObject != null)
        {
            // Check if the selected GameObject already has a CharacterVariationSwitcher component
            if (selectedGameObject.GetComponent<CharacterVariationSwitcher>() == null)
            {
                // Add the CharacterVariationSwitcher component
                selectedGameObject.AddComponent<CharacterVariationSwitcher>();
            }
            else
            {
                Debug.LogWarning("The selected GameObject already has a CharacterVariationSwitcher component.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject to add the CharacterVariationSwitcher component.");
        }
    }
}
