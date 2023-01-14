using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    public static StatusUI statusUIInstance;
    [SerializeField] GameObject needPrefab;
    [SerializeField] RectTransform rootPanel;
    [SerializeField] PlanUI planUI;
    [SerializeField] ComentaryDialogueUI dialogueUI;
    Dictionary<MonoBehaviour, NeedUI> displayedStats = new Dictionary<MonoBehaviour, NeedUI>();
    int needCounter = 0;
    void Awake()
    {
        statusUIInstance = this;
    }
    public void UpdateGoal(MonoBehaviour need, string _name, float _priority)
    {
        string name = "";
        if (!displayedStats.ContainsKey(need))
        {
            Vector3 spawnPoint = new Vector3(rootPanel.transform.position.x, rootPanel.transform.position.y - (needCounter*50), rootPanel.transform.position.z);
            displayedStats[need] = Instantiate(needPrefab, spawnPoint , Quaternion.identity, rootPanel).GetComponent<NeedUI>();
           needCounter ++;
        }
        name = _name;
        displayedStats[need].UpdateGoalInfo(name, _priority);
    }
    public void UpdatePlanStatus(string _plan){
        planUI.UpdatePlanText(_plan);
    }

    public void UpdateDialogue(string _nPCName, string _dialogue){
        dialogueUI.UpdateNameText(_nPCName);
        dialogueUI.UpdateDialogueText(_dialogue);
    }
}
