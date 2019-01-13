using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputMapping;

public class InputController
{
    public Dictionary<InputMappingKeyName, List<InputMappingKey>> KeysMapping { private set; get; } = new Dictionary<InputMappingKeyName, List<InputMappingKey>>();

    private static InputController instance;
    public static InputController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputController();
                InputMapping.Load(); //load keys
            }

            return instance;
        }
    }

    public List<InputMappingKey> GetActualButtons(InputMappingKeyName name)
    {
        List<InputMappingKey> keys;

        if (KeysMapping.TryGetValue(name, out keys))
        {
            return keys;
        }
        else
        {
            throw new System.Exception($"Specified key is not in Input settings:{ name}");
        }
    }

    public void Add(InputMappingKeyName key, List<InputMappingKey> values)
    {
        KeysMapping.Add(key, values);
    }
}


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

            foreach (InputMappingKey key in Keys)
            {
                var buttons = new[] { key.PositiveButton, key.NegativeButton, key.AltPositiveButton, key.AltNegativeButton };

                foreach (string button in buttons)
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
            return string.Join(", ", ButtonNames);
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
                keys = InputController.Instance.GetActualButtons(InputKey);
            }

            return keys;
        }
    }
}


