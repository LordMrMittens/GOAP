using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actions : MonoBehaviour
{
    public string actionName = "Action";
    public string[] goalsRelatedTo;
    
    [SerializeField] float defaultCost = 1;
    public float cost = 1f;
    public GameObject target {get; set;}
    public GameObject[]  targets {get; set;}
    public GameObject defaultTarget;
    public string targetTag;
    public float duration = 0f;
    public worldState[] preConditions;
    public worldState[] actionResults;
    public Dictionary<string, int> preconditions;
    public Dictionary<string,int> actionresults;
    public Inventory inventory;
    public int maxOwners = 1;
    public List<NPCController> currentOwners = new List<NPCController>();
    public NPCController defaultOwner;
    public WorldStates belief;
    public NeedsManager needsManager {get; set;}
    public bool running {get; set;} = false;
    public bool activatingAction = false;
    public string relatedItemIfAvailable;
    public bool hasOwner {get; set;} = false;

   [SerializeField] ContainerObject containerUsed;
    public Actions(){
        preconditions = new Dictionary<string, int>();
        actionresults = new Dictionary<string, int>();
    }
    private void Awake() {

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
        ResetCost();
    }
    public void SetupOwnership(NPCController owner)
    {
        if (currentOwners.Count < maxOwners)
        {
            currentOwners.Add(owner);
            
        } else if (defaultOwner != owner) {
            Debug.LogWarning($"Owner {owner.gameObject.name} was not added because action {actionName} was full");
        }
    }
    public void RemoveOwnership(NPCController owner)
    {
        currentOwners.Remove(owner);
    }
    public void ResetCost(){
       cost = defaultCost;
    }

    public bool IsAchievable()
    {
        if(containerUsed){
        return CheckIfItemsAvailable();
        }
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
    public abstract bool PostPerform(NPCController _nPCController);

    bool CheckIfItemsAvailable()
    {
        if (containerUsed.storedObjects.Count > 0)
        {
            if(containerUsed.storedObjects.Contains(relatedItemIfAvailable)){
                return true;
            }
        }
        return false;
    }
}
