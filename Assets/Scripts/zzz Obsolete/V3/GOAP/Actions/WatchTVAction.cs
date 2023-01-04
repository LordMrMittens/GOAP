using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTVAction : BaseAction
{

    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(WatchTVGoal)});

    WatchTVGoal goToTarget;
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
        goToTarget = (WatchTVGoal)linkedGoal;
        agent.MoveTo(goToTarget.FindSofa().transform.position);

    }

    public override void OnActionDeactivated()
    {
        base.OnActionDeactivated();
        goToTarget = null;
    }

    public override void OnActionUpdate()
    {
        if (agent.AtDestination){ //let agent check if at destination
            WatchTv();
        }
    }

    void WatchTv(){

        goToTarget.StartCoroutine("WatchSomeTV");
    }
}
