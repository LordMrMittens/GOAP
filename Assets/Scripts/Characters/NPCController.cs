using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
public class SubGoal
{
    public Dictionary<string, int> subGoals;
    public bool remove;
    public string keyword;

    public SubGoal(string s, int i, bool b, string k = "")
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(s, i);
        remove = b;
        keyword = k;
    }
}

public class NPCController : MonoBehaviour
{
    public string SetGoal;
    public float worldSpeed = 1;
    NavMeshAgent agent;
    public List<Actions> allAvailableActions = new List<Actions>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public Inventory inventory = new Inventory();
    public WorldStates beliefs = new WorldStates();
    [SerializeField] GameObject ActionHolder;
    public GameObject target;
    Planner planner;
    public Queue<Actions> actionQueue;
    List<Actions> actionsInPlan = new List<Actions>();
    public Actions currentAction;
    public Actions previousAction { get; private set; }
    public GameObject previousTarget { get; private set; }
    SubGoal currentGoal;
    public bool invoked = false;

    [SerializeField] float DistanceFromTarget = 1.3f;

    public NeedsManager needsManager;
    public NPCInventory nPCInventory;

    public float tickFrequency = 1f;
    float tickCounter;

    public bool hasGoal { get; set; }
    public bool canPlan = true;
    public List<SubGoal> failedGoalsList = new List<SubGoal>();

    protected virtual void Start()
    {
        GetDefaultActions();
        agent = GetComponent<NavMeshAgent>();
    }
    void CompleteAction()
    {
        previousAction = currentAction;
        previousTarget = currentAction.target;
        currentAction.PostPerform(this);
        if(!currentAction.defaultOwner){
            allAvailableActions.Remove(currentAction);
            previousAction.RemoveOwnership(this);
        }
        invoked = false;

    }

    void GetDefaultActions()
    {
        Actions[] myActions = FindObjectsOfType<Actions>();
        foreach (Actions action in myActions)
        {
            if (action.defaultOwner == this)
            {
                allAvailableActions.Add(action);
                action.SetupOwnership(this);
            }
        }
    }
    void LateUpdate()
    {

        Time.timeScale = worldSpeed;
        if (currentAction && currentAction.target != null)
        {
            target = currentAction.target;
        }
        if (currentAction != null && !CheckForActionCompletion()) //&& currentAction.running
        {
            return;
        }
        if (actionQueue != null && actionQueue.Count == 0)
        {
            DeletePlanner();
        }
        if (actionQueue != null && actionQueue.Count > 0)
        {
            ExecutePlan();
        }
        tickCounter = 0;
    }

    private bool CheckForActionCompletion()
    {
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget < DistanceFromTarget)//currentAction.agent.hasPath &&  // && distanceToTarget < DistanceFromTarget && || currentAction.activatingAction
        {
            if (!invoked)
            {
                Invoke("CompleteAction", currentAction.duration);
                invoked = true;
                return true;
            }
        }
        return false;
    }

    private void ExecutePlan()
    {
        
        currentAction = actionQueue.Dequeue();
        if (currentAction.PrePerform())
        {
            if (currentAction.defaultTarget != null) //might have to add some else ifs?
            {
                target = currentAction.defaultTarget;
            }
            if (currentAction.target != null)
            {
                target = currentAction.target;
            }
            if (currentAction.freeTargets.Count > 0)
            {
                float bestDistance = Mathf.Infinity;
                int bestTarget = 0;
                for (int i = 0; i < currentAction.freeTargets.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, currentAction.freeTargets[i].transform.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTarget = i;
                    }
                }
                target = currentAction.freeTargets[bestTarget];
                currentAction.RemoveAvailableTarget(currentAction.freeTargets[bestTarget]);
            }
            agent.SetDestination(target.transform.position);
        }
        else
        {
            actionQueue = null;
        }
    }

    private void DeletePlanner()
    {
        if (currentGoal.remove)
        {
            goals.Remove(currentGoal);
        }
        for (int i = 0; i < allAvailableActions.Count; i++)
        {
            allAvailableActions[i].ResetCost();
            if (!allAvailableActions[i].defaultOwner)
            {

                RemoveUnusedActions(i);
            }

        }
        previousAction = null;
        previousTarget=null;
        actionsInPlan.Clear();
        planner = null;
        hasGoal = false;
        canPlan = true;
        failedGoalsList.Clear();
    }

    private void CreatePlan()
    {
        planner = new Planner();
        List<Actions> relevantActions = new List<Actions>();
        var sortedGoals = from entry in goals orderby entry.Value descending select entry;
        //implement priority system here possibly remove queque above
        Actions[] AllActions = FindObjectsOfType<Actions>();
        foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
        {
            GetRelevantActions(relevantActions, AllActions, subGoal);
            SetActionCosts(relevantActions);
            actionQueue = planner.Plan(relevantActions, subGoal.Key.subGoals, beliefs, this.transform);
            if (actionQueue != null)
            {
                actionsInPlan = actionQueue.ToList<Actions>();
                for (int i = 0; i < allAvailableActions.Count; i++)
                {
                    allAvailableActions[i].ResetCost();
                    if (!allAvailableActions[i].defaultOwner && !actionsInPlan.Contains(allAvailableActions[i]))
                    {
                        RemoveUnusedActions(i);
                    }
                }
                canPlan = false;
                currentGoal = subGoal.Key;
                goals.Clear();
                break;
            }
            else
            {
                for (int i = 0; i < allAvailableActions.Count; i++)
                {
                    allAvailableActions[i].ResetCost();
                    if (!allAvailableActions[i].defaultOwner)
                    {
                        RemoveUnusedActions(i);
                    }
                }
                failedGoalsList.Add(subGoal.Key);
                canPlan = true;
                goals.Clear();
                break;
            }
        }

    }

    private void GetRelevantActions(List<Actions> relevantActions, Actions[] AllActions, KeyValuePair<SubGoal, int> subGoal)
    {
        foreach (Actions action in AllActions)
        {
            if (action.goalsRelatedTo.Contains(subGoal.Key.keyword) && !action.defaultOwner && action.currentOwners.Count < action.maxOwners)
            {
                allAvailableActions.Add(action);
            }
        }
        foreach (Actions action in allAvailableActions)
        {
            if (action.goalsRelatedTo.Contains(subGoal.Key.keyword))
            {
                relevantActions.Add(action);
                action.SetupOwnership(this);
            }
        }
    }

    private void SetActionCosts(List<Actions> relevantActions)
    {
        foreach (Actions action in relevantActions)
        {
            if(action.activatingAction){
                continue;
            }
            else if (action.defaultTarget != null)
            {
                float distance = Vector3.Distance(transform.position, action.defaultTarget.transform.position);
                action.cost += distance;
            }
            else if (action.targetTag != "")
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag(action.targetTag);
                float bestDistance = Mathf.Infinity;
                int bestTarget = 0;
                for (int i = 0; i < targets.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, targets[i].transform.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTarget = i;
                    }
                }
                action.cost = bestDistance;
            }
        }
    }

    private void RemoveUnusedActions(int i)
    {
        allAvailableActions[i].RemoveOwnership(this);
        allAvailableActions.Remove(allAvailableActions[i]);
    }

    public void AddSubGoal(string goal, int value, bool canBeDeleted, string keyword)
    {
        SubGoal subGoalToAdd = new SubGoal(goal, value, canBeDeleted, keyword);
        if (failedGoalsList.Count > 0)
        {
            foreach (SubGoal sGoal in failedGoalsList)
            {
                if (sGoal.keyword == keyword)
                {
                    return;
                }

            }
        }
        if (goals.Count > 0)
        {
            foreach (KeyValuePair<SubGoal, int> Sgoal in goals)
            {
                if (Sgoal.Key.keyword == keyword){
                    return;
                }
            }
        }

        
        if (canPlan)
        {
            goals.Add(subGoalToAdd, 5); //what is the number? to eliminate queue and potentially crashing put this inside the canplan check
            CreatePlan();
        }

    }

    public void GetPlanInformation()
    {
        string planToDisplay = "My plan is: \n";
        if (actionsInPlan.Count > 0)
        {
            foreach (Actions action in actionsInPlan)
            {
                planToDisplay += action.actionName + ".\n";
            }
        }
        else {
            planToDisplay += "There is no plan";
        }
        StatusUI.statusUIInstance.UpdatePlanWindow(planToDisplay);
    }
}
