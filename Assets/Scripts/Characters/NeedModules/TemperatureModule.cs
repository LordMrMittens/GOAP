using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureModule : MonoBehaviour
{

    [field : SerializeField] public float targetTemperature;
    [field : SerializeField] public float coldToleranceOffset;
    float defaultColdTolerance;
    [field : SerializeField] public float heatToleranceOffset;
    float defaultHeatTolerance;

    public float currentTemperature;
    [field : SerializeField] public float energyConsumptionRate {get; private set;} = .5f;
    [field : SerializeField] public float waterConsumptionRate {get; private set;} = .1f;
    public bool isWarming;
    public bool isWearingAJacket;
    public float reportedTemperature;
    void Start()
    {
        currentTemperature = targetTemperature;
        defaultColdTolerance = coldToleranceOffset;
        defaultHeatTolerance = heatToleranceOffset;
    }

    public bool AdjustTemperature(float value)
    {
        if (isWearingAJacket){
            coldToleranceOffset = 20;
            heatToleranceOffset = -10;
        } else {
            coldToleranceOffset = defaultColdTolerance;
            heatToleranceOffset = defaultHeatTolerance;
        }
        AffectTemperature(value);
        isWarming = false;
        return CorrectTemperature();
    }

    private bool CorrectTemperature()
    {
        if (currentTemperature < targetTemperature)
        {
            WarmUp();
            return true;
        }
         else if (currentTemperature > targetTemperature)
        {
            CoolDown();
            return true;
        }
        return false;
    }

    private void AffectTemperature(float value)
    {
        if (currentTemperature > value)
        {
            currentTemperature -= (currentTemperature - value) / 120;
        }
        else if (currentTemperature < value)
        {
            currentTemperature += (value - currentTemperature) / 120; //this may be the wrong way for doing this
        }
        reportedTemperature = currentTemperature;
    }

    void WarmUp(){
        currentTemperature = targetTemperature;
        isWarming=true;
    }
    void CoolDown(){
        currentTemperature = targetTemperature;
        isWarming = false;
    }
    public float GetCurrentTemperature(){
        return reportedTemperature;
    }
}
