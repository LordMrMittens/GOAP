using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureModule : BasicNeedModule
{

    [field : SerializeField] public float targetTemperature;
    [field : SerializeField] public float toleranceOffset;
    public float currentTemperature;
    [field : SerializeField] public float energyConsumptionRate {get; private set;} = .5f;
    [field : SerializeField] public float waterConsumptionRate {get; private set;} = .1f;
    public bool isWarming;
    public bool isWearingAJacket;
    public float reportedTemperature;
    protected override void Start()
    {
        base.Start();
        currentTemperature = targetTemperature;
        isCommonNeed = false;
        resourceType = "Temperature";
    }

   public bool AdjustTemperature(float value)
    {
        if (isWearingAJacket){
            currentTemperature += 20;
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
        currentResource = reportedTemperature;
        return reportedTemperature;
    }
}
