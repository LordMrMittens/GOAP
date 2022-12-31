using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actions : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1f;
    public GameObject target;
    public string targetTag;
    public float duration = 0f;
    public worldState[] preConditions;
    public worldState[] actionResults;
    public NavMeshAgent agent;
    public Dictionary<string, int> preconditions;
    public Dictionary<string,int> actionresults;
    public Inventory inventory;
    public WorldStates agentBelief;
    public WorldStates belief;
    public bool running = false;
    public bool activatingAction = false;

    

    public Actions(){
        preconditions = new Dictionary<string, int>();
        actionresults = new Dictionary<string, int>();
    }
    private void Awake() {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        if (preConditions != null){
            foreach (worldState state in preConditions)
            {
                preconditions.Add(state.key, state.value);
            }
        }
        if (actionResults != null)
        {
            foreach (worldState state in actionResults)
            {
                actionresults.Add(state.key, state.value);
            }
        }
        inventory = this.GetComponent<NPCController>().inventory;
        belief = this.GetComponent<NPCController>().beliefs;
        
    }

    public bool IsAchievable()
    {
        return true;
    }
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> condition in preconditions)
        {
            if (!conditions.ContainsKey(condition.Key))
            {
                return false;
            }
        }

        return true;
    }
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
