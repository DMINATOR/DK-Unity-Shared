using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AffectionReceiver : MonoBehaviour
{
    [Tooltip("Current affection applied to this receiver, to override default behavior")]
    public AffectionDefinitionBase Affection;

    public delegate void AffectionChangedHandler();
    public event AffectionChangedHandler AffectionChanged;


    // Start is called before the first frame update
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

    protected virtual void OnAffectionChanged()
    {
        AffectionChanged?.Invoke();
    }
}
