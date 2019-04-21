using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    [Tooltip("Direction vector maximum value")]
    [SerializeField]
    private Vector3 DirectionVectorMax = Vector3.zero;

    [Tooltip("Direction vector minimum value")]
    [SerializeField]
    private Vector3 DirectionVectorMin = Vector3.zero;

    [Tooltip("Current vector direction")]
    [SerializeField]
    [ReadOnly]
    private Vector3 CurrentDirection = Vector3.zero;

    [Tooltip("How frequently direction vector should be changed (seconds)")]
    [SerializeField]
    private float ChangeDirectionTime = 1.0f;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale TimeScaleInstance;

    //Invoker used to generate new direction vector when changes happen
    private TimeControlInvoker _invoker;

    private void Awake()
    {
        _invoker = new TimeControlInvoker(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeScaleInstance = TimeControlController.Instance.CreateTimeScaleInstance(this);
        OnInvokerTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        TimeScaleInstance.Update();

        this.transform.Translate(
                CurrentDirection.x * TimeScaleInstance.TimeScaleDelta,
                CurrentDirection.y * TimeScaleInstance.TimeScaleDelta,
                CurrentDirection.z * TimeScaleInstance.TimeScaleDelta
            );
        
        _invoker.Update();
    }

    void OnInvokerTrigger()
    {
        //calculate new direction vector
        CurrentDirection = new Vector3()
        {
            x = Random.Range(DirectionVectorMin.x, DirectionVectorMax.x),
            y = Random.Range(DirectionVectorMin.y, DirectionVectorMax.y),
            z = Random.Range(DirectionVectorMin.z, DirectionVectorMax.z)
        };

        //set next invoke time
        _invoker.Invoke(OnInvokerTrigger, ChangeDirectionTime);
    }
}
