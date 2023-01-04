using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BaseAction
{
    List<System.Type> GoalsActionAchieves = new List<System.Type>(new System.Type[] {typeof(IdleGoal)});
    public override List<System.Type> GetGoalsAchievedByAction()
    {
        return GoalsActionAchieves;
    }
    public override float GetActionCost()
    {
        return 0f;
    }
}
