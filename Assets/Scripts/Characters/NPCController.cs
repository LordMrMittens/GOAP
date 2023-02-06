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
    ExcelImporter excelImporter;
    NavMeshAgent agent;
    public List<Actions> allAvailableActions = new List<Actions>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs = new WorldStates();
    public GameObject target;
    Planner planner;
    public Queue<Actions> actionQueue;
    List<Actions> actionsInPlan = new List<Actions>();
    public Actions currentAction;
    public SubGoal currentGoal;
    public bool invoked = false;
    [SerializeField] float DistanceFromTarget = 1.3f;
    public NeedsManager needsManager;
    public NPCInventory nPCInventory;
    public float tickFrequency = 1f;
    public float tickCounter { get; set; }
    public bool hasGoal { get; set; }
    public bool canPlan = true;
    public List<SubGoal> failedGoalsList = new List<SubGoal>();
    float failedTaskListResetTimer = 0;
    float failedTaskListResetFrequency = 5;
    public string jobGoalRelatedTo; // eg MarketJob

    protected virtual void Start()
    {
        AssignName();
        agent = GetComponent<NavMeshAgent>();
    }

    private void AssignName()
    {
        excelImporter = FindObjectOfType<ExcelImporter>();
        if (excelImporter.text.ContainsKey("Names"))
        {
            int randomName = Random.Range(0, excelImporter.text["Names"].Count);
            this.gameObject.name = excelImporter.text["Names"][randomName];
            excelImporter.text["Names"].RemoveAt(randomName);
        }
    }

    void CompleteAction()
    {
        currentAction.RemoveOwnership(this);
        currentAction.PostPerform(this);
        allAvailableActions.Remove(currentAction);
        invoked = false;
    }
    void CancelCurrentAction()
    {
        currentAction.RemoveOwnership(this);
        foreach (Actions action in actionsInPlan)
        {
            action.RemoveOwnership(this);
        }
        allAvailableActions.Clear();
        actionsInPlan.Clear();
        invoked = false;
    }

    void LateUpdate()
    {
        failedTaskListResetTimer += Time.deltaTime;
        if (currentAction && currentAction.target != null)
        {
            target = currentAction.target;
        }
        if (currentAction != null && currentAction.currentOwners.Contains(this)) //&& currentAction.running
        {
            CheckForActionCompletion();
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
        if (failedTaskListResetTimer > failedTaskListResetFrequency)
        {
            failedGoalsList.Clear();
            failedTaskListResetTimer = 0;
        }
    }

    private void CheckForActionCompletion()
    {
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget < DistanceFromTarget || currentAction.activatingAction)
        {
            if (!invoked)
            {
                Invoke("CompleteAction", currentAction.duration);
                invoked = true;
            }
        }
    }

    private void ExecutePlan()
    {

        currentAction = actionQueue.Dequeue();
        if (currentAction.PrePerform())
        {
            if (currentAction.goalsRelatedTo.Contains("Idle"))
            {
                target = currentAction.freeTargets[Random.Range(0, currentAction.freeTargets.Count)];
            }
            else
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
            RemoveUnusedActions(i);

        }
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
            actionQueue = planner.Plan(relevantActions, subGoal.Key.subGoals, beliefs, this.transform, this);
            if (actionQueue != null)
            {
                actionsInPlan = actionQueue.ToList<Actions>();
                foreach (Actions actionInPlan in actionsInPlan)
                {
                    actionInPlan.SetupOwnership(this);
                }
                for (int i = 0; i < allAvailableActions.Count; i++)
                {
                    allAvailableActions[i].ResetCost();
                    if (!actionsInPlan.Contains(allAvailableActions[i]) || !relevantActions.Contains(allAvailableActions[i]))
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

                    RemoveUnusedActions(i);

                }
                //Debug.Log($" {gameObject.name} has No Plan for {subGoal.Key.keyword}");
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
            if (action.defaultOwner == this)
            {
                allAvailableActions.Add(action);
            }
        }
        foreach (Actions action in allAvailableActions)
        {
            if (action.goalsRelatedTo.Contains(subGoal.Key.keyword))
            {
                relevantActions.Add(action);
                //action.SetupOwnership(this); might be causing issues
            }
        }
    }

    private void SetActionCosts(List<Actions> relevantActions)
    {
        foreach (Actions action in relevantActions)
        {
            if (action.activatingAction)
            {
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
                if (Sgoal.Key.keyword == keyword)
                {
                    return;
                }
            }
        }
        if (currentGoal != null && currentAction != null)
        {
            foreach (KeyValuePair<string, int> cGoal in currentGoal.subGoals)
            {
                if (cGoal.Value < value && !currentAction.activatingAction) //if the goals priority is higher than the previous goal priority then change goals
                {
                    DeletePlanner();
                    CancelCurrentAction();
                    currentGoal=null;
                    goals.Clear();
                }
            }
        }
        if (canPlan)
        {
            //TODO Why does this Dictionary / Queue crash? 
            //temp (permanent?) fix, put this inside the canplan check
            goals.Add(subGoalToAdd, value);
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
        else
        {
            planToDisplay += "There is no plan";
        }
        StatusUI.statusUIInstance.UpdatePlanWindow(planToDisplay);
    }
}
