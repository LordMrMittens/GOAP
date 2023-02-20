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
            string newPlan = dialogueManager.GetPlanInformation(nPCController);
            StatusUI.statusUIInstance.UpdatePlanWindow(newPlan);
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
            dialogueManager = new DialogueManager();
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
            commentary = dialogueManager.GenerateComentaryOnNeeds(mostNeedyStat.resourceType);
        }
        dialogueManager.GenerateComentaryOnTemperature(temperatureModule);
        StatusUI.statusUIInstance.UpdateDialogue(nPCController.gameObject.name, commentary);
    }

    // TODO MOVE ALL BELOW TO THEIR OWN CLASS
    
}
