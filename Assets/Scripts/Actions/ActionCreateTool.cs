using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCreateTool :  Actions
{
    public string[] relatedItemsIfAvailable;
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        foreach (string relatedItem in relatedItemsIfAvailable)
        {
            containerUsed.RemoveObject(relatedItem);
        }
        _nPCController.nPCInventory.DepositObject("Tool");
        return true;
    }
        protected override bool CheckIfItemsAvailable(NPCController _nPCController)
    {
        foreach (string relatedItem in relatedItemsIfAvailable)
        {
            if (!containerUsed.storedObjects.Contains(relatedItem))
            {
                _nPCController.beliefs.RemoveState($"ShopHasNoToolStored");
                return false;
            }
        }
        return true;
    }
}
