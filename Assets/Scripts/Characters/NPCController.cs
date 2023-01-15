using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    public List<Actions> allAvailableActions = new List<Actions>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public Inventory inventory = new Inventory();
    public WorldStates beliefs = new WorldStates();
    [SerializeField] GameObject ActionHolder;

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
        Actions[] myActions = ActionHolder.GetComponents<Actions>();
        foreach (Actions action in myActions)
        {
            allAvailableActions.Add(action);
        }
    }
    void CompleteAction()
    {
        previousAction = currentAction;
        previousTarget = currentAction.target;
        currentAction.running = false;
        currentAction.PostPerform();
        currentAction.target = null; //so that it may choose a different target when several present
        invoked = false;

    }
    void LateUpdate()
    {

        if (currentAction != null && currentAction.running)
        {
            CheckForActionCompletion();

            return;
        }
        if (hasGoal)
        {
            if (planner == null || actionQueue == null)
            {
               // CreatePlan(); //may have to activate from here?
            }

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

    private void CheckForActionCompletion()
    {
        float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
        if (distanceToTarget < DistanceFromTarget)//currentAction.agent.hasPath &&  // && distanceToTarget < DistanceFromTarget && || currentAction.activatingAction
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
            if (currentAction.defaultTarget != null)
            {
                currentAction.target = currentAction.defaultTarget;
            }
            if (currentAction.target == null && currentAction.targetTag != "")
            {
                currentAction.targets = GameObject.FindGameObjectsWithTag(currentAction.targetTag);
                if (currentAction.targets.Length > 0)
                {
                    float bestDistance = Mathf.Infinity;
                    int bestTarget = 0;
                    for (int i = 0; i < currentAction.targets.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, currentAction.targets[i].transform.position);
                        if (distance > bestDistance)
                        {
                            bestDistance = distance;
                            bestTarget = i;
                        }
                    }
                    currentAction.target = currentAction.targets[bestTarget];
                }
                //Debug.Log(currentAction.name + "Target is null" + currentAction.agent.destination);
            }
            if (currentAction.target != null)
            {
                currentAction.running = true;
                currentAction.agent.SetDestination(currentAction.target.transform.position);
                //Debug.Log(currentAction.name + "Target isn't null" + currentAction.agent.destination);
            }
            if (currentAction.target == null && currentAction.activatingAction) //target is added from previous action
            {
                currentAction.target = previousTarget;
                currentAction.agent.SetDestination(currentAction.target.transform.position);
                currentAction.running = true;
            }
        }
        else
        {
            //Debug.Log(currentAction.name + "Ending Queue");
            actionQueue = null;

        }
    }

    private void DeletePlanner()
    {
        if (currentGoal.remove)
        {
            goals.Remove(currentGoal);
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
        var sortedGoals = from entry in goals orderby entry.Value descending select entry; 
        List<Actions> relevantActions = new List<Actions>(); 
        foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
        {
            foreach (Actions action in allAvailableActions)
            {
                if (action.goalsRelatedTo.Contains(subGoal.Key.keyword))
                {
                    relevantActions.Add(action);
                }
            }
            actionQueue = planner.Plan(relevantActions, subGoal.Key.subGoals, beliefs);
            actionsInPlan = actionQueue.ToList<Actions>();
            if (actionQueue != null)
            {
                canPlan = false;
                currentGoal = subGoal.Key;
                break;
            } else {
                failedGoalsList.Add(subGoal.Key);
                canPlan = true;
            }
        }
    }

    public void AddSubGoal(string goal, int value, bool canBeDeleted, string keyword)
    {
        SubGoal subGoalToAdd = new SubGoal(goal, value, canBeDeleted, keyword);
        if (failedGoalsList.Count >0){
            foreach (SubGoal sGoal in failedGoalsList)
            {
                if(sGoal.subGoals.ContainsKey(goal)){
                    Debug.Log("failed to add goal in loop");
                    return;
                }
            }
        }
        if (goals.ContainsKey(subGoalToAdd) == false ) //|| failedGoals.Contains(subGoalToAdd.keyword) possible check? if so remove negative from fist check
        {
            Debug.Log("AddingSubgoal");
            goals.Add(subGoalToAdd, 5);
            if(canPlan){
            CreatePlan();
            Debug.Log("Creating plan");
            }
        }
    }

    public void GetStatusInformation()
    {

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
