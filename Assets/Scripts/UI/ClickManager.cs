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
    DialogueManager dialogueManager;
    string currentPlan;
    bool grantedRequest;
    bool deniedRequest;
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
                    EnterFocusMode();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitFocusMode();
        }
        if (NPCDetailView && nPCController && needsManager && temperatureModule)
        {
            UpdateFocusMode();
        }

    }

    private void UpdateFocusMode()
    {
        needsManager.UpdateStatsSheet();
        string newPlan = dialogueManager.GetPlanInformation(nPCController);
        string newInventory = dialogueManager.GetInventoryInformation(nPCController.nPCInventory);
        StatusUI.statusUIInstance.UpdatePlanWindow(newPlan);
        StatusUI.statusUIInstance.UpdateInventoryWindow(newInventory);
        if (currentPlan != newPlan)
        {
            currentPlan = newPlan;
            dialogueManager.GenerateDialogue(nPCController, needsManager, mostNeedyStat, temperatureModule, grantedRequest, deniedRequest);
            grantedRequest=false;
            deniedRequest=false;
        }
        StatusUI.statusUIInstance.SetTemperature(temperatureModule.GetCurrentTemperature());
    }

    private void ExitFocusMode()
    {
        StatusUI.statusUIInstance.ClearStats();
        StatusUI.statusUIInstance.statsDisplay.SetActive(false);
        cam.ResetLastPositionAndRotation();
        NPCDetailView = false;
        nPCController = null;
        needsManager = null;
        mostNeedyStat = null;
        temperatureModule = null;
        worldStatusManager.ChangeWorldSpeed(1);
        grantedRequest=false;
        deniedRequest = false;
    }

    private void EnterFocusMode()
    {
        if (cam.isCloseUp == false)
        {
            dialogueManager = new DialogueManager();
            excelImporter = FindObjectOfType<ExcelImporter>();
            nPCController = selectedNPC.GetComponent<NPCController>();
            needsManager = selectedNPC.GetComponent<NeedsManager>();
            mostNeedyStat = needsManager.GetLowestStat();
            temperatureModule = needsManager.GetTemperatureModule();
            cam.SetCloseUpPosition(nPCController.closeupCamPos, nPCController.lookAtOffset);
            dialogueManager.GenerateDialogue(nPCController,needsManager,mostNeedyStat,temperatureModule, grantedRequest, deniedRequest);
            NPCDetailView = true;
            StatusUI.statusUIInstance.statsDisplay.SetActive(true);
           worldStatusManager.ChangeWorldSpeed(0);
           grantedRequest=false;
           deniedRequest = false;
        }
    }
    public void GrantRequest()
    {
        if (needsManager.GetLowestStat())
        {
            string keyword = needsManager.GetLowestStat().resourceType;
            switch (keyword)
            {
                case "Hydration":
                    needsManager.QuenchThirst();
                    break;

                case "Nutrition":
                    needsManager.SatiateHunger();
                    break;

                case "Tiredness":
                    needsManager.RestoreEnergy();
                    break;
                case "Tools": // tools stuff

                    break;
                default:

                    break;
            }
            nPCController.ResetGoals();
            //update all dialogue and plans
            mostNeedyStat = needsManager.GetLowestStat();
        } else {
            mostNeedyStat =null;
        }
        grantedRequest = true;
        deniedRequest  =false;
        dialogueManager.GenerateDialogue(nPCController, needsManager, mostNeedyStat, temperatureModule, grantedRequest, deniedRequest);
    }

    public void DenyRequest()
    {
        if (needsManager.GetLowestStat())
        {
            string keyword = needsManager.GetLowestStat().resourceType;
            switch (keyword)
            {
                case "Hydration":
                    
                    break;

                case "Nutrition":
                    
                    break;

                case "Tiredness":
                    
                    break;
                case "Tools": // tools stuff

                    break;
                default:

                    break;
            }
            nPCController.ResetGoals();
            //update all dialogue and plans
            mostNeedyStat = needsManager.GetLowestStat();
        } else {
            mostNeedyStat =null;
        }
        grantedRequest = false;
        deniedRequest = true;
        dialogueManager.GenerateDialogue(nPCController, needsManager, mostNeedyStat, temperatureModule, grantedRequest, deniedRequest);
    }
}
