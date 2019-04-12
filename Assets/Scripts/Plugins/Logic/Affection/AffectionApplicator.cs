using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Affection Applicator applies properties of AffectionDefinition
/// </summary>
public class AffectionApplicator : MonoBehaviour
{
    //Exposed to Editor

    [Tooltip("Current affection applied to this receiver, to override default behavior")]
    [SerializeField]
    private AffectionDefinitionBase Affection = null;

    //Exposed to other classes

    //Internal

    /// <summary>
    /// Receivers that are receiving effect from this applicator
    /// </summary>
    private HashSet<AffectionReceiver> Receivers = new HashSet<AffectionReceiver>();

    protected void Start()
    {
        RegisterEvents();

        OnAffectionChanged();
    }

    protected void OnDestroy()
    {
        UnregisterEvents();
    }

    protected void RegisterEvents()
    {
        if (Affection != null)
        {
            Affection.AffectionChanged += OnAffectionChanged;
        }
    }

    protected void UnregisterEvents()
    {
        if (Affection != null)
        {
            Affection.AffectionChanged -= OnAffectionChanged;
        }
    }

    public virtual void OnAffectionChanged()
    {
        foreach( var receiver in Receivers )
        {
            receiver.OnAffectionChanged();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var receiver = other.gameObject.GetComponent<AffectionReceiver>();

        if( receiver != null )
        {
            Receivers.Add(receiver);
            receiver.AffectionFromApplicator = Affection;
            receiver.OnAffectionChanged();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var receiver = other.gameObject.GetComponent<AffectionReceiver>();

        if (receiver != null)
        {
            receiver.AffectionFromApplicator = null;
            Receivers.Remove(receiver);
            receiver.OnAffectionChanged();
        }
    }
}
