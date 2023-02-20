using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{

    public void GenerateDialogue(NPCController nPCController, NeedsManager needsManager, BasicNeedModule mostNeedyStat, TemperatureModule temperatureModule)
    {
        string commentaryToDisplay ="";
        string needsCommentary = "";
        mostNeedyStat = needsManager.GetLowestStat();
        if (mostNeedyStat != null)
        {
            needsCommentary = GenerateComentaryOnNeeds(mostNeedyStat.resourceType);
        }
        bool negativeTemperatureState = false;
        string planCommentary = GenerateComentaryOnPlans(nPCController);
        string jobCommetary = GenerateComentaryOnJob(nPCController);
        string temperatureCommentary = GenerateComentaryOnTemperature(temperatureModule, negativeTemperatureState);
         commentaryToDisplay = planCommentary + jobCommetary + temperatureCommentary + needsCommentary;
        string job = nPCController.jobGoalRelatedTo;
        job = job.Remove(job.Length - 3, 3);
        StatusUI.statusUIInstance.UpdateDialogue(nPCController.gameObject.name, job , commentaryToDisplay, needsManager.tirednessModule.nightOwl);
    }
    public string GenerateComentaryOnNeeds(string keyword) 
    {
        string needsDialogue = "";
        switch (keyword)
        {
            case "Hydration":
                needsDialogue = GetDialogue("ThirstyDialogue");
                break;

            case "Nutrition":
                needsDialogue = GetDialogue("HungryDialogue");
                break;

            case "Tiredness":
                needsDialogue = GetDialogue("TiredDialogue");
                break;
            default:
                needsDialogue = "";
                break;
        }
        if (needsDialogue != "")
        {
            return needsDialogue + " \n";
        }
        return needsDialogue;
    }
    public string GenerateComentaryOnTemperature(TemperatureModule temperatureModule, bool negativeMood)
    {
        string temperatureDialogue = "";
        if (temperatureModule.currentTemperature < temperatureModule.targetTemperature - 3 || temperatureModule.currentTemperature > temperatureModule.targetTemperature + 3)
        {
            //means it cant adjust temperature properly anymore
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                temperatureDialogue = GetDialogue("HypothermiaDialogue"); // need to make these add to eachother
                temperatureDialogue = GetDialogue("HungryDialogue");
                negativeMood = true;
            }
            else
            {
                temperatureDialogue = GetDialogue("HeatstrokeDialogue");
                temperatureDialogue = GetDialogue("ThirstyDialogue");
                negativeMood = true;
            }
        }
        else
        {
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature - temperatureModule.coldToleranceOffset)
                {
                    temperatureDialogue = GetDialogue("ColdDialogue");
                    negativeMood = true;
                }
                else
                {
                    temperatureDialogue = GetDialogue("CoolBreezeDialogue");
                    negativeMood = false;
                }
            }
            else
            {
                if (WorldStatusManager.WSMInstance.currentTemperature > temperatureModule.targetTemperature + temperatureModule.heatToleranceOffset)
                {
                    temperatureDialogue = GetDialogue("HotDialogue");
                    negativeMood = true;
                }
                else
                {
                    temperatureDialogue = GetDialogue("WarmBreezeDialogue");
                    negativeMood = false;
                }
            }
        }
                if (temperatureDialogue != "")
        {
            return temperatureDialogue + " \n";
        }
        return temperatureDialogue;
    }

    string GenerateComentaryOnPlans(NPCController nPCController)
    {
        string planDialogue = "";
        if (nPCController.currentGoal != null)
        {
            Debug.Log(nPCController.currentGoal.keyword);
             planDialogue = "I am doing work stuff because ";
            if (nPCController.currentGoal.keyword == "Idle")
            {
                planDialogue = GetDialogue("IdleDialogue");
                if (nPCController.needsManager.jobModule.isAtWork)
                {
                    planDialogue += " but ";
                }
            }
            if (nPCController.currentGoal.keyword == "Food")
            {
                planDialogue = GetDialogue("FoodPlanDialogue");
                if (nPCController.needsManager.jobModule.isAtWork)
                {
                    planDialogue += " but ";
                }
            }
            if (nPCController.currentGoal.keyword == "Drink")
            {
                planDialogue = GetDialogue("DrinkPlanDialogue");
                if (nPCController.needsManager.jobModule.isAtWork)
                {
                    planDialogue += " but ";
                }
            }
            if (nPCController.currentGoal.keyword == "Temperature")
            {
                if(nPCController.nPCInventory.CheckForJacket()){
                    planDialogue = GetDialogue("LeaveJacketDialogue");
                } else {
                    planDialogue = GetDialogue("PickJacketDialogue");
                }
            }
            if (nPCController.currentGoal.keyword == "Light")
            {
                if(nPCController.nPCInventory.CheckForLight()){
                    planDialogue = GetDialogue("LeaveLightDialogue");
                } else {
                    planDialogue = GetDialogue("PickLightDialogue");
                }
            }
            if (nPCController.currentGoal.keyword == "Groceries")
            {
                    planDialogue = GetDialogue("GroceriesDialogue");
            }

               
            
        }
        else
        {
            planDialogue = "I have no plans ";
        }
        if (planDialogue != "")
        {
            return planDialogue + " \n";
        }else{
        return "";}
    }
    void GenerateComentaryOnInventory()
    {

    }
    string GenerateComentaryOnJob(NPCController nPCController)
    {
        string jobDialogue = "";
        if(nPCController.needsManager.jobModule.isAtWork){
           jobDialogue = GetDialogue("JobDialogue");
        }
        if( jobDialogue != ""){
            Debug.Log("JobCommentary returning a line break");
            return jobDialogue + " \n";
        }
        return jobDialogue;
    }

    private string GetDialogue(string keywordDialogue)
    {
        string _dialogue= "";
        if (ExcelImporter.textImporterInstance.text.ContainsKey(keywordDialogue))
        {
            int randomdialogue = Random.Range(0, ExcelImporter.textImporterInstance.text[keywordDialogue].Count);
            _dialogue = ExcelImporter.textImporterInstance.text[keywordDialogue][randomdialogue];
        }
        else
        {
            Debug.LogWarning($"No keyword {keywordDialogue} matches text file name");
        }
        if (_dialogue != "")
        {
            return _dialogue + " \n";
        }
        return _dialogue;
    }

        public string GetPlanInformation(NPCController nPCController)
    {
        string planToDisplay = "My plan is: \n";
        if (nPCController.actionsInPlan.Count > 0)
        {
            foreach (Actions action in nPCController.actionsInPlan)
            {
                if(nPCController.actionsCompleted.Contains(action)){
                    planToDisplay += $"<s>{action.actionName}</s>.\n";
                } else {
                planToDisplay += action.actionName + ".\n";
                }
            }
        }
        else
        {
            planToDisplay += "There is no plan";
        }
        return (planToDisplay);
    }
}
