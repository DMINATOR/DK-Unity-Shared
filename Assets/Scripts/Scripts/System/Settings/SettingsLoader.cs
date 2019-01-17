using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingsLoader : MonoBehaviour
{
    public SettingsConstants.Name setting;

    public String Name
    {
        get
        {
            return Enum.GetName(typeof(SettingsConstants.Name), setting);
        }
    }

    private void OnEnable()
    {
        Load();
    }

    public void Load()
    {
        SettingsController.Instance.OnSettingLoad(Name, gameObject);
    }

    public void OnSettingChanged(float value)
    {
        SettingsController.Instance.OnSettingChanged(Name, value);
    }

    public void OnSettingChanged(int value)
    {
        SettingsController.Instance.OnSettingChanged(Name, value);
    }

    public void OnSettingChanged(string value)
    {
        SettingsController.Instance.OnSettingChanged(Name, value);
    }
}


