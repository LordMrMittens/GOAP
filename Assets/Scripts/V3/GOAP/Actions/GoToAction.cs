using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAction : BaseAction
{

    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(GoTo)});

    GoTo goToTarget;
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
        goToTarget = (GoTo)linkedGoal;
        agent.MoveTo(goToTarget.MoveToTarget());

    }

    public override void OnActionDeactivated()
    {
        base.OnActionDeactivated();
        goToTarget = null;
    }

    public override void OnActionUpdate()
    {
        agent.MoveTo(goToTarget.MoveToTarget());
    }
}
