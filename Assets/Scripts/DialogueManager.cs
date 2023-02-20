using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{
public string GenerateComentaryOnNeeds(string keyword) 
    {
        string dialogue = "Nothing to report";
        switch (keyword)
        {
            case "Hydration":
                dialogue = GetDialogue(dialogue, "ThirstyDialogue");
                break;

            case "Nutrition":
                dialogue = GetDialogue(dialogue, "HungryDialogue");
                break;

            case "Tiredness":
                dialogue = GetDialogue(dialogue, "TiredDialogue");
                break;
            default:
                dialogue = "Nothing to report";
                break;
        }
        return dialogue;
    }
    public string GenerateComentaryOnTemperature(TemperatureModule temperatureModule)
    {
        string dialogue = "Weather is pleasant";
        if (temperatureModule.currentTemperature < temperatureModule.targetTemperature - 3 || temperatureModule.currentTemperature > temperatureModule.targetTemperature + 3)
        {
            //means it cant adjust temperature properly anymore
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                dialogue = GetDialogue(dialogue, "HypothermiaDialogue"); // need to make these add to eachother
                dialogue = GetDialogue(dialogue, "HungryDialogue");
            }
            else
            {
                dialogue = GetDialogue(dialogue, "HeatstrokeDialogue");
                dialogue = GetDialogue(dialogue, "ThirstyDialogue");
            }
        }
        else
        {
            if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature)
            {
                if (WorldStatusManager.WSMInstance.currentTemperature < temperatureModule.targetTemperature - temperatureModule.coldToleranceOffset)
                {
                    dialogue = GetDialogue(dialogue, "ColdDialogue");
                }
                else
                {
                    dialogue = GetDialogue(dialogue, "CoolBreezeDialogue");
                }
            }
            else
            {
                if (WorldStatusManager.WSMInstance.currentTemperature > temperatureModule.targetTemperature + temperatureModule.heatToleranceOffset)
                {
                    dialogue = GetDialogue(dialogue, "HotDialogue");
                }
                else
                {
                    dialogue = GetDialogue(dialogue, "WarmBreezeDialogue");
                }
            }
        }
        return dialogue;
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
