using UnityEditor;
using UnityEngine;

namespace CJM.MediaDisplay
{
    public class DependencyDefineSymbolAdder
    {
        // Constant string representing the custom define symbol.
        private const string CustomDefineSymbol = "CJM_UNITY_MEDIA_DISPLAY";

        // This method is called on Unity editor load to ensure the custom define symbol is added.
        [InitializeOnLoadMethod]
        public static void AddCustomDefineSymbol()
        {
            // Get the currently selected build target group.
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            // Retrieve the current scripting define symbols for the selected build target group.
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            // Check if the custom define symbol is already in the list of define symbols.
            if (!defines.Contains(CustomDefineSymbol))
            {
                // Append the custom define symbol to the list of define symbols.
                defines += $";{CustomDefineSymbol}";

                // Set the updated list of define symbols for the selected build target group.
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);

                // Log the successful addition of the custom define symbol.
                Debug.Log($"Added custom define symbol '{CustomDefineSymbol}' to the project.");
            }
        }
    }
}
