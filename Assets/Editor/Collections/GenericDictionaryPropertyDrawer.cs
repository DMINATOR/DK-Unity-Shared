using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Draws the generic dictionary a bit nicer than Unity would natively (not as many expand-arrows
/// and better spacing between KeyValue pairs). Also renders a warning-box if there are duplicate
/// keys in the dictionary.
/// </summary>
[CustomPropertyDrawer(typeof(GenericDictionary<,>))]
public class GenericDictionaryPropertyDrawer : PropertyDrawer
{
    static float lineHeight = EditorGUIUtility.singleLineHeight;
    static float vertSpace = EditorGUIUtility.standardVerticalSpacing;
    static float combinedPadding = lineHeight + vertSpace;

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        // Render list header and expand arrow.
        var list = property.FindPropertyRelative("list");
        var headerPos = new Rect(lineHeight, pos.y, pos.width, lineHeight);
        string fieldName = ObjectNames.NicifyVariableName(fieldInfo.Name);
        EditorGUI.PropertyField(headerPos, list, new GUIContent(fieldName));

        // Render list if expanded.
        var currentPos = new Rect(lineHeight, pos.y, pos.width, lineHeight);
        if (list.isExpanded)
        {
            // Render list size first.
            list.NextVisible(true);
            EditorGUI.indentLevel++;
            currentPos = new Rect(headerPos.x, headerPos.y + combinedPadding, pos.width, lineHeight);
            EditorGUI.PropertyField(currentPos, list, new GUIContent("Size"));

            // Render list content.
            currentPos.y += vertSpace;
            while (true)
            {
                if (list.name == "Key" || list.name == "Value")
                {
                    // Render key or value.
                    var entryPos = new Rect(currentPos.x, currentPos.y + combinedPadding, pos.width, lineHeight);

                    if (list.hasChildren)
                    {
                        EditorGUI.PropertyField(entryPos, list, new GUIContent($"{list.name} ({list.type})"), list.hasChildren);

                        if (list.isExpanded)
                        {
                            currentPos.y += EditorGUI.GetPropertyHeight(list);
                        }
                        else
                        {
                            currentPos.y += combinedPadding;
                        }
                    }
                    else
                    {
                        EditorGUI.PropertyField(entryPos, list, new GUIContent(list.name));
                        currentPos.y += combinedPadding;
                    }

                    // Add spacing after each key value pair.
                    if (list.name == "Value")
                    {
                        currentPos.y += vertSpace;
                    }
                }
                if (!list.NextVisible(true)) break;
            }
        }

        // If there are key collisions render warning box.
        bool keyCollision = property.FindPropertyRelative("keyCollision").boolValue;
        if (keyCollision)
        {
            var entryPos = new Rect(lineHeight, currentPos.y + combinedPadding, pos.width, lineHeight * 2f);
            EditorGUI.HelpBox(entryPos, "There are duplicate keys in the dictionary." +
                " Duplicate keys will not be serialized.", MessageType.Warning);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totHeight = 0f;

        // If there is a key collision account for height of warning box.
        bool keyCollision = property.FindPropertyRelative("keyCollision").boolValue;
        if (keyCollision)
        {
            totHeight += lineHeight * 2f + vertSpace;
        }

        // Return height of KeyValue list (take into account if list is expanded or not).
        var list = property.FindPropertyRelative("list");
        if (list.isExpanded)
        {
            totHeight += combinedPadding;
            while (true)
            {
                if (list.name == "Key" || list.name == "Value")
                {
                    if (list.hasChildren)
                    {
                        if (list.isExpanded)
                        {
                            totHeight += EditorGUI.GetPropertyHeight(list);
                        }
                        else
                        {
                            totHeight += combinedPadding;
                        }
                    }
                    else
                    {
                        totHeight += combinedPadding;
                    }

                    // Add spacing after each key value pair.
                    if (list.name == "Value")
                    {
                        totHeight += vertSpace;
                    }
                }
                if (!list.NextVisible(true)) break;
            }

            return totHeight + lineHeight;
        }
        else
        {
            return totHeight + lineHeight;
        }
    }
}