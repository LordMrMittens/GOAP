using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actions : MonoBehaviour
{
    public string actionName = "Action";
    public string[] goalsRelatedTo;
    [SerializeField] float defaultCost = 1;
    public float cost {get; set;} = 1f;
    public GameObject target {get; set;}
    public List<GameObject> freeTargets {get; set;} = new List<GameObject>();
    public List<GameObject> targetsInUse {get; set;} = new List<GameObject>();
    public GameObject defaultTarget;
    public string targetTag;
    public float duration = 0f;
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> actionresults;
    public List<string> nonPermanentEffects {get; set;} = new List<string>();
    public Inventory inventory;
    public int maxOwners = 1;
    public List<NPCController> currentOwners {get; set;} = new List<NPCController>();
    public NPCController defaultOwner;
    public WorldStates belief;
    public NeedsManager needsManager {get; set;}
    //public bool running {get; set;} = false;
     [SerializeField] ContainerObject containerUsed; //if depositing item container used should be blank
    public bool activatingAction = false;
    public string relatedItemIfAvailable;
    public worldState[] preConditions;
    public worldState[] actionResults;
   
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
                if (!state.carryOver){
                    nonPermanentEffects.Add(state.key);
                }
            }
        }
        ResetCost();
        if (defaultTarget != null)
        {
            target = defaultTarget;
        }
        if (target == null && targetTag != "" && freeTargets.Count ==0 ) //only adds targets automatically if the array is empty
        {
            freeTargets.AddRange(GameObject.FindGameObjectsWithTag(targetTag));
        }
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

    public bool IsAchievable(NPCController _nPCController)
    {
        if(containerUsed){
        return CheckIfItemsAvailable(_nPCController);
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

    bool CheckIfItemsAvailable(NPCController _nPCController)
    {
        if (containerUsed.storedObjects.Contains(relatedItemIfAvailable)) //if depositing item container used should be blank
        {
            return true;
        }
        else if (defaultOwner == _nPCController)
        {
            _nPCController.beliefs.AddSingleState($"HasNo{relatedItemIfAvailable}Stored", 0);
        }
        return false;
    }
    public void RemoveAvailableTarget(GameObject _targetToRemove)
    {
        if (freeTargets.Contains(_targetToRemove))
        {
            freeTargets.Remove(_targetToRemove);
        }
        targetsInUse.Add(_targetToRemove);
        StartCoroutine(WaitForActionToComplete(_targetToRemove));
    }
    public void AddAvailableTarget(GameObject _targetToAdd)
    {
        if (targetsInUse.Contains(_targetToAdd))
        {
            targetsInUse.Remove(_targetToAdd);
        }
        freeTargets.Add(_targetToAdd);
    }
    IEnumerator WaitForActionToComplete(GameObject _target){
        yield return new WaitForSeconds(duration);
        AddAvailableTarget(_target);
    }
}
