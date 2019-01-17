
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Default
public class SettingsConstants
{

    public enum Name
    {
		MusicVolume,
		SoundVolume,

    }

    public static void Load()
    {
		SettingsController.Instance.AddSetting(new SettingValue()
		{
			Name = Enum.GetName(typeof(SettingsConstants.Name), Name.MusicVolume),
			Type = SettingValueType.Float,
			MinValue = "0.0f",
			DefaultValue = "0.5f",
			MaxValue = "1.0f"
		});
		SettingsController.Instance.AddSetting(new SettingValue()
		{
			Name = Enum.GetName(typeof(SettingsConstants.Name), Name.SoundVolume),
			Type = SettingValueType.Float,
			MinValue = "0.0f",
			DefaultValue = "0.7f",
			MaxValue = "1.0f"
		});

    }
}
