using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Interface to support Asset Loading
/// </summary>
public interface DKAsset
{
}

public class AssetLoader
{
    private const string FOLDER_PATH = "StreamingAssets";
    private const string EXTENSION = ".json";

    public static T Load<T>() where T : DKAsset, new()
    {
        var filePath = Path.Combine(Application.dataPath, FOLDER_PATH, typeof(T).Name + EXTENSION);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(dataAsJson);
        }
        else
        {
            return new T();
        }
    }

    public static void Save(DKAsset asset)
    {
        string dataAsJson = JsonUtility.ToJson(asset);

        string filePath = Path.Combine(Application.dataPath, FOLDER_PATH);
        string full_name = Path.Combine(filePath, asset.GetType().Name + EXTENSION);

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        File.WriteAllText(full_name, dataAsJson);
    }
}
