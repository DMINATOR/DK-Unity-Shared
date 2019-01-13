using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingsLoader : MonoBehaviour
{
    public SettingsConstants.Name setting;

    private void OnEnable()
    {
        Load();
    }

    public void Load()
    {
        SettingsController.OnSettingLoad(setting, gameObject);
    }

    public void OnSettingChanged(float value)
    {
        SettingsController.OnSettingChanged(setting, value);
    }

    public void OnSettingChanged(int value)
    {
        SettingsController.OnSettingChanged(setting, value);
    }

    public void OnSettingChanged(string value)
    {
        SettingsController.OnSettingChanged(setting, value);
    }
}


