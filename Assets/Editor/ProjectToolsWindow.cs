using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProjectToolsWindow : EditorWindow
{
    public bool Loaded = false;

    public SettingValueData SettingValueData = null;

    public LogConfigurationData LogConfigurationData = null;

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

        SerializedObject serializedObject = new SerializedObject(this);

        OnInitialLoad();

        OnProjectSettings(serializedObject);

        OnDebugSettings(serializedObject);

        serializedObject.ApplyModifiedProperties();

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


    void OnInitialLoad()
    {
        if (!Loaded)
        {
            SettingValueData = AssetLoader.Load<SettingValueData>();
            LogConfigurationData = AssetLoader.Load<LogConfigurationData>();
            Loaded = true;
        }
    }

    void OnProjectSettings(SerializedObject serializedObject)
    {
        GUILayout.Label("Project Settings", EditorStyles.boldLabel);

        if (this.SettingValueData != null)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(nameof(SettingValueData));
            EditorGUILayout.PropertyField(serializedProperty, true);

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
    }

    void OnDebugSettings(SerializedObject serializedObject)
    {
        GUILayout.Label("Debug Settings", EditorStyles.boldLabel);

        if (LogConfigurationData != null)
        {
            //SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty(nameof(LogConfigurationData));
            EditorGUILayout.PropertyField(serializedProperty, true);

            //serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save"))
            {
                //save values
                AssetLoader.Save(LogConfigurationData);
                Log.Instance.Load(); //reload again
            }
        }

        if (GUILayout.Button("Load Defaults"))
        {
            LogConfigurationData = Log.GetDefaults();
        }

        //Buttons to debug different logging methods:
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Log - Test Info"))
        {
            Log.Instance.Info("test", "Test Info");
        }

        if (GUILayout.Button("Log - Test Warning"))
        {
            Log.Instance.Warning("test", "Test Warning");
        }

        if (GUILayout.Button("Log - Test Error"))
        {
            Log.Instance.Error("test", "Test Error");
        }

        GUILayout.EndHorizontal();
    }
}