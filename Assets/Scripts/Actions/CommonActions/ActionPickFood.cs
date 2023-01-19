using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPickFood : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        if (target.GetComponent<ContainerObject>().RemoveObject(relatedItemIfAvailable))
        {
            nPCInventory.DepositObject(relatedItemIfAvailable);
            return true;
        }

        return false;
    }
}
