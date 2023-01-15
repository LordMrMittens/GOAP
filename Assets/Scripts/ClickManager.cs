using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    GameObject selectedNPC;
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
        }
    }
    private void OnNpcClicked(){
        NPCController nPCController = selectedNPC.GetComponent<NPCController>();
        NeedsManager needsManager = selectedNPC.GetComponent<NeedsManager>();
        needsManager.UpdateStatsSheet();
        nPCController.GetPlanInformation();

        StatusUI.statusUIInstance.statsDisplay.SetActive(true);
    }
}
