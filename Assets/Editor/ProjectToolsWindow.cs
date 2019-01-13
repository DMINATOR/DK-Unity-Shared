using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProjectToolsWindow : EditorWindow
{
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Project/Project Tools")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ProjectToolsWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Input Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Input Constants"))
        {
            EditorUtility.GenerateClass(new EditorCodeGenerationDefinition()
            {
                Name = "InputConstants",
                Contents = InputConstantsGenerator.Generate(),
                Path = EditorUtilityConstants.SCRIPTS_FOLDER
            });
        }
    }
}