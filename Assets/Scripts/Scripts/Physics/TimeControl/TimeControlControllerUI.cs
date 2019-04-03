using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeControlControllerUI : MonoBehaviour
{
    [Header("Real Time Control")]

    [Tooltip("Scale of time across all game objects")]
    [SerializeField]
    [Range(-5f, 5f)]
    float _timeScale = 1.0f;

    [Tooltip("Instance of TimeControl object")]
    public TimeControlController Instance;

    [Tooltip("Event to call when triggered")]
    public UnityEvent EventOnTimeScaleChangedCallback;

    // Start is called before the first frame update
    void Start()
    {
        Instance = TimeControlController.Instance;
        Instance.TimeScaleChanged += Instance_TimeScaleChanged;
    }

    private void Instance_TimeScaleChanged()
    {
        EventOnTimeScaleChangedCallback.Invoke();
    }

    private void OnDestroy()
    {
        Instance.TimeScaleChanged -= Instance_TimeScaleChanged;
    }

    void OnValidate()
    {
        //update properties
        Instance.TimeScale = _timeScale;
    }
}
