using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public static StatusUI statusUIInstance;
    public GameObject statsDisplay;
    [SerializeField] GameObject needPrefab;
    [SerializeField] TextMeshProUGUI temperatureText;
    [SerializeField] RectTransform rootPanel;
    [SerializeField] PlanUI planUI;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ComentaryDialogueUI dialogueUI;
    Dictionary<BasicNeedModule, NeedUI> displayedStats = new Dictionary<BasicNeedModule, NeedUI>();
    Vector3 spawnPoint;
    float verticalPanelOffset = 50;
    float horizontalPanelOffset= 320;
    void Awake()
    {
        statsDisplay.SetActive(true);
        statusUIInstance = this;
        spawnPoint = rootPanel.transform.position;
        statsDisplay.SetActive(false);
    }
    public void UpdateStatusWindow(BasicNeedModule[] needs)
    {
        int xCounter = 0;
        int yCounter = 0;
        for (int i = 0; i < needs.Length; i++)
        {
            if (i == 5)
            {
                xCounter++;
                yCounter = 0;
            }
            if (!displayedStats.ContainsKey(needs[i]))
            {
                spawnPoint = new Vector3(rootPanel.transform.position.x + (xCounter * horizontalPanelOffset), rootPanel.transform.position.y - (yCounter * verticalPanelOffset), rootPanel.transform.position.z);
                displayedStats[needs[i]] = Instantiate(needPrefab, spawnPoint, Quaternion.identity, rootPanel).GetComponent<NeedUI>();
            }
            yCounter++;
            displayedStats[needs[i]].UpdateGoalInfo(needs[i].displayName, needs[i].currentResource);

        }

    }
    public void UpdatePlanWindow(string _plan)
    {
        planUI.UpdatePlanText(_plan);
    }
    public void UpdateInventoryWindow(string _inventory){
        inventoryUI.UpdateInventoryText(_inventory);
    }

    public void UpdateDialogue(string _nPCName, string job , string _dialogue, bool _nightOwl = false){
        string nightOwl = "";
        if(_nightOwl){
            nightOwl = "(Nightshift)";
        }
        dialogueUI.UpdateNameText(_nPCName, job ,nightOwl);
        dialogueUI.UpdateDialogueText(_dialogue);
    }
    public void ClearStats()
    {
        foreach (KeyValuePair<BasicNeedModule, NeedUI> stat in displayedStats)
        {
            Destroy(stat.Value.gameObject);
        }
        displayedStats.Clear();
        UpdateDialogue("", "", "");
        UpdatePlanWindow("");
        UpdateInventoryWindow("");
    }
    public void SetTemperature(float temp){

        temperatureText.text = $"Body Temp: {temp.ToString("F1")}°C";
    }
    public void UpdateInventory(){

    }
}
