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
        ContainerObject container = target.GetComponent<ContainerObject>();
        foreach (string relatedItem in relatedItemsIfAvailable)
        {
            if (container.storedObjects.Contains(relatedItem))
            {
                int itemCounter = 0;
                foreach (string item in container.storedObjects)
                {
                    if (item == relatedItem)
                    {
                        itemCounter++;
                    }
                }
                if (itemCounter < 20)
                {
                    _nPCController.beliefs.AddSingleState($"ShopHasNo{relatedItem}Stored", 0);
                }
            }
            else
            {
                foreach (NPCController NPC in allNPCsWithThisAction)
                {
                    _nPCController.beliefs.AddSingleState($"ShopHasNo{relatedItem}Stored", 0);
                }
            }
        }
        _nPCController.beliefs.RemoveState($"ShouldCheckStock");
        return true;
    }
}
