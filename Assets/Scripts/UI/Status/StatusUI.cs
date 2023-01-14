using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    public static StatusUI statusUIInstance;
    [SerializeField] GameObject needPrefab;
    [SerializeField] RectTransform rootPanel;
    Dictionary<MonoBehaviour, NeedUI> displayedStats = new Dictionary<MonoBehaviour, NeedUI>();
    //List<NeedUI> displayedNeeds = new List<NeedUI>();
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
           // displayedNeeds.Add(displayedStats[need]);
           needCounter ++;
        }
        name = _name;
        displayedStats[need].UpdateGoalInfo(name, _priority);
        /*
        for (int i = 0; i < displayedNeeds.Count; i++)
        {
            Vector3 position = displayedNeeds[i].transform.position;
            displayedNeeds[i].transform.position = new Vector3(position.x, position.y - (i * 40), position.z);
        }*/
    }
}
