using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickManager : MonoBehaviour
{
    GameObject selectedNPC;
    ExcelImporter excelImporter;
    WorldStatusManager worldStatusManager;
    CameraMovement cam;
    bool NPCDetailView = false;
    NPCController nPCController;
    NeedsManager needsManager; 
    BasicNeedModule mostNeedyStat;
    TemperatureModule temperatureModule; 
    string currentPlan;
    private void Start() {
        worldStatusManager = WorldStatusManager.WSMInstance;
        cam= Camera.main.GetComponent<CameraMovement>();
        
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.tag == "NPC"){
                    selectedNPC = hit.transform.gameObject;
                    OnNpcClicked();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            StatusUI.statusUIInstance.ClearStats();
            StatusUI.statusUIInstance.statsDisplay.SetActive(false);
            cam.ResetLastPositionAndRotation();
            NPCDetailView = false;
            nPCController = null;
            needsManager = null;
            mostNeedyStat = null;
            temperatureModule = null;
            worldStatusManager.timeSpeed = 1;
        }
        if (NPCDetailView && nPCController && needsManager && temperatureModule){
            needsManager.UpdateStatsSheet();
            string newPlan = GetPlanInformation();
            if(currentPlan != newPlan){
                currentPlan = newPlan;
                GenerateDialogue();
            }
            StatusUI.statusUIInstance.SetTemperature(temperatureModule.GetCurrentTemperature());
        }

    }
    private void OnNpcClicked()
    {
        if (cam.isCloseUp == false)
        {
            excelImporter = FindObjectOfType<ExcelImporter>();
            nPCController = selectedNPC.GetComponent<NPCController>();
            needsManager = selectedNPC.GetComponent<NeedsManager>();
            mostNeedyStat = needsManager.GetLowestStat();
            temperatureModule = needsManager.GetTemperatureModule();
            cam.SetCloseUpPosition(nPCController.closeupCamPos, nPCController.lookAtOffset);
            GenerateDialogue();
            NPCDetailView = true;
            StatusUI.statusUIInstance.statsDisplay.SetActive(true);
            worldStatusManager.timeSpeed = 0;
        }
    }

    private void GenerateDialogue()
    {
        string commentary = "";
        mostNeedyStat = needsManager.GetLowestStat();
        if (mostNeedyStat != null)
        {
            commentary = GenerateComentaryOnNeeds(mostNeedyStat.resourceType);
        }
        GenerateComentaryOnTemperature(temperatureModule);
        StatusUI.statusUIInstance.UpdateDialogue(nPCController.gameObject.name, commentary);
    }

    // TODO MOVE ALL BELOW TO THEIR OWN CLASS
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
            if (worldStatusManager.currentTemperature < temperatureModule.targetTemperature)
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
            if (worldStatusManager.currentTemperature < temperatureModule.targetTemperature)
            {
                if (worldStatusManager.currentTemperature < temperatureModule.targetTemperature - temperatureModule.coldToleranceOffset)
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
                if (worldStatusManager.currentTemperature > temperatureModule.targetTemperature + temperatureModule.heatToleranceOffset)
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
        if (excelImporter.text.ContainsKey(keywordDialogue))
        {
            int randomdialogue = Random.Range(0, excelImporter.text[keywordDialogue].Count);
            dialogue = excelImporter.text[keywordDialogue][randomdialogue];
        } else {
            Debug.LogWarning($"No keyword {keywordDialogue} matches text file name");
        }

        return dialogue;
    }

        public string GetPlanInformation()
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
        StatusUI.statusUIInstance.UpdatePlanWindow(planToDisplay);
        return (planToDisplay);
    }
}
