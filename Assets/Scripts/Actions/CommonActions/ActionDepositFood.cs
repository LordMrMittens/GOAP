using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDepositFood : Actions
{
    public override bool PrePerform()
    {

        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (target.GetComponent<ContainerObject>().DepositObject(relatedItemIfAvailable))
        {
            _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
            _nPCController.beliefs.RemoveState($"HasNo{relatedItemIfAvailable}Stored");
        }
    return true;
    }
}
