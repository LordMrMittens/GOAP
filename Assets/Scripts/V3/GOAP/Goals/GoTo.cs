using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : BaseGoal
{
    [SerializeField] float minThresholdToStartAction = 1.5f;
    [SerializeField] float thresholdToStopAction = 1f;
    [SerializeField] int maxPriority = 30;

    DetectableTarget CurrentTarget;
    int currentPriority = 0;

    public Vector3 MoveToTarget (){
        if (CurrentTarget != null){
            return CurrentTarget.transform.position;
        } else {
            return transform.position;
        }
    }

    public override void OnUpdate()
    {
        currentPriority = 0;
        if (sensors.ActiveTargets == null || sensors.ActiveTargets.Count == 0)
        {
            return;
        }
        if (CurrentTarget != null)
        {
            foreach (var target in sensors.ActiveTargets.Values)
            {
                if (target.Detectable == CurrentTarget)
                {
                    if (target.Awareness < thresholdToStopAction)
                    {
                        return;
                    }
                    else
                    {
                        currentPriority = maxPriority;
                        return;
                    }

                }
            }
            CurrentTarget = null;
        }
        foreach (var target in sensors.ActiveTargets.Values)
        {
            if (target.Awareness >= minThresholdToStartAction)
            {
                CurrentTarget = target.Detectable;
                currentPriority = maxPriority;
                return;
            }
        }
    }
    public override void OnGoalActivated(BaseAction _linkedAction)
    {
    }
    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        CurrentTarget = null;
    }
    public override int OnCalculatePriority()
    {

        return currentPriority;
    }


    public override bool CanDo()
    {

        if (sensors.ActiveTargets == null || sensors.ActiveTargets.Count == 0)
        {
            return false;
        }
        foreach (var target in sensors.ActiveTargets.Values)
        {
            if (target.Awareness >= minThresholdToStartAction)
            {
                return true;
            }
        }
        return true;
    }
}
