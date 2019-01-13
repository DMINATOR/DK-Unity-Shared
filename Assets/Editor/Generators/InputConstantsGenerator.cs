using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class InputConstantsGenerator
{
    private static InputMappingKey CreateFromSerializedProperty(SerializedProperty property)
    {
        InputMappingKey key = new InputMappingKey();

        //axis.displayName	"Name"	string
        key.Name = property.displayName;
        key.SafeName = key.Name.Replace(" ", "");

        //axis.displayName	"Descriptive Name"	string
        property.Next(true);
        property.Next(false); //skip one
        key.DescriptiveName = property.stringValue;

        //axis.displayName	"Descriptive Negative Name"	string
        property.Next(false);
        key.DescriptiveNegativeName = property.stringValue;

        //axis.displayName	"Negative Button"	string
        property.Next(false);
        key.NegativeButton = property.stringValue;

        //axis.displayName	"Positive Button"	string
        property.Next(false);
        key.PositiveButton = property.stringValue;

        //axis.displayName	"Alt Negative Button"	string
        property.Next(false);
        key.AltNegativeButton = property.stringValue;

        //axis.displayName	"Alt Positive Button"	string
        property.Next(false);
        key.AltPositiveButton = property.stringValue;

        return key;
    }

    // Start is called before the first frame update
    public static string Generate()
    {
        var contents = new StringBuilder();
        var mappingKeys = new Dictionary<string, List<InputMappingKey>>();

        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        SerializedObject obj = new SerializedObject(inputManager);
        SerializedProperty axisArray = obj.FindProperty("m_Axes");
        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");

        for (int i = 0; i < axisArray.arraySize; ++i)
        {
            var key = CreateFromSerializedProperty(axisArray.GetArrayElementAtIndex(i));

            if (!mappingKeys.ContainsKey(key.SafeName))
            {
                mappingKeys[key.SafeName] = new List<InputMappingKey>();
            }

            mappingKeys[key.SafeName].Add(key);
        }

        //HEADER
        contents.Append(
@"
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

");

        //CLASS START
        contents.AppendLine(
$"//Generated on: {DateTime.Now.ToString()}");


        //ENUM
        contents.Append(
@"public class InputMapping
{
");

        contents.AppendLine(
@"
    public enum InputMappingKeyName
    {");

        foreach (var name in mappingKeys.Keys)
        {
            contents.AppendLine($"\t\t{name},");
        }

        contents.AppendLine(
@"
    }");

        //KEY DICTIONARY
        contents.AppendLine(
 @"
    private static InputMapping instance;
    public static InputMapping Instance
    {
        get
        {
            if( instance == null )
            {
                instance = new InputMapping();
            }

            return instance;
        }
    }

    public List<InputMappingKey> GetActualButtons(InputMappingKeyName name)
    {
        List<InputMappingKey> keys;

        if ( KeysMapping.TryGetValue(name, out keys) )
        {
            return keys;
        }
        else
        {
            throw new System.Exception($""Specified key is not in Input settings:{ name}"");
        }
    }

    public Dictionary<InputMappingKeyName, List<InputMappingKey>> KeysMapping { private set; get; } = new Dictionary<InputMappingKeyName, List<InputMappingKey>>();

    public InputMapping()
    {");

        foreach (var key in mappingKeys.Keys)
        {

            contents.AppendLine($"\t\tKeysMapping.Add(InputMappingKeyName.{key}, new List<InputMappingKey>()");
            contents.AppendLine("\t\t{");

            for (var i = 0; i < mappingKeys[key].Count; i++)
            {
                var button = mappingKeys[key][i];

                if (i > 0)
                {
                    contents.AppendLine("\t\t\t,");
                }

                contents.AppendLine("\t\t\tnew InputMappingKey(){");

                contents.AppendLine($"\t\t\t Name = \"{button.Name}\", DescriptiveName = \"{button.DescriptiveName}\", DescriptiveNegativeName = \"{button.DescriptiveNegativeName}\", NegativeButton = \"{button.NegativeButton}\", PositiveButton = \"{button.PositiveButton}\", AltNegativeButton = \"{button.AltNegativeButton}\", AltPositiveButton = \"{button.AltPositiveButton}\"");

                contents.AppendLine("\t\t\t}");
            }

            contents.AppendLine("\t\t});");
        }

        //CLASS END
        contents.AppendLine(
@"
    }");

        //End of class
        contents.AppendLine("}");

        contents.AppendLine(
@"

/// <summary>
/// Wrapper to define a key and find actual buttons
/// </summary>
[System.Serializable]
public class InputButton
{
    //Key to use for the input
    public InputMapping.InputMappingKeyName InputKey;

    //Get name from the first key
    public string KeyName
    {
        get
        {
            return Keys[0].Name;
        }
    }

    /// <summary>
    /// Returns all button names that have been set
    /// </summary>
    public List<string> ButtonNames
    {
        get
        {
            List<string> buttonNames = new List<string>();

            foreach(InputMappingKey key in Keys)
            {
                var buttons = new []{ key.PositiveButton, key.NegativeButton, key.AltPositiveButton, key.AltNegativeButton };

                foreach(string button in buttons)
                {
                    if (!string.IsNullOrEmpty(button))
                    {
                        buttonNames.Add(button);
                    }
                }
            }

            return buttonNames;
        }
    }

    public string ButtonNamesCombined
    {
        get
        {
            return string.Join("", "", ButtonNames);
        }
    }

    //Keys that would actually be used
    private List<InputMappingKey> keys;
    public List<InputMappingKey> Keys
    {
        get
        {
            if (keys == null)
            {
                keys = InputMapping.Instance.GetActualButtons(InputKey);
            }

            return keys;
        }
    }   
}
");
        return contents.ToString();
    }
}
