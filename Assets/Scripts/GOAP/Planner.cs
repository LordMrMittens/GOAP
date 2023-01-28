using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public Actions action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Actions action, Transform _NPCTransform)
    {
        float distance = 0;
        this.parent = parent;
        if (action.defaultTarget != null && this.parent.action != null && this.parent.action.defaultTarget != null)
        {
            distance = Vector3.Distance(action.defaultTarget.transform.position, parent.action.defaultTarget.transform.position);
        }
        else if (this.parent.action == null && action.defaultTarget != null)
        {
            distance = Vector3.Distance(action.defaultTarget.transform.position, _NPCTransform.position);
        }
        this.cost = cost + distance;
        this.state = new Dictionary<string, int>(allStates);
        if (parent.action != null)
        {
            for (int i = 0; i < parent.action.nonPermanentEffects.Count; i++)
            {
               if (this.state.ContainsKey(parent.action.nonPermanentEffects[i])){
                this.state.Remove(parent.action.nonPermanentEffects[i]);
               }
            }
        }
        this.action = action;
    }
    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, Actions action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);

        foreach (KeyValuePair<string, int> belief in beliefStates)
        {
            if (!this.state.ContainsKey(belief.Key))
                this.state.Add(belief.Key, belief.Value);
        }
        this.action = action;
    }
}
public class Planner
{

    Dictionary<Actions, string> testDict = new Dictionary<Actions, string>();
    public Queue<Actions> Plan(List<Actions> actions, Dictionary<string,int> goal, WorldStates beliefStates , Transform _NPCTransform, NPCController _controller){
        
        List<Actions> doableActions = new List<Actions>();

        foreach (Actions action in actions)
        {
            if (action.IsAchievable(_controller)){

                doableActions.Add(action);
            }
        }
        List<Node> leaves = new List<Node>();
        
        Node start = new Node(null, 0, World.Instance.GetWorld().GetAllStates(),beliefStates.GetAllStates(),null);

        bool success = BuildGraph(start, leaves, doableActions, goal, _NPCTransform);

        if(!success){
            foreach (KeyValuePair<string, int> failedGoal in goal)
            {
                Debug.Log($" {_NPCTransform.gameObject.name} has No Plan for {failedGoal.Key} with the keyword {_controller.currentGoal.keyword}");
            }
            return null;
        }
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if(cheapest == null){
                cheapest = leaf;
            } else if (leaf.cost < cheapest.cost) {
                cheapest = leaf;
            }
        }
        //Debug.Log($"cheapest Leaf cost {cheapest.cost}");
        List<Actions> result = new List<Actions>();
        Node n = cheapest;
        while(n != null){
            if (n.action != null){
                result.Insert(0,n.action);
                
            }
            n = n.parent;
        }
        Queue<Actions> queue = new Queue<Actions>();
        foreach (Actions action in result)
        {
            queue.Enqueue(action);
        }
        Debug.Log("The plan is: ");
      foreach (Actions action in queue)
       {
           Debug.Log($"Q: {action.actionName}");
       }
        return queue;
    }

    bool BuildGraph(Node parent, List<Node> leaves, List<Actions> doableActions, Dictionary<string, int> goal, Transform _NPCTransform){
        bool foundPath = false;
        foreach (Actions action in doableActions)
        {
            if(action.IsAchievableGiven(parent.state)){
                Dictionary<string,int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.actionresults)
                {
                    if(!currentState.ContainsKey(effect.Key)){
                        currentState.Add(effect.Key, effect.Value);
                    }
                    Node node = new Node(parent, parent.cost + action.cost, currentState, action, _NPCTransform);
                    if(GoalAchieved(goal, currentState)){
                        leaves.Add(node);
                        foundPath = true;
                    } else {
                        List<Actions> subset = ActionSubset(doableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal, _NPCTransform);
                        if (found){
                            foundPath = true;
                        }
                    }
                }
            }
        }
        return foundPath;
    }
    bool GoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state){
        foreach (KeyValuePair<string, int> goal in goals)
        {
            if(!state.ContainsKey(goal.Key)){
                return false;
            }
        }
        return true;
    }
    List<Actions> ActionSubset(List<Actions> actions, Actions removeAction){
        List<Actions> subset = new List<Actions>();
        foreach (Actions action in actions)
        {
            if (!action.Equals(removeAction)){
                subset.Add(action);
            }
        }
        return subset;
    }

}
