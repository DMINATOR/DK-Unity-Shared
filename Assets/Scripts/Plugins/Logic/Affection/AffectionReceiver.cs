using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AffectionReceiver : MonoBehaviour
{
    //Exposed to Editor

    //Exposed to other classes

    [Tooltip("Current affection applied to this receiver, to override default behavior")]
    public AffectionDefinitionBase Affection;

    [Tooltip("Current affection applied to this receiver from Applicator, to override default behavior")]
    public AffectionDefinitionBase AffectionFromApplicator;

    public delegate void AffectionChangedHandler();
    public event AffectionChangedHandler AffectionChanged;

    //Internal


    // Start is called before the first frame update
    protected void Start()
    {
        RegisterEvents();

        OnAffectionChanged();

        OnStart();
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

    public virtual void OnStart()
    {

    }

    public virtual void OnAffectionChanged()
    {
        AffectionChanged?.Invoke();
    }
}
