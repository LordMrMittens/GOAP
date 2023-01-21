using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDrink : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        _nPCController.beliefs.RemoveState("IsThirsty");
        _nPCController.beliefs.RemoveState("HasDrink");
        _nPCController.needsManager.QuenchThirst();
        _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
        return true;
    }
}
