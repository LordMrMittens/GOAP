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
    void Awake()
    {
        statsDisplay.SetActive(true);
        statusUIInstance = this;
        spawnPoint = rootPanel.transform.position;
        statsDisplay.SetActive(false);
    }
    public void UpdateStatusWindow(BasicNeedModule[] needs)
    {
        for (int i = 0; i < needs.Length; i++)
        {
            if (!displayedStats.ContainsKey(needs[i]))
            {
                spawnPoint = new Vector3(rootPanel.transform.position.x, rootPanel.transform.position.y - (i * 50), rootPanel.transform.position.z);
                displayedStats[needs[i]] = Instantiate(needPrefab, spawnPoint, Quaternion.identity, rootPanel).GetComponent<NeedUI>();
            }
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
