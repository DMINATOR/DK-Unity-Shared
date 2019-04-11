using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for implementing Affection based classes that should apply their properties to other objects
/// </summary>
public class AffectionDefinitionBase : MonoBehaviour
{
    [Tooltip("Name of this affection")]
    public string Name;

    public delegate void AffectionChangedHandler();
    public event AffectionChangedHandler AffectionChanged;

    /// <summary>
    /// OnValidate is only triggered by unity in the editor, sent events to all the listeners
    /// </summary>
    public void OnValidate()
    {
        AffectionChanged?.Invoke();
    }
}
