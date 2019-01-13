using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Definition for the code to be generated
/// </summary>
public class EditorCodeGenerationDefinition
{
    public string Path;

    public string Name;

    public string Contents;
}

public class EditorUtilityConstants
{
    public const string SCRIPTS_FOLDER = "Assets/Scripts/Generated/";
}


/// <summary>
/// Utility to use for EDITOR code
/// </summary>
public class EditorUtility : MonoBehaviour
{
    public static void GenerateClass(EditorCodeGenerationDefinition classDefinition)
    {
        var dir = new DirectoryInfo(classDefinition.Path);
        if( !dir.Exists )
        {
            dir.Create();
        }

        string fullPath = classDefinition.Path + classDefinition.Name + ".cs";
        if ( File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }


        Debug.Log($"Creating Class: {fullPath}");

        File.WriteAllText(fullPath, classDefinition.Contents);

        Debug.Log($"Class: {fullPath} generated");

        AssetDatabase.Refresh();
    }
}