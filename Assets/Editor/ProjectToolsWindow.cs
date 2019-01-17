using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProjectToolsWindow : EditorWindow
{
    //SettingsConstantsGenerator generator = null;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Project/Project Tools")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ProjectToolsWindow));
    }

    void OnGUI()
    {
        OnGUIInputSettings();

        OnProjectSettings();
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
        //Debug.Log($"Loaded {UnityEngine.Random.RandomRange(0,100)}");
        GUILayout.Label("Project Settings", EditorStyles.boldLabel);

        //// "target" can be any class derrived from ScriptableObject 
        //// (could be EditorWindow, MonoBehaviour, etc)
        //ScriptableObject target = this;
        //SerializedObject so = new SerializedObject(target);
        //SerializedProperty stringsProperty = so.FindProperty("Strings");

        //var generator = new SettingsConstantsGenerator();

        var generator = CreateInstance<SettingsConstantsGenerator>();

        //ScriptableObject target = generator;
        //SerializedObject so = new SerializedObject(target);
        //SerializedProperty property = so.FindProperty($"{nameof(generator.Values)}");

        EditorGUILayout.PropertyField(generator.Property, true); // True means show children

        generator.ApplyModifiedProperties();

        //so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Generate Setting Constants"))
        {
            //TODO - Add settings generator 
        }
    }
}