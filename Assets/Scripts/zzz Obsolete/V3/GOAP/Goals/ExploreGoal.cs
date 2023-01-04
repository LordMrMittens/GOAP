using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreGoal : BaseGoal
{
    [SerializeField] int minPriority = 0;
    [SerializeField] int maxPriority = 30;
    [SerializeField] float buildRate = 1f;
    [SerializeField] float decayRate = .3f;
    float currentPriority = 0f;

    public override void OnUpdate()
    {
        if (agent.IsMoving)
        {
            currentPriority -= decayRate * Time.deltaTime;
        }
        else
        {
            currentPriority += buildRate * Time.deltaTime;
        }
    }
    public override void OnGoalActivated(BaseAction _linkedAction)
    {
        base.OnGoalActivated(_linkedAction);
        currentPriority = maxPriority;
    }
    public override int OnCalculatePriority()
    {
        return Mathf.FloorToInt(currentPriority);
    }

    public override bool CanDo()
    {
        return true;
    }
}
