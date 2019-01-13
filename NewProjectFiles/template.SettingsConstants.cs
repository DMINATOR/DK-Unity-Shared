using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Default settings for new projects
public class SettingsConstants
{
    public enum Name
    {
        SoundVolume,
        MusicVolume
    }

    public static void Load()
    {
        SettingsController.AddSetting(new SettingValue()
        {
            Name = Name.MusicVolume,
            type = typeof(float),
            MinValue = 0.0f,
            DefaultValue = 0.5f,
            MaxValue = 1.0f
        });

        SettingsController.AddSetting(new SettingValue()
        {
            Name = Name.SoundVolume,
            type = typeof(float),
            MinValue = 0.0f,
            DefaultValue = 0.7f,
            MaxValue = 1.0f
        });
    }
}
