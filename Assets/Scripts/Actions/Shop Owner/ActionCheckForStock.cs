using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCheckForStock : Actions
{
    List< NPCController> allNPCsWithThisAction = new List<NPCController>();

    void Start()
    {
        NPCController[] allNPCs = FindObjectsOfType<NPCController>();
        foreach (NPCController NPC in allNPCs)
        {
            foreach (string keyword in goalsRelatedTo)
            {
                if( NPC.jobGoalRelatedTo == keyword)
                {
                    allNPCsWithThisAction.Add(NPC);
                }
            }
        }
    }
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (!target.GetComponent<ContainerObject>().storedObjects.Contains(relatedItemIfAvailable) || target.GetComponent<ContainerObject>().storedObjects.Count < 20)
        {
            foreach (NPCController NPC in allNPCsWithThisAction)
            {
                _nPCController.beliefs.AddSingleState($"ShopHasNo{relatedItemIfAvailable}Stored", 0);
            }
        }
        _nPCController.beliefs.RemoveState($"ShouldCheck{relatedItemIfAvailable}Stock");
        return true;
    }
}
