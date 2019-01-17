using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class AssetLoaderData
{
    public List<SettingValue> Settings;
}

public class AssetLoaderWindow : EditorWindow
{
    public AssetLoaderData gameData;

    private string gameDataProjectFilePath = "StreamingAssets";
    private string gameDataFileName = "data.json";

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
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine( Application.dataPath, gameDataProjectFilePath, gameDataFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<AssetLoaderData>(dataAsJson);
        }
        else
        {
            gameData = new AssetLoaderData();
        }
    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Path.Combine( Application.dataPath , gameDataProjectFilePath );
        string full_name = Path.Combine(filePath, gameDataFileName);

        if( !Directory.Exists( filePath ))
        {
            Directory.CreateDirectory(filePath);
        }

        File.WriteAllText(full_name, dataAsJson);
    }
}