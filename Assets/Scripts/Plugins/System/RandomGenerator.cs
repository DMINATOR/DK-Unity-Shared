using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Using 
/// https://en.wikipedia.org/wiki/Linear_congruential_generator#cite_note-LEcuyer99-8
/// https://towardsdatascience.com/how-to-generate-random-variables-from-scratch-no-library-used-4b71eb3c8dc7
/// </summary>
public class RandomGenerator
{
    private float _mult;
    private float _mod;
    private int _seed;
    private const int _minSeed = 10000;

    public RandomGenerator(int mult, int mod, int seed)
    {
        _mult = mult;
        _mod = mod;
        _seed = seed;
    }

    public RandomGenerator(int seed)
    {
        _mult = 1664525;
        _mod = Mathf.Pow(2f,31f) - 1;
        _seed = seed + _minSeed;
    }

    private float GetNumber(float seed)
    {
        return ((seed * _mult + 1) % _mod) / _mod;
    }

    public int GetNext(int high, int low)
    {
        var rawNumber = GetNumber(_seed);
        _seed++;
        return low + (int)((high - low) * rawNumber);
    }

    private void Offset(int seed)
    {
        _seed = seed + _minSeed;
    }
}

public class RandomGeneratorTests
{
    public static void TestGetOneNumber()
    {
        var generator = new RandomGenerator(0);

        int val1 = generator.GetNext(0, 100);
        int val2 = generator.GetNext(0, 100);
        int val3 = generator.GetNext(0, 100);
        int val4 = generator.GetNext(0, 100);
        int val5 = generator.GetNext(0, 100);
        int val6 = generator.GetNext(0, 100);

        if( val1 != 25)
        {
            throw new System.Exception($"invalid number {val1}");
        }

        if (val2 != 1)
        {
            throw new System.Exception($"invalid number {val2}");
        }

        if (val3 != 1)
        {
            throw new System.Exception($"invalid number {val3}");
        }

        if (val4 != 1)
        {
            throw new System.Exception($"invalid number {val4}");
        }

        if (val5 != 1)
        {
            throw new System.Exception($"invalid number {val5}");
        }

        if (val6 != 1)
        {
            throw new System.Exception($"invalid number {val6}");
        }
    }
}