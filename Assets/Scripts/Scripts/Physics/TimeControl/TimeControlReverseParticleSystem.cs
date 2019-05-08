using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on http://www.mirzabeig.com/tutorials/rewinding-particle-systems/
/// </summary>
public class TimeControlReverseParticleSystem : MonoBehaviour
{

    ParticleSystem[] particleSystems;

    float[] simulationTimes;

    //public float startTime = 2.0f;
    //public float simulationSpeedScale = 1.0f;

    [Range(0f, 10f)]
    public float currentSimulationTime;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale _instance;

    void Initialize()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(false);
        simulationTimes = new float[particleSystems.Length];
    }

    private void Start()
    {
        _instance = TimeControlController.Instance.CreateTimeScaleInstance(this);
    }

    void OnEnable()
    {
        if (particleSystems == null)
        {
            Initialize();
        }

        for (int i = 0; i < simulationTimes.Length; i++)
        {
            simulationTimes[i] = 0.0f;
        }

        particleSystems[0].Simulate(0.0f, true, false, true);
    }
    void Update()
    {
        _instance.Update();

        particleSystems[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        for (int i = particleSystems.Length - 1; i >= 0; i--)
        {
            bool useAutoRandomSeed = particleSystems[i].useAutoRandomSeed;
            particleSystems[i].useAutoRandomSeed = false;

            particleSystems[i].Play(false);

            simulationTimes[i] += (_instance.TimeScaleDeltaRaw * particleSystems[i].main.simulationSpeed);

            currentSimulationTime = simulationTimes[i];
            particleSystems[i].Simulate(currentSimulationTime, false, false, true);

            Log.Instance.Info("Particles", $"Time = {currentSimulationTime}");

            particleSystems[i].useAutoRandomSeed = useAutoRandomSeed;

            if (currentSimulationTime < 0.0f)
            {
                if (particleSystems[i].main.loop)
                {
                    //reset back to max duration
                    simulationTimes[i] = particleSystems[i].main.duration;
                }
                else
                {
                    //stop
                    particleSystems[i].Play(false);
                    particleSystems[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
            else if( currentSimulationTime > particleSystems[i].main.duration )
            {
                if( particleSystems[i].main.loop)
                {
                    //reset back to 0
                    simulationTimes[i] = 0;
                }
                else
                {
                    //stop
                    particleSystems[i].Play(false);
                    particleSystems[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }
}
