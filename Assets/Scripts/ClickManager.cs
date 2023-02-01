using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    GameObject selectedNPC;
    ExcelImporter excelImporter;
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
        excelImporter = FindObjectOfType<ExcelImporter>();
        NPCController nPCController = selectedNPC.GetComponent<NPCController>();
        NeedsManager needsManager = selectedNPC.GetComponent<NeedsManager>();
        BasicNeedModule mostNeedyStat = needsManager.GetLowestStat();
        TemperatureModule temperatureModule = needsManager.GetTemperatureModule();
        string commentary = "";
        if (mostNeedyStat != null)
        {
           commentary = GenerateComentaryOnNeeds(mostNeedyStat.resourceType);
        }
        needsManager.UpdateStatsSheet();
        nPCController.GetPlanInformation();
        StatusUI.statusUIInstance.UpdateDialogue(nPCController.gameObject.name, commentary);
        StatusUI.statusUIInstance.statsDisplay.SetActive(true);
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
            case "Temperature":
            //temperature works differently
                dialogue = GetDialogue(dialogue, "TemperatureDialogue");
                break;
                default:
                dialogue = "Nothing to report";
                break;
        }
        return dialogue;
    }
        void GenerateTemperatureCommentary(TemperatureModule _temperatureModule){

        if(_temperatureModule.isWarming){
            //its cold
        } else{
            //its hot
        }

        //check for jacket in inventory
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


}
