using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBedAction : BaseAction
{

    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(GoToBedGoal)});

    GoToBedGoal goToTarget;
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
        goToTarget = (GoToBedGoal)linkedGoal;
        agent.MoveTo(goToTarget.FindBed());

    }

    public override void OnActionDeactivated()
    {
        base.OnActionDeactivated();
        goToTarget = null;
    }

    public override void OnActionUpdate()
    {
        if (agent.AtDestination){ //let agent check if at destination
            Rest();
        }
    }

    void Rest(){

        goToTarget.StartCoroutine("Rest");
    }
}
