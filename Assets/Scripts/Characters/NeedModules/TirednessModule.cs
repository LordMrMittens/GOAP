using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TirednessModule : BasicNeedModule
{
    NavMeshAgent agent;
    public bool nightOwl;
    WorldStatusManager worldStatusManager;
    protected override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        resourceType = "Rest";
        worldStatusManager = WorldStatusManager.WSMInstance;
    }
    public override void ConsumeResource(float value)
    {
        if (agent.velocity != Vector3.zero)
        {
            value += value;
        }
        if (worldStatusManager.isDark && !nightOwl)
        {
            value += value;
        }
        else if (!worldStatusManager.isDark && nightOwl)
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
