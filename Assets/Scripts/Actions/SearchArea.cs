using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchArea : Actions
{
    [SerializeField] float walkableRadius;
    public override bool PrePerform()
    {
        ChooseRandomDir();
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        return true;
    }
    void ChooseRandomDir()
    {
        Vector3 randomDir = Random.insideUnitSphere * walkableRadius;
        randomDir += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, walkableRadius, 1);
        Vector3 finalPosition = hit.position;
        //agent.SetDestination(finalPosition);
    }
}
