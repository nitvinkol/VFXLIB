using UnityEngine;
using UnityEditor;

public class MaterialFixer : EditorWindow
{
    private Material sourceMaterial;
    private Material replacementMaterial;

    [MenuItem("NVK Tools/Material Fixer")]
    static void Init()
    {
        MaterialFixer window = (MaterialFixer)EditorWindow.GetWindow(typeof(MaterialFixer));
        window.minSize = new Vector2(350, 100);
        window.maxSize = new Vector2(350, 100);
        window.Show();
    }


    private void OnGUI()
    {
        GUILayout.Label("Material Fixer", EditorStyles.boldLabel);

        sourceMaterial = (Material)EditorGUILayout.ObjectField("Source Material", sourceMaterial, typeof(Material), false);
        replacementMaterial = (Material)EditorGUILayout.ObjectField("Replacement Material", replacementMaterial, typeof(Material), false);

        if (GUILayout.Button("Fix"))
        {
            if (sourceMaterial != null && replacementMaterial != null)
            {
                ReplaceMaterialsInScene();
            }
            else
            {
                Debug.LogError("Source and Replacement materials must be assigned.");
            }
        }
    }

    private void ReplaceMaterialsInScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        bool sourceMaterialExistsInScene = false;

        foreach (GameObject obj in allObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == sourceMaterial)
                    {
                        sourceMaterialExistsInScene = true;
                        materials[i] = replacementMaterial;
                        renderer.sharedMaterials = materials;
                        EditorUtility.SetDirty(renderer);
                    }
                }
            }
        }

        if (!sourceMaterialExistsInScene)
        {
            Debug.LogError("Source material does not exist in the scene.");
        }
        else
        {
            Debug.Log("Material replacement complete.");
        }
    }
}
