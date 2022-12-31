using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookAction : BaseAction
{

    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(Cook)});

    Cook goToTarget;

    bool hasFood;
    public override List<System.Type> GetGoalsAchievedByAction()
    {
        return GoalsActionAchieves;
    }
    public override float GetActionCost()
    {
        return 0f;
    }
    public override void OnActionActivated(BaseGoal _linkedGoal)
    {
        base.OnActionActivated(_linkedGoal);
        goToTarget = (Cook)linkedGoal;
        Vector3 food = goToTarget.FindFood();
        agent.MoveTo(food);

    }

    public override void OnActionDeactivated()
    {
        base.OnActionDeactivated();
        goToTarget = null;
    }

    public override void OnActionUpdate()
    {
        if (agent.AtDestination && !hasFood)
        { //let agent check if at destination
        hasFood=true;
            agent.MoveTo(goToTarget.FindTable());
            
        }
        else if (agent.AtDestination && hasFood)
        {
           
            Eat();
        }
    }

    void Eat()
    {
        goToTarget.StartCoroutine("GetSomeFood");
    }
}
