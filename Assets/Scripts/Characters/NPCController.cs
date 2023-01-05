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
    public Actions previousAction { get; private set; }
    public GameObject previousTarget { get; private set; }
    SubGoal currentGoal;
    public bool invoked = false;

    [SerializeField] float DistanceFromTarget = 1.3f;

    public NeedsManager needsManager;
    public NPCInventory nPCInventory;

    public float tickFrequency = 1f;
    float tickCounter;

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
        previousAction = currentAction;
        previousTarget = currentAction.target;
        currentAction.running = false;
        currentAction.PostPerform();
        currentAction.target = null; //so that it may choose a different target when several present
        invoked = false;

    }
    void LateUpdate()
    {
        tickCounter += Time.deltaTime;
        if (tickCounter > tickFrequency)
        {
            if (currentAction != null && currentAction.running)
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
                tickCounter = 0;

            }
            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }
            if (actionQueue != null && actionQueue.Count > 0)
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
            tickCounter = 0;
        }

    }

}
