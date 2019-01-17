using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;


[System.Serializable]
public class AssetLoaderData : DKAsset
{
    public List<SettingValue> Settings;
}

public class AssetLoaderWindow : EditorWindow
{
    public AssetLoaderData gameData;

    [MenuItem("Project/Test/Asset Loader")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(AssetLoaderWindow)).Show();
    }

    void OnGUI()
    {
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                AssetLoader.Save(gameData);
            }
        }

        if (GUILayout.Button("Load data"))
        {
            gameData = AssetLoader.Load<AssetLoaderData>();
        }
    }

}