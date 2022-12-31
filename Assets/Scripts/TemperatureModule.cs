using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureModule : MonoBehaviour
{

    [SerializeField] float targetTemperature = 36.5f;
    float currentTemperature;
    [field : SerializeField] public float energyConsumptionRate {get; private set;} = .5f;
    [field : SerializeField] public float waterConsumptionRate {get; private set;} = .1f;
    public bool isWarming;


    // Start is called before the first frame update
    void Start()
    {
        currentTemperature = 36.5f;
    }

   public bool AdjustTemperature(float value)
    {
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
            currentTemperature += (value - currentTemperature) / 120; //this may be the wrong way fo doing this
        }
    }

    void WarmUp(){
        currentTemperature = targetTemperature;
        isWarming=true;
    }
    void CoolDown(){
        currentTemperature = targetTemperature;
        isWarming = false;
    }
}
