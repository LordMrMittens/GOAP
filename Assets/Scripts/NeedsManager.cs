using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] NutritionModule energyModule;
    [SerializeField] TemperatureModule temperatureModule;
    [SerializeField] HydrationModule hydrationModule;
    //need reference to character controller,  can it be cast since they derive from a parent class?
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (temperatureModule.AdjustTemperature(36))
        { //temperature comes from the world
            if (temperatureModule.isWarming)
            {
                energyModule.ConsumeEnergy(temperatureModule.energyConsumptionRate);
            }
            else
            {
                hydrationModule.ConsumeWater(temperatureModule.waterConsumptionRate);
            }

        } //change this so it comes from the world state
        if (energyModule.currentNutrition < 20)
        {
            //goal is hungry
        }
    }
}
