using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBedGoal : BaseGoal
{
    [SerializeField] float timeResting = 3f;
    [SerializeField] float buildRate = 1f;

    Transform CurrentTarget;
    float currentPriority = 0;

    public Vector3 FindBed(){
        return GameObject.FindGameObjectWithTag("Bed").transform.position;
    }

    public override void OnUpdate()
    {
        if (agent.isAwake){
            currentPriority += buildRate * Time.deltaTime;
            if(agent.IsMoving){
                currentPriority += (buildRate *1.5f ) *Time.deltaTime; 
            }
        }
    }
    public override void OnGoalActivated(BaseAction _linkedAction)
    {
        agent.isTired =true;
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

        return true;
    }

    public IEnumerator Rest(){

        agent.isAwake=false;
        
        yield return new WaitForSeconds(timeResting);
        currentPriority = 0;
        agent.isTired=false;
        agent.isAwake=true;
        yield return null;
    }
}
