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
            if (nutritionModule.currentResource < nutritionModule.minResource)
            {
                nPCController.Invoke("GetHungry", 0);
                nPCController.hasGoal=true;
                //use  delegates instead??
            }
            if (hydrationModule.currentResource < hydrationModule.minResource)
            {
                nPCController.Invoke("GetThirsty", 0);
                nPCController.hasGoal=true;
            }
            if (tirednessModule.currentResource < tirednessModule.minResource)
            {
                nPCController.Invoke("GetTired", 0);
                nPCController.hasGoal=true;
            }


            GenerateComentary(); // testing purposes only
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
                nPCController.hasGoal=true;
            }
            else if (worldStatusManager.currentTemperature > temperatureModule.GetCurrentTemperature() + temperatureModule.toleranceOffset)
            {
                nPCController.Invoke("GetTooHot", 0); //npc is too hot
                nPCController.hasGoal=true;
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
    //create a public void where dialogue manager can get priorities and generate dialogue
    public void GenerateComentary() //move this to a dialogue manager turn this into get priorities
    {
        string dialogue = "Testing dialogue";
        StatusUI.statusUIInstance.UpdateDialogue(this.gameObject.name, dialogue);

    }
}
