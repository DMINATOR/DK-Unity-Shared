using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProjectToolsWindow : EditorWindow
{
    public SettingValueData SettingValueData = null;

    Vector2 scrollPos;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Project/Project Tools")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ProjectToolsWindow));
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        OnGUIInputSettings();

        OnProjectSettings();

        EditorGUILayout.EndScrollView();
    }

    void OnGUIInputSettings()
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

    void OnProjectSettings()
    {
        GUILayout.Label("Project Settings", EditorStyles.boldLabel);

        if (SettingValueData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty(nameof(SettingValueData));
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save and Generate Settings"))
            {
                //save values
                AssetLoader.Save(SettingValueData);

                //generate constants as well
                EditorUtility.GenerateClass(new EditorCodeGenerationDefinition()
                {
                    Name = "SettingsConstants",
                    Contents = SettingsConstantsGenerator.Generate(SettingValueData),
                    Path = EditorUtilityConstants.SCRIPTS_FOLDER
                });

            }
        }

        if (GUILayout.Button("Load data"))
        {
            SettingValueData = AssetLoader.Load<SettingValueData>();
        }
    }
}