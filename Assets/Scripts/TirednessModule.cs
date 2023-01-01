using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirednessModule : MonoBehaviour
{
    float maxEnergy = 100;
    public float currentEnergy {get; private set;}
    [field:SerializeField] public float backgroundDecayRate = .01f;
     
    public bool isTired;
    
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    public void ConsumeEnergy(float value)
    {
        currentEnergy -= value;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        isTired = currentEnergy < 10;
    }

    public void AddEnergy(float value)
    {
        currentEnergy += value;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        isTired = currentEnergy < 10;
    }
}
