using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class InputMappingKey
{
    //Display name safe for retrieval
    public string SafeName;
    public string Name;

    public string DescriptiveName;
    public string DescriptiveNegativeName;

    public string NegativeButton;
    public string PositiveButton;

    public string AltNegativeButton;
    public string AltPositiveButton;
}
