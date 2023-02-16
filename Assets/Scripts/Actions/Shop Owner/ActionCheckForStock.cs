using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCheckForStock : Actions
{
    List< NPCController> allNPCsWithThisAction = new List<NPCController>();

    public string[] relatedItemsIfAvailable;

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
    protected override bool CheckIfItemsAvailable(NPCController _nPCController)
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        foreach (string relatedItem in relatedItemsIfAvailable)
        {
            if (!target.GetComponent<ContainerObject>().storedObjects.Contains(relatedItem) || target.GetComponent<ContainerObject>().storedObjects.Count < 20)
            {
                foreach (NPCController NPC in allNPCsWithThisAction)
                {
                    _nPCController.beliefs.AddSingleState($"ShopHasNo{relatedItem}Stored", 0);
                }
            }
            _nPCController.beliefs.RemoveState($"ShouldCheck{relatedItem}Stock");
        }
        return true;
    }
}
