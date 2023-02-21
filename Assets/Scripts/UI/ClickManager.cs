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
            dialogueManager.GenerateDialogue(nPCController, needsManager, mostNeedyStat, temperatureModule);
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
        worldStatusManager.timeSpeed = 1;
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
            dialogueManager.GenerateDialogue(nPCController,needsManager,mostNeedyStat,temperatureModule);
            NPCDetailView = true;
            StatusUI.statusUIInstance.statsDisplay.SetActive(true);
            worldStatusManager.timeSpeed = 0;
        }
    }
    
}