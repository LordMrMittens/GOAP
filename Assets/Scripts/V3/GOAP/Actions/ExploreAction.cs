using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreAction : BaseAction
{
    [SerializeField] float exploreRange = 20f;

    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(Explore)});
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
        Vector3 targetLocation = agent.PickLocationInRange(exploreRange);
        agent.MoveTo(targetLocation);

    }

    public override void OnActionDeactivated()
    {

    }

    public override void OnActionUpdate()
    {
        if (agent.AtDestination){ //let agent check if at destination
            OnActionActivated(linkedGoal);
        }
    }
}
