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
    public static void Load()
    {");

        foreach (var key in mappingKeys.Keys)
        {

            contents.AppendLine($"\t\tInputController.Instance.Add(InputMappingKeyName.{key}, new List<InputMappingKey>()");
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

        return contents.ToString();
    }
}
