using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Setting values definition
/// </summary>
public class SettingValue
{
    public SettingsConstants.Name Name;
    public Type type;

    //current value in memory. Save to commit to disk
    public object Value;

    public object DefaultValue;
    public object MinValue;
    public object MaxValue;

    public void Save()
    {
        if (type == typeof(int))
        {
            PlayerPrefs.SetInt(Enum.GetName(typeof(SettingsConstants.Name), Name), (int)Convert.ChangeType(Value, typeof(int)));
        }
        else
        if (type == typeof(string))
        {
            PlayerPrefs.SetString(Enum.GetName(typeof(SettingsConstants.Name), Name), (string)Convert.ChangeType(Value, typeof(string)));
        }
        else
        if (type == typeof(float))
        {
            PlayerPrefs.SetFloat(Enum.GetName(typeof(SettingsConstants.Name), Name), (float)Convert.ChangeType(Value, typeof(float)));
        }
        else
        if (type == typeof(bool))
        {
            PlayerPrefs.SetInt(Enum.GetName(typeof(SettingsConstants.Name), Name), (int)Convert.ChangeType(Value, typeof(int)));
        }
        else
        {
            throw new Exception($"Unknown setting type {type.Name}");
        }
    }

    public void Load()
    {
        if (type == typeof(int))
        {
            Value = Convert.ChangeType(PlayerPrefs.GetInt(Enum.GetName(typeof(SettingsConstants.Name), Name), DefaultValue == null ? int.MinValue : (int)DefaultValue), type);
        }
        else
        if (type == typeof(string))
        {
            Value = Convert.ChangeType(PlayerPrefs.GetString(Enum.GetName(typeof(SettingsConstants.Name), Name), (string)DefaultValue), type);
        }
        else
        if (type == typeof(float))
        {
            Value = Convert.ChangeType(PlayerPrefs.GetFloat(Enum.GetName(typeof(SettingsConstants.Name), Name), DefaultValue == null ? float.MinValue : (float)DefaultValue), type);
        }
        else
        if (type == typeof(bool))
        {
            Value = Convert.ChangeType(PlayerPrefs.GetInt(Enum.GetName(typeof(SettingsConstants.Name), Name), DefaultValue == null ? int.MinValue : (int)DefaultValue), type);
        }
        else
        {
            throw new Exception($"Unknown setting type {type.Name}");
        }
    }

    public void Set(object value)
    {
        Value = value;
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


public static class SettingsController
{
    private static Dictionary<SettingsConstants.Name, SettingValue> values = null;

    /// <summary>
    /// Adds value to the setting
    /// </summary>
    /// <param name="value"></param>
    public static void AddSetting(SettingValue value)
    {
        if( values == null )
        {
            values = new Dictionary<SettingsConstants.Name, SettingValue>();
        }

        if (values.ContainsKey(value.Name))
        {
            throw new Exception($"Value already exists in settings {value.Name}");
        }
        else
        {
            values.Add(value.Name, value);
        }
    }

    public static void LoadFromDisk()
    {
        if (values == null)
        {
            SettingsConstants.Load();
        }

        //Load settings from the disk
        foreach (var settingValue in values.Values)
        {
            settingValue.Load();
        }
    }

    public static void SaveToDisk()
    {
        //Save values in cache to the settings
        foreach (var settingValue in values.Values)
        {
            settingValue.Save();
        }

        //Save the changes to the disk
        PlayerPrefs.Save();
    }

    public static T GetValue<T>(SettingsConstants.Name setting)
    {
        if (values == null)
        {
            LoadFromDisk();
        }

        var value = values[setting];

        if (value == null)
        {
            throw new Exception($"Setting not found {Enum.GetName(typeof(SettingsConstants.Name), setting)}");
        }
        else
        {
            return values[setting].Get<T>();
        }
    }

    public static void SetValue<T>(SettingsConstants.Name setting, T value)
    {
        if (values == null)
        {
            LoadFromDisk();
        }

        if (value == null)
        {
            throw new Exception($"Setting not found {Enum.GetName(typeof(SettingsConstants.Name), setting)}");
        }
        else
        {
            values[setting].Set(value);
        }
    }

    public static void OnSettingLoad(SettingsConstants.Name setting, GameObject gameobj)
    {
        var slider_component = gameobj.GetComponent<Slider>();
        var input_component = gameobj.GetComponent<InputField>();
        var audio_source_components = gameobj.GetComponents<AudioSource>();

        if (slider_component != null)
        {
            slider_component.value = GetValue<float>(setting);
        }
        else if (input_component != null)
        {
            input_component.text = GetValue<string>(setting);
        }
        else if(audio_source_components != null )
        {
            foreach( AudioSource source in audio_source_components )
            {
                source.volume = GetValue<float>(setting);
            }
        }
        else
        {
            throw new Exception($"Unknown component type for: {gameobj.name}");
        }
    }

    public static void OnSettingChanged(SettingsConstants.Name setting, object value)
    {
        SetValue(setting, value);
    }
}

