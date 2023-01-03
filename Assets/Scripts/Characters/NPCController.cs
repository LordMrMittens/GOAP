using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SubGoal
{
    public Dictionary<string, int> subGoals;
    public bool remove;

    public SubGoal(string s, int i, bool b)
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(s, i);
        remove = b;
    }
}

public class NPCController : MonoBehaviour
{
    public string SetGoal;
    public List<Actions> actions = new List<Actions>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public Inventory inventory = new Inventory();
    public WorldStates beliefs = new WorldStates();
    [SerializeField] GameObject ActionHolder;

    Planner planner;
    public Queue<Actions> actionQueue;
    public Actions currentAction;
    SubGoal currentGoal;
    public bool invoked = false;

    [SerializeField] float DistanceFromTarget =1.3f;

    public NeedsManager needsManager;
    public NPCInventory nPCInventory;

    protected virtual void Start()
    {
        Actions[] myActions = ActionHolder.GetComponents<Actions>();
        foreach (Actions action in myActions)
        {
            actions.Add(action);
        }
    }
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
            if (currentAction.agent.hasPath && distanceToTarget < DistanceFromTarget || currentAction.activatingAction && distanceToTarget <DistanceFromTarget)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;

                }
            }
            return;
        }
        
        if (planner == null || actionQueue == null)
        {
            planner = new Planner();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
            {
                actionQueue = planner.Plan(actions, subGoal.Key.subGoals, beliefs);
                if (actionQueue != null)
                {
                    currentGoal = subGoal.Key;
                    break;
                }
            }
        }
        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }
        if (actionQueue != null && actionQueue.Count > 0 ){
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform()){
                if (currentAction.target == null && currentAction.targetTag != ""){
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                    //Debug.Log(currentAction.name + "Target is null" + currentAction.agent.destination);
                }
                if (currentAction.target != null){
                    currentAction.running=true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                    //Debug.Log(currentAction.name + "Target isn't null" + currentAction.agent.destination);
                }
            } else {
                //Debug.Log(currentAction.name + "Ending Queue");
                actionQueue = null;
            }
        }
    }

}
