using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

public class SettingsConstantsGenerator
{
    public static string Generate(SettingValueData data)
    {
        var contents = new StringBuilder();

        //HEADER
        contents.Append(
@"
using System;
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
@"public class SettingsConstants
{
");

        contents.AppendLine(
@"
    public enum Name
    {");

        foreach (var setting in data.Settings)
        {
            contents.AppendLine($"\t\t{setting.Name.Replace(" ","") },");
        }

        contents.AppendLine(
@"
    }");

        //KEY DICTIONARY
        contents.AppendLine(
 @"
    public static void Load()
    {");

        foreach (var setting in data.Settings)
        {

            contents.AppendLine($"\t\tSettingsController.Instance.AddSetting(new SettingValue()");
            contents.AppendLine("\t\t{");
            
            contents.AppendLine($"\t\t\tName = Enum.GetName(typeof(SettingsConstants.Name), Name.{setting.Name.Replace(" ", "")}),");
            contents.AppendLine($"\t\t\tType = SettingValueType.{Enum.GetName(typeof(SettingValueType), setting.Type)},");
            contents.AppendLine($"\t\t\tMinValue = \"{setting.MinValue}\",");
            contents.AppendLine($"\t\t\tDefaultValue = \"{setting.DefaultValue}\",");
            contents.AppendLine($"\t\t\tMaxValue = \"{setting.MaxValue}\"");

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
