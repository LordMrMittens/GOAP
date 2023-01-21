using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TirednessModule : BasicNeedModule
{
    NavMeshAgent agent;
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }
    public override void ConsumeResource(float value)
    {
        if (agent.velocity != Vector3.zero)
        {
            value += value;
        }
        currentResource -= value;
        if (currentResource < 0)
        {
            currentResource = 0;

        }
        hasResource = currentResource > 0;

    }
}
