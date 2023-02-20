using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{

    public void GenerateDialogue(NPCController nPCController, NeedsManager needsManager, BasicNeedModule mostNeedyStat, TemperatureModule temperatureModule)
    {
        string commentaryToDisplay;
        string needsCommentary = "";
        string junction = "";
        mostNeedyStat = needsManager.GetLowestStat();
        if (mostNeedyStat != null)
        {
            needsCommentary = GenerateComentaryOnNeeds(mostNeedyStat.resourceType);
        }
        bool negativeMood = false;
        if(negativeMood){
            junction = "and \n";
        } else {
            junction = "but \n";
        }

        string temperatureCommentary = GenerateComentaryOnTemperature(temperatureModule, negativeMood);
        if (needsCommentary != "")
        {
            commentaryToDisplay = temperatureCommentary + "\n" + junction + needsCommentary;
        }
        else
        {
            commentaryToDisplay = temperatureCommentary;
        }
        string job = nPCController.jobGoalRelatedTo;
        job = job.Remove(job.Length - 3, 3);
        StatusUI.statusUIInstance.UpdateDialogue(nPCController.gameObject.name, job , commentaryToDisplay, needsManager.tirednessModule.nightOwl);
    }
    public string GenerateComentaryOnNeeds(string keyword) 
    {
        string needsDialogue = "Nothing to report";
        switch (keyword)
        {
            case "Hydration":
                needsDialogue = GetDialogue(needsDialogue, "ThirstyDialogue");
                break;

            case "Nutrition":
                needsDialogue = GetDialogue(needsDialogue, "HungryDialogue");
                break;

            case "Tiredness":
                needsDialogue = GetDialogue(needsDialogue, "TiredDialogue");
                break;
            default:
                needsDialogue = "Nothing to report";
                break;
        }
        return needsDialogue;
    }
    public string GenerateComentaryOnTemperature(TemperatureModule temperatureModule, bool negativeMood)
    {
        string temperatureDialogue = "Weather is pleasant";
        if (temperatureModule.currentTemperature < temperatureModule.targetTemperature - 3 || temperatureModule.currentTemperature > temperatureModule.targetTemperature + 3)
        {
            //means it cant adjust temperature properly anymore
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                temperatureDialogue = GetDialogue(temperatureDialogue, "HypothermiaDialogue"); // need to make these add to eachother
                temperatureDialogue = GetDialogue(temperatureDialogue, "HungryDialogue");
                negativeMood = true;
            }
            else
            {
                temperatureDialogue = GetDialogue(temperatureDialogue, "HeatstrokeDialogue");
                temperatureDialogue = GetDialogue(temperatureDialogue, "ThirstyDialogue");
                negativeMood = true;
            }
        }
        else
        {
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature - temperatureModule.coldToleranceOffset)
                {
                    temperatureDialogue = GetDialogue(temperatureDialogue, "ColdDialogue");
                    negativeMood = true;
                }
                else
                {
                    temperatureDialogue = GetDialogue(temperatureDialogue, "CoolBreezeDialogue");
                    negativeMood = false;
                }
            }
            else
            {
                if (WorldStatusManager.WSMInstance.currentTemperature > temperatureModule.targetTemperature + temperatureModule.heatToleranceOffset)
                {
                    temperatureDialogue = GetDialogue(temperatureDialogue, "HotDialogue");
                    negativeMood = true;
                }
                else
                {
                    temperatureDialogue = GetDialogue(temperatureDialogue, "WarmBreezeDialogue");
                    negativeMood = false;
                }
            }
        }
        return temperatureDialogue;
    }

    void GenerateComentaryOnPlans()
    {

    }
    void GenerateComentaryOnInventory()
    {

    }
    void GenerateComentaryOnJob()
    {

    }

    private string GetDialogue(string dialogue, string keywordDialogue)
    {
        if (ExcelImporter.textImporterInstance.text.ContainsKey(keywordDialogue))
        {
            int randomdialogue = Random.Range(0, ExcelImporter.textImporterInstance.text[keywordDialogue].Count);
            dialogue = ExcelImporter.textImporterInstance.text[keywordDialogue][randomdialogue];
        } else {
            Debug.LogWarning($"No keyword {keywordDialogue} matches text file name");
        }

        return dialogue;
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
