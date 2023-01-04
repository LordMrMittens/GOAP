using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] NutritionModule nutritionModule;
    [SerializeField] TemperatureModule temperatureModule;
    [SerializeField] HydrationModule hydrationModule;
    [SerializeField] TirednessModule tirednessModule;
    WorldStatusManager worldStatusManager;
    [SerializeField] NPCController nPCController;
    [SerializeField] float tickFrequency;
    public NPCInventory nPCInventory { get; private set; }

    float tickTimer;
    //need reference to character controller,  can it be cast since they derive from a parent class?

    private void Awake()
    {
        worldStatusManager = GameObject.FindObjectOfType<WorldStatusManager>();
        nPCInventory = nPCController.nPCInventory;
    }
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > tickFrequency)
        {
            ConsumeBackgroundResources();
            ManageTemperature();

            //TODO add some sort of priority system
            if (nutritionModule.currentResource < 20)
            {
                nPCController.Invoke("GetHungry", 0);
                //use  delegates instead??
            }
            if (hydrationModule.currentResource < hydrationModule.minResource)
            {
                nPCController.Invoke("GetThirsty", 0);
            }
            if (tirednessModule.currentResource < 10)
            {
                nPCController.Invoke("GetTired", 0);
            }



            tickTimer = 0;
        }
    }

    private void ManageTemperature()
    {
        if (worldStatusManager != null)
        {
            if (temperatureModule.AdjustTemperature(worldStatusManager.currentTemperature))
            {
                if (temperatureModule.isWarming)
                {
                    nutritionModule.ConsumeResource(temperatureModule.energyConsumptionRate);
                }
                else
                {
                    hydrationModule.ConsumeResource(temperatureModule.waterConsumptionRate);
                }
            }
            // TODO calling functions may need to be moved to priority system
            if (worldStatusManager.currentTemperature < temperatureModule.GetCurrentTemperature() - temperatureModule.toleranceOffset)
            {
                nPCController.Invoke("GetTooCold", 0); //npc is too cold 
            }
            else if (worldStatusManager.currentTemperature > temperatureModule.GetCurrentTemperature() + temperatureModule.toleranceOffset)
            {
                nPCController.Invoke("GetTooHot", 0); //npc is too hot
            }
        }
    }

    private void ConsumeBackgroundResources()
    {
        nutritionModule.ConsumeResource(nutritionModule.backgroundDecayRate);
        hydrationModule.ConsumeResource(hydrationModule.backgroundDecayRate);
        tirednessModule.ConsumeResource(tirednessModule.backgroundDecayRate);
    }
    public void SatiateHunger()
    {
        nutritionModule.AddResource(100);
    }
    public void QuenchThirst()
    {
        hydrationModule.AddResource(100);
    }
    public void RestoreEnergy()
    {
        tirednessModule.AddResource(100);
    }
    public void ToggleJacket(bool IsWearingJacket)
    {
        temperatureModule.isWearingAJacket = IsWearingJacket;
    }
}
