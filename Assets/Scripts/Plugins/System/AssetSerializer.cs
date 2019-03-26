using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Serializable]
public class DateTimeSerializer : ISerializationCallbackReceiver
{
    public static implicit operator DateTime(DateTimeSerializer date)
    {
        return (date.Value);
    }

    public static implicit operator DateTimeSerializer(DateTime date)
    {
        return new DateTimeSerializer() { Value = date };
    }

    [HideInInspector]
    [SerializeField]
    private string _dateTimeValue;

    [HideInInspector]
    public DateTime Value;

    public void OnBeforeSerialize()
    {
        _dateTimeValue = Value.ToString(CultureInfo.InvariantCulture);
    }

    public void OnAfterDeserialize()
    {
        DateTime.TryParse(_dateTimeValue,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out Value);
    }
}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DateTimeSerializer))]
public class IngredientDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect amountRect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative( "_dateTimeValue"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif
