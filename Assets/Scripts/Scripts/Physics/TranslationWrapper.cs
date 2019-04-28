using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wraps translation events and performs them on a game object
/// </summary>
public class TranslationWrapper
{
    MonoBehaviour _behavior;

    ITranslationWrapper _target;

    public TranslationWrapper(MonoBehaviour behavior)
    {
        _behavior = behavior;
        _target = behavior.GetComponent<ITranslationWrapper>();
    }


    public void Translate(float x, float y, float z)
    {
        if( _target != null )
        {
            RotateAndPosition(
                _behavior.transform.position + new Vector3(x,y,z),
                _behavior.transform.rotation
                );
        }
        else
        {
            _behavior.transform.Translate(x, y, z);
        }
    }

    public void Rotate(Vector3 eulers)
    {
        if (_target != null)
        {
            RotateAndPosition(
             _behavior.transform.position,
              Quaternion.Euler(eulers)
             );
        }
        else
        {
            _behavior.transform.Rotate(eulers);
        }
    }

    public void RotateAndPositionInstant(Vector3 position, Quaternion rotation)
    {
        if (_target != null)
        {
            _target.RotateAndPositionInstant(position, rotation);
        }
        else
        {
            _behavior.transform.position = position;
            _behavior.transform.rotation = rotation;
        }
    }

    public void RotateAndPosition(Vector3 position, Quaternion rotation)
    {
        if (_target != null)
        {
            _target.RotateAndPosition(position, rotation);
        }
        else
        {
            _behavior.transform.position = position;
            _behavior.transform.rotation = rotation;
        }
    }
}

public interface ITranslationWrapper
{ 
    void RotateAndPosition(Vector3 position, Quaternion rotation);

    void RotateAndPositionInstant(Vector3 position, Quaternion rotation);
}