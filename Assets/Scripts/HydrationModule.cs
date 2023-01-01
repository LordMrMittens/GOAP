using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationModule : MonoBehaviour
{
    public float maxHydration { get; private set; } = 100;
    public float minHydration { get; private set; } = 30;
    [field : SerializeField] public float currentHydration { get; private set; }
    [field : SerializeField] public float backgroundDecayRate = .005f;
    public bool NeedsWater;

    void Start()
    {
        currentHydration = maxHydration;
    }


    public void ConsumeWater(float value)
    {
        currentHydration -= value;
        if (currentHydration < 0)
        {
            currentHydration = 0;
        }
        NeedsWater = currentHydration > minHydration;
    }

    public void AddWater(float value)
    {
        currentHydration += value;
        if (currentHydration > maxHydration)
        {
            currentHydration = maxHydration;
        }
        NeedsWater = currentHydration > minHydration;
    }
}
