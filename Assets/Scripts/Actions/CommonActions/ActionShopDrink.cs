using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionShopDrink : Actions
{
    // Start is called before the first frame update
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
