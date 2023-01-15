using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public Actions action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Actions action) {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        this.action = action;
    }
        public Node(Node parent, float cost, Dictionary<string, int> allStates,Dictionary<string, int> beliefStates, Actions action) {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);

        
        foreach(KeyValuePair<string, int> belief in beliefStates){
            if (!this.state.ContainsKey(belief.Key))
                this.state.Add(belief.Key,belief.Value);
        }
        this.action = action;
    }
}
public class Planner
{

    Dictionary<Actions, string> testDict = new Dictionary<Actions, string>();
    public Queue<Actions> Plan(List<Actions> actions, Dictionary<string,int> goal, WorldStates beliefStates){
        
        List<Actions> doableActions = new List<Actions>();

        foreach (Actions action in actions)
        {
            if (action.IsAchievable()){

                doableActions.Add(action);
            }
        }
        List<Node> leaves = new List<Node>();
        
        Node start = new Node(null, 0, World.Instance.GetWorld().GetAllStates(),beliefStates.GetAllStates(),null);

        bool success = BuildGraph(start, leaves, doableActions, goal);

        if(!success){
            Debug.Log("No Plan");
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
 //       Debug.Log("The plan is: ");
 //       foreach (Actions action in queue)
  //      {
//            Debug.Log($"Q: {action.actionName}");
//        }
        return queue;
    }

    bool BuildGraph(Node parent, List<Node> leaves, List<Actions> doableActions, Dictionary<string, int> goal){
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
                    Node node = new Node(parent, parent.cost + action.cost, currentState, action);
                    if(GoalAchieved(goal, currentState)){
                        leaves.Add(node);
                        foundPath = true;
                    } else {
                        List<Actions> subset = ActionSubset(doableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal);
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
