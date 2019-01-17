﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SettingValueType
{
    String,
    Integer,
    Float,
    Bool
}

/// <summary>
/// Setting values definition
/// </summary>
[System.Serializable]
public class SettingValue
{
    public string Name;

    public SettingValueType Type;
    //public Type type;

    //current value in memory. Save to commit to disk
    public string Value;

    public string DefaultValue;
    public string MinValue;
    public string MaxValue;

    public void Save()
    {
       PlayerPrefs.SetString(Name, Value);
    }

    public void Load()
    {
        Value = PlayerPrefs.GetString(Name, (string)DefaultValue);
    }

    public void Set(object value)
    {
        switch( Type )
        {
            case SettingValueType.Bool:
                Value = Convert.ToBoolean(value).ToString();
                break;

            case SettingValueType.Float:
                Value = Convert.ToDecimal(value).ToString();
                break;

            case SettingValueType.Integer:
                Value = Convert.ToInt32(value).ToString();
                break;

            case SettingValueType.String:
                Value = value.ToString();
                break;

            default:
                throw new Exception($"Cannot convert value {value.ToString()} to {Type}");

        }
    }

    public T Get<T>()
    {
        if (Value == null)
        {
            if (DefaultValue == null)
            {
                return default(T);
            }
            else
            {
                return (T)Convert.ChangeType(DefaultValue, typeof(T));
            }
        }
        else
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}


public class SettingsController
{
    public Dictionary<string, SettingValue> Values = null;

    private static SettingsController instance;
    public static SettingsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SettingsController();
                SettingsConstants.Load(); //load keys
            }

            return instance;
        }
    }

    /// <summary>
    /// Adds value to the setting
    /// </summary>
    /// <param name="value"></param>
    public void AddSetting(SettingValue value)
    {
        if( Values == null )
        {
            Values = new Dictionary<string, SettingValue>();
        }

        if (Values.ContainsKey(value.Name))
        {
            throw new Exception($"Value already exists in settings {value.Name}");
        }
        else
        {
            Values.Add(value.Name, value);
        }
    }

    public void LoadFromDisk()
    {
        if (Values == null)
        {
            SettingsConstants.Load();
        }

        //Load settings from the disk
        foreach (var settingValue in Values.Values)
        {
            settingValue.Load();
        }
    }

    public void SaveToDisk()
    {
        //Save values in cache to the settings
        foreach (var settingValue in Values.Values)
        {
            settingValue.Save();
        }

        //Save the changes to the disk
        PlayerPrefs.Save();
    }

    public T GetValue<T>(string name)
    {
        if (Values == null)
        {
            LoadFromDisk();
        }

        var value = Values[name];

        if (value == null)
        {
            throw new Exception($"Setting not found {name}");
        }
        else
        {
            return Values[name].Get<T>();
        }
    }

    public void SetValue<T>(string name, T value)
    {
        if (Values == null)
        {
            LoadFromDisk();
        }

        if (value == null)
        {
            throw new Exception($"Setting not found {name}");
        }
        else
        {
            Values[name].Set(value);
        }
    }

    public void OnSettingLoad(string name, GameObject gameobj)
    {
        var slider_component = gameobj.GetComponent<Slider>();
        var input_component = gameobj.GetComponent<InputField>();
        var audio_source_components = gameobj.GetComponents<AudioSource>();

        if (slider_component != null)
        {
            slider_component.value = GetValue<float>(name);
        }
        else if (input_component != null)
        {
            input_component.text = GetValue<string>(name);
        }
        else if(audio_source_components != null )
        {
            foreach( AudioSource source in audio_source_components )
            {
                source.volume = GetValue<float>(name);
            }
        }
        else
        {
            throw new Exception($"Unknown component type for: {gameobj.name}");
        }
    }

    public void OnSettingChanged(string name, object value)
    {
        SetValue(name, value);
    }
}

