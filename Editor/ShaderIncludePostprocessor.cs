#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class ShaderIncludePostprocessor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        AddShaderToAlwaysIncludedShaders("Unlit/Texture");
    }

    private static void AddShaderToAlwaysIncludedShaders(string shaderName)
    {
        Shader shader = Shader.Find(shaderName);
        if (shader == null)
        {
            Debug.LogWarning($"Shader '{shaderName}' not found.");
            return;
        }

        var graphicsSettings = AssetDatabase.LoadAssetAtPath<GraphicsSettings>("ProjectSettings/GraphicsSettings.asset");
        if (graphicsSettings == null)
        {
            Debug.LogWarning("GraphicsSettings.asset not found.");
            return;
        }

        SerializedObject serializedGraphicsSettings = new SerializedObject(graphicsSettings);
        SerializedProperty alwaysIncludedShadersProp = serializedGraphicsSettings.FindProperty("m_AlwaysIncludedShaders");

        for (int i = 0; i < alwaysIncludedShadersProp.arraySize; i++)
        {
            SerializedProperty shaderProp = alwaysIncludedShadersProp.GetArrayElementAtIndex(i);
            if (shaderProp.objectReferenceValue == shader)
            {
                Debug.Log($"Shader '{shaderName}' is already in the Always Included Shaders list.");
                return;
            }
        }

        alwaysIncludedShadersProp.InsertArrayElementAtIndex(alwaysIncludedShadersProp.arraySize);
        SerializedProperty newShaderProp = alwaysIncludedShadersProp.GetArrayElementAtIndex(alwaysIncludedShadersProp.arraySize - 1);
        newShaderProp.objectReferenceValue = shader;
        serializedGraphicsSettings.ApplyModifiedProperties();

        Debug.Log($"Shader '{shaderName}' has been added to the Always Included Shaders list.");
    }
}
#endif
