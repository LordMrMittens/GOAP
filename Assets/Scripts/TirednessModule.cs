using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirednessModule : MonoBehaviour
{
    float maxEnergy = 100;
    public float currentEnergy {get; private set;}
    [SerializeField] float backgroundDecayRate = .01f;
    float timer;
     
    public bool hasEnergy;
    
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ConsumeBackgroundEnergy();

        hasEnergy = currentEnergy > 0;
    }

    private void ConsumeBackgroundEnergy()
    {
        if (timer > 1)
        {
            currentEnergy -= backgroundDecayRate;
            timer = 0;
        }
    }

    public void ConsumeEnergy(float value)
    {
        currentEnergy -= value;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
    }

    public void AddEnergy(float value)
    {
        currentEnergy += value;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
}
