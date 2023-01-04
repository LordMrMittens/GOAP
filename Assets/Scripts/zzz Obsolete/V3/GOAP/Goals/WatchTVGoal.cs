using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTVGoal : BaseGoal
{
    [SerializeField] float timeWatching = 3f;
    [SerializeField] float buildRate = .4f;

    Transform CurrentTarget;
    Transform LookAtTarget;
    float currentPriority = 0;

    public GameObject FindSofa(){
        GameObject sofa = GameObject.FindGameObjectWithTag("Sofa");
        if(sofa != null){
        return sofa;} 
        return null;
    }
        public Vector3 FindTV(){
        return GameObject.FindGameObjectWithTag("TV").transform.position;
    }

    public override void OnUpdate()
    {
        if (agent.isAwake){
            currentPriority += buildRate * Time.deltaTime;
        }
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
        if(FindSofa() == null || FindTV()==null)
        {
            return false;
        }
        return true;
    }

    public IEnumerator WatchSomeTV(){
        agent.SetLookAtTarget(FindTV());
        yield return new WaitForSeconds(timeWatching);
        currentPriority = 0;
        yield return null;
    }
}
