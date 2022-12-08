using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{


    BaseGoal[] goals;
    BaseAction[] actions;

    BaseGoal activeGoal;
    BaseAction activeAction;

    private void Awake()
    {
        goals = GetComponents<BaseGoal>();
        actions = GetComponents<BaseAction>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var goal in goals)
        {
            goal.OnUpdate();
        }
        BaseGoal bestGoal = null;
        BaseAction bestAction = null;


        foreach (var goal in goals)
        {
            goal.OnUpdate();
            if (!goal.CanDo())
            {
                continue;
            }
            if (!(bestGoal == null || goal.OnCalculatePriority() > bestGoal.OnCalculatePriority()))
            {
                continue;
            }
            BaseAction PotentialAction = null;

            foreach (var action in actions)
            {

                if (!action.GetGoalsAchievedByAction().Contains(goal.GetType()))
                {
                    continue;
                }
                if (PotentialAction == null || action.GetActionCost() < PotentialAction.GetActionCost())
                {
                    PotentialAction = action;

                }
            }
            if (PotentialAction != null)
            {
                bestGoal = goal;
                bestAction = PotentialAction;
            }
        }

        if (activeGoal == null)
        {
            activeGoal = bestGoal;
            activeAction = bestAction;
            if (activeGoal != null)
            {
                activeGoal.OnGoalActivated(activeAction);
            }
            if (activeAction != null)
            {
                activeAction.OnActionActivated(activeGoal);
            }
        }
        else if (activeGoal == bestGoal)
        {
            if (activeAction != bestAction)
            {
                activeAction.OnActionDeactivated();
                activeAction = bestAction;
                activeAction.OnActionActivated(activeGoal);
            }
        }
        else if (activeGoal != bestGoal)
        {
            activeGoal.OnGoalDeactivated();
            activeAction.OnActionDeactivated();
            activeGoal = bestGoal;
            activeAction = bestAction;
            if (activeGoal != null)
            {
                activeGoal.OnGoalActivated(activeAction);
            }
            if (activeAction != null)
            {
                activeAction.OnActionActivated(activeGoal);
            }
        }
        if (activeAction != null)
        {
            activeAction.OnActionUpdate();
        }
    }
}
