using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

public class SettingsConstantsGenerator : ScriptableObject
{
    public SettingValue[] Values = null;

    private SerializedObject serializedObject = null;

    private SerializedProperty property;
    public SerializedProperty Property
    {
        get
        {
            if (Values == null)
            {
                Values = SettingsController.Instance.Values.Values.ToArray();
            }

            if (property == null)
            {
                var target = this;
                serializedObject = new SerializedObject(target);
                property = serializedObject.FindProperty($"{nameof(Values)}");
            }

            return property;
        }
    }

    public void ApplyModifiedProperties ()
    {
        serializedObject.ApplyModifiedProperties();
    }

    //public SerializedProperty GetProperty()
    //{
    //    if (values == null)
    //    {
    //        property = CreateProperty();
    //    }

    //    return property;
    //}

    //private SerializedProperty CreateProperty()
    //{
    //    values = SettingsController.Instance.Values.Values.ToList();

    //    try
    //    {
    //        var target = this;
    //        var so = new SerializedObject(target);
    //        property = so.FindProperty($"{nameof(values)}");

    //        so.ApplyModifiedProperties();
    //    }
    //    catch(Exception ex)
    //    {
    //        throw new System.Exception($"Failed to Create Property {ex.Message}", ex);
    //    }

    //    return property;
    //}

}
