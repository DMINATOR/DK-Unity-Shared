using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaveGameWindow : EditorWindow
{
    public bool Loaded = false;

    public SaveGameData SaveGameData = null;

    Vector2 _scrollPos;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Project/Save Game")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SaveGameWindow));
    }

    void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);

        SerializedObject serializedObject = new SerializedObject(this);

        OnInitialLoad();

        OnSaveGameData(serializedObject);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();
    }

    void OnInitialLoad()
    {
        if (!Loaded)
        {
            SaveGameData = SaveGameController.Instance.Load();
            Loaded = true;
        }
    }

    void OnSaveGameData(SerializedObject serializedObject)
    {
        GUILayout.Label("Save Game Data", EditorStyles.boldLabel);

        if (SaveGameData != null)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(nameof(SaveGameData));
            EditorGUILayout.PropertyField(serializedProperty, true);

            if (GUILayout.Button("Save"))
            {
                //save values
                SaveGameController.Instance.Save(SaveGameData);
            }
        }

        if (GUILayout.Button("Load"))
        {
            SaveGameData = SaveGameController.Instance.Load();
        }

    }
}