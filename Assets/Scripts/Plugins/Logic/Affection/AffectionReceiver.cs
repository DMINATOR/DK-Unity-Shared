using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectionReceiver : MonoBehaviour
{
    [Tooltip("Current affection applied to this receiver, to override default behavior")]
    public AffectionDefinitionBase Affection;

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

    protected virtual void OnAffectionChanged() { }
}
