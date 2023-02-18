using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionShopTool : Actions
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (target.GetComponent<ContainerObject>().RemoveObject(relatedItemIfAvailable))
        {
            _nPCController.nPCInventory.DepositObject(relatedItemIfAvailable);
            _nPCController.nPCInventory.RestoreTool();
            _nPCController.beliefs.RemoveState("NeedTool");
            return true;
        }
        return false;
    }
}
