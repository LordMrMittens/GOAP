using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPickFood : Actions
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
            return true;
        }
        return false;
    }
}
