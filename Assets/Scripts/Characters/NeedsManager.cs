using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] NutritionModule nutritionModule;
    [SerializeField] TemperatureModule temperatureModule;
    [SerializeField] HydrationModule hydrationModule;
    [field: SerializeField] public TirednessModule tirednessModule;
    public WorldStatusManager worldStatusManager {get;set;}
    [SerializeField] NPCController nPCController;
    [SerializeField] float tickFrequency;
    public NPCInventory nPCInventory { get; private set; }
    public BasicNeedModule[] basicNeedModules {get; private set;}
    public BaseJobModule jobModule {get;set;}
    int workNeedToleranceOffset = 20;
    float tickTimer;
    bool canSee = true;
    int idletimer =0;
    [SerializeField] CapsuleCollider spriteCollider;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject yellowExclamation;
    [SerializeField] GameObject redExclamation;
    //need reference to character controller,  can it be cast since they derive from a parent class?

    private void Awake()
    {
        worldStatusManager = GameObject.FindObjectOfType<WorldStatusManager>();
        basicNeedModules = GetComponents<BasicNeedModule>();
        
        jobModule = GetComponent<BaseJobModule>();
        nPCInventory = nPCController.nPCInventory;
        spriteCollider.enabled = false;
    }
    void Update()
    {
        foreach (BasicNeedModule module in basicNeedModules)
        {
            bool hasAllResources = true;
            if (module.hasResource == false)
            {
                if (nPCController.currentGoal != null && nPCController.currentGoal.keyword != module.resourceType)
                {
                    hasAllResources = false;
                }
            }
            nPCController.canMove = hasAllResources;
        }
        tickTimer += Time.deltaTime;
        if (tickTimer > tickFrequency)
        {
            ConsumeBackgroundResources();
            ManageTemperature();
            canSee = !worldStatusManager.isDark;
            if(!canSee){
                if (CheckForLight()==false){
                nPCController.Invoke("PrepForNightTime", 0);
                nPCController.hasGoal=true;
                }
            } else {
                if(CheckForLight()== true){
                nPCController.Invoke("PrepForDayTime", 0);
                nPCController.hasGoal=true;
                }
            }
            //TODO add some sort of priority system
            if ((jobModule.isAtWork && nutritionModule.currentResource < nutritionModule.minResource - workNeedToleranceOffset) || 
                (!jobModule.isAtWork && nutritionModule.currentResource < nutritionModule.minResource))
            {
                nPCController.Invoke("GetHungry", 0);
                nPCController.hasGoal=true;
                //use  delegates instead??
            }
            if ((jobModule.isAtWork && hydrationModule.currentResource < hydrationModule.minResource - workNeedToleranceOffset) || 
                (!jobModule.isAtWork && hydrationModule.currentResource < hydrationModule.minResource))
            {
                nPCController.Invoke("GetThirsty", 0);
                nPCController.hasGoal=true;
            }
            if ((jobModule.isAtWork && tirednessModule.currentResource < tirednessModule.minResource - workNeedToleranceOffset) || 
                (!jobModule.isAtWork && tirednessModule.currentResource < tirednessModule.minResource))
            {
                nPCController.Invoke("GetTired", 0);
                nPCController.hasGoal=true;
            }
            if(nPCController.beliefs.GetAllStates().ContainsKey("HasNoFoodStored")){
                nPCController.Invoke("RestockFood", 0);
                nPCController.hasGoal=true;
            }
            if(nPCController.beliefs.GetAllStates().ContainsKey("HasNoDrinkStored")){
                nPCController.Invoke("RestockDrink", 0);
                nPCController.hasGoal = true;
            }
            if (!jobModule.isAtWork || !nPCController.hasGoal) //&& not social hours??
            {
                idletimer++;
                if (idletimer > 1)
                {
                    nPCController.Invoke("BeIdle", 0);
                    nPCController.hasGoal = true;
                    idletimer = 0;
                }

            }
            else
            {
                idletimer = 0;
            }
            NeedSignalLogic();
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
                    if (temperatureModule.isWearingAJacket)
                    {
                        nutritionModule.ConsumeResource(temperatureModule.energyConsumptionRate);
                    }
                    else
                    {
                        nutritionModule.ConsumeResource(temperatureModule.energyConsumptionRate + .5f);
                    }
                }
                else
                {
                    if (temperatureModule.isWearingAJacket)
                    {
                        hydrationModule.ConsumeResource(temperatureModule.waterConsumptionRate + .5f);
                    }
                    else
                    {
                        hydrationModule.ConsumeResource(temperatureModule.waterConsumptionRate);
                    }
                }
            }
            // TODO calling functions may need to be moved to priority system
            if (worldStatusManager.currentTemperature < temperatureModule.GetCurrentTemperature() - temperatureModule.coldToleranceOffset)
            {
                nPCController.Invoke("GetTooCold", 0); //npc is too cold 
                nPCController.hasGoal=true;
            }
            else if (worldStatusManager.currentTemperature > temperatureModule.GetCurrentTemperature() + temperatureModule.heatToleranceOffset)
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

    public void UpdateStatsSheet()
    {
        StatusUI.statusUIInstance.UpdateStatusWindow(basicNeedModules);
         //move this to a dialogue manager?
    }

    public bool CheckForLight()
    {
        if (nPCInventory.itemsEquipped.Contains("Light"))
        {
            return true;
        }
        return false;
    }

    public TemperatureModule GetTemperatureModule(){
        return temperatureModule;
    }
    public BasicNeedModule GetLowestStat(){
        BasicNeedModule lowestModule = null;
        float lowestStat = Mathf.Infinity;
        if (basicNeedModules.Length > 0)
        {
            foreach (BasicNeedModule module in basicNeedModules)
            {
                if (module.currentResource < module.minResource &&  module.currentResource < lowestStat)
                {
                    if (module != temperatureModule)
                    {
                        lowestStat = module.currentResource;
                        lowestModule = module;
                    }
                }
            }
        }
        if (lowestModule != null)
        {
            return lowestModule;
        }
        return null;
    }
    void NeedSignalLogic()
    {
        BasicNeedModule needyModule = GetLowestStat();
        if (needyModule != null)
        {
            if(needyModule.currentResource <= needyModule.minResource/5){
                ActivateNeedSignal(redExclamation);
            } else if (needyModule.currentResource <= needyModule.minResource/2){
                ActivateNeedSignal(yellowExclamation);
            } else if(needyModule.currentResource <= needyModule.minResource){
                ActivateNeedSignal(exclamation);
            } else {
                DeactivateNeedSignal();
            }
        } else{
            DeactivateNeedSignal();
        }
    }
    void ActivateNeedSignal(GameObject _signal){
        spriteCollider.enabled = true;
        exclamation.SetActive(false);
        yellowExclamation.SetActive(false);
        redExclamation.SetActive(false);
        _signal.SetActive(true);

    }
    void DeactivateNeedSignal()
    {
        spriteCollider.enabled = false;
        exclamation.SetActive(false);
        yellowExclamation.SetActive(false);
        redExclamation.SetActive(false);
    }
}

