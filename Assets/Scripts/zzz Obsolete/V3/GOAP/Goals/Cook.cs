using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : BaseGoal
{
    [SerializeField] float timeEating = 5f;
    [SerializeField] float buildRate = .2f;

    Transform CurrentTarget;
    Transform LookAtTarget;
    float currentPriority = 0;

    bool IsEating = false;

    public Vector3 FindFood(){
        return GameObject.FindGameObjectWithTag("Food").transform.position;
    }
        public Vector3 FindTable(){
        return GameObject.FindGameObjectWithTag("Table").transform.position;
    }

    public override void OnUpdate()
    {
        if(!IsEating)
        {currentPriority += buildRate * Time.deltaTime;}
    }
    public override void OnGoalActivated(BaseAction _linkedAction)
    {
        base.OnGoalActivated(_linkedAction);
        currentPriority =100;
    }
    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        CurrentTarget = null;
    }
    public override int OnCalculatePriority()
    {

        return Mathf.FloorToInt(currentPriority);
    }


    public override bool CanDo()
    {
        if(FindFood() == null || FindTable()==null)
        {
            return false;
        }
        return true;
    }

    public IEnumerator GetSomeFood(){
        IsEating = true;
        agent.SetLookAtTarget(FindTable());
        yield return new WaitForSeconds(timeEating);
        currentPriority = 0;
        IsEating = false;
        yield return null;
    }
}
