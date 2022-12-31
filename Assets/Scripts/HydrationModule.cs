using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationModule : MonoBehaviour
{
    float maxHydration = 100;
    float minHydration = 30;
    public float currentHydration { get; private set; }
    [SerializeField] float backgroundDecayRate = .005f;
    float timer;

    public bool NeedsWater;

    void Start()
    {
        currentHydration = maxHydration;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ConsumeBackgroundWater();

        NeedsWater = currentHydration > minHydration;
    }

    private void ConsumeBackgroundWater()
    {
        if (timer > 1)
        {
            currentHydration -= backgroundDecayRate;
            timer = 0;
        }
    }

    public void ConsumeWater(float value)
    {
        currentHydration -= value;
        if (currentHydration < 0)
        {
            currentHydration = 0;
        }
    }

    public void AddWater(float value)
    {
        currentHydration += value;
        if (currentHydration > maxHydration)
        {
            currentHydration = maxHydration;
        }
    }
}
