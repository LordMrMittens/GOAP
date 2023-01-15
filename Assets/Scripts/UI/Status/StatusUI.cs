using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    public static StatusUI statusUIInstance;
    public GameObject statsDisplay;
    [SerializeField] GameObject needPrefab;
    [SerializeField] RectTransform rootPanel;
    [SerializeField] PlanUI planUI;
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

    public void UpdateDialogue(string _nPCName, string _dialogue){
        dialogueUI.UpdateNameText(_nPCName);
        dialogueUI.UpdateDialogueText(_dialogue);
    }
    public void ClearStats()
    {
        foreach (KeyValuePair<BasicNeedModule, NeedUI> stat in displayedStats)
        {
            Destroy(stat.Value.gameObject);
        }
        displayedStats.Clear();
        UpdateDialogue("", "");
        UpdatePlanWindow("");
    }
}
