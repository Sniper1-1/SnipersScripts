using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateHDRPMaterialsFromTextures
{
    private const string HDRP_LIT_SHADER = "HDRP/Lit";
    private const string HDRP_UNLIT_SHADER = "HDRP/Unlit";

    [MenuItem("Assets/Create/Create Material from Image/Lit", false, 20)]
    private static void CreateLitMaterials()
    {
        CreateMaterials(HDRP_LIT_SHADER, "_BaseColorMap");
    }

    [MenuItem("Assets/Create/Create Material from Image/Lit", true)]
    private static bool ValidateCreateLitMaterials()
    {
        return HasSelectedTextures();
    }

    [MenuItem("Assets/Create/Create Material from Image/Unlit", false, 21)]
    private static void CreateUnlitMaterials()
    {
        CreateMaterials(HDRP_UNLIT_SHADER, "_UnlitColorMap");
    }

    [MenuItem("Assets/Create/Create Material from Image/Unlit", true)]
    private static bool ValidateCreateUnlitMaterials()
    {
        return HasSelectedTextures();
    }

    private static bool HasSelectedTextures()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D)
                return true;
        }
        return false;
    }

    private static void CreateMaterials(string shaderName, string baseMapProperty)
    {
        Shader shader = Shader.Find(shaderName);
        if (shader == null)
        {
            UnityEngine.Debug.LogError($"Could not find shader '{shaderName}'. Make sure HDRP is installed and set up in this project.");
            return;
        }

        int createdCount = 0;

        foreach (Object obj in Selection.objects)
        {
            Texture2D texture = obj as Texture2D;
            if (texture == null)
                continue;

            string texturePath = AssetDatabase.GetAssetPath(texture);
            string directory = Path.GetDirectoryName(texturePath);
            string textureName = Path.GetFileNameWithoutExtension(texturePath);

            string materialPath = Path.Combine(directory, textureName + ".mat").Replace("\\", "/");
            materialPath = AssetDatabase.GenerateUniqueAssetPath(materialPath);

            Material material = new Material(shader);

            if (material.HasProperty(baseMapProperty))
            {
                material.SetTexture(baseMapProperty, texture);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Shader '{shaderName}' has no property '{baseMapProperty}'. Texture not assigned for {textureName}.");
            }

            AssetDatabase.CreateAsset(material, materialPath);
            createdCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        UnityEngine.Debug.Log($"Created {createdCount} HDRP material(s) using shader '{shaderName}'.");
    }
}