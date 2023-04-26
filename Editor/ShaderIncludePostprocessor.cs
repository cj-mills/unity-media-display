using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

// Contains utility classes for creating and managing media displays in Unity.
namespace CJM.MediaDisplay
{
    /// <summary>
    /// A build postprocessor that ensures the specified shader is included in the build.
    /// This script only runs in the Unity Editor.
    /// </summary>
    public class ShaderIncludePostprocessor : IPostprocessBuildWithReport
    {
        /// <summary>
        /// The order in which this postprocessor is called.
        /// </summary>
        public int callbackOrder { get { return 0; } }

        /// <summary>
        /// Called after the build has been completed.
        /// </summary>
        /// <param name="report">The BuildReport object containing information about the build.</param>
        public void OnPostprocessBuild(BuildReport report)
        {
            AddShaderToAlwaysIncludedShaders("Unlit/Texture");
        }

        /// <summary>
        /// Adds the specified shader to the list of always included shaders in GraphicsSettings.
        /// </summary>
        /// <param name="shaderName">The name of the shader to add to the always included shaders list.</param>
        private static void AddShaderToAlwaysIncludedShaders(string shaderName)
        {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
            {
                Debug.LogWarning($"Shader '{shaderName}' not found.");
                return;
            }

            // Load the GraphicsSettings asset
            var graphicsSettings = AssetDatabase.LoadAssetAtPath<GraphicsSettings>("ProjectSettings/GraphicsSettings.asset");
            if (graphicsSettings == null)
            {
                Debug.LogWarning("GraphicsSettings.asset not found.");
                return;
            }

            // Create a serialized object from the GraphicsSettings asset
            SerializedObject serializedGraphicsSettings = new SerializedObject(graphicsSettings);
            // Find the "m_AlwaysIncludedShaders" property in the serialized object
            SerializedProperty alwaysIncludedShadersProp = serializedGraphicsSettings.FindProperty("m_AlwaysIncludedShaders");

            // Iterate through the shaders in the Always Included Shaders list
            for (int i = 0; i < alwaysIncludedShadersProp.arraySize; i++)
            {
                SerializedProperty shaderProp = alwaysIncludedShadersProp.GetArrayElementAtIndex(i);
                if (shaderProp.objectReferenceValue == shader)
                {
                    Debug.Log($"Shader '{shaderName}' is already in the Always Included Shaders list.");
                    return;
                }
            }

            // Add the specified shader to the Always Included Shaders list
            alwaysIncludedShadersProp.InsertArrayElementAtIndex(alwaysIncludedShadersProp.arraySize);
            SerializedProperty newShaderProp = alwaysIncludedShadersProp.GetArrayElementAtIndex(alwaysIncludedShadersProp.arraySize - 1);
            newShaderProp.objectReferenceValue = shader;
            serializedGraphicsSettings.ApplyModifiedProperties();

            Debug.Log($"Shader '{shaderName}' has been added to the Always Included Shaders list.");
        }
    }
}
