using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDepositDrink : Actions
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
        }
    return true;
    }
}
