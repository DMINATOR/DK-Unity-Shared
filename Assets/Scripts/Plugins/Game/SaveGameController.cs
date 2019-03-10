using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all save game data
/// </summary>
[System.Serializable]
public partial class SaveGameData : DKAsset
{
    public int BaseField;
}

public class SaveGameController
{
    private static SaveGameController _instance;
    public static SaveGameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveGameController();
            }
            //else - instance created, re-use

            return _instance;
        }
    }

    public SaveGameData Load()
    {
        return AssetLoader.Load<SaveGameData>();
    }

    public void Save(SaveGameData data)
    {
        AssetLoader.Save(data);
    }
}
