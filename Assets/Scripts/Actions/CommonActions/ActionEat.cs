using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEat : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {   
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        _nPCController.beliefs.RemoveState("HasFood");
        _nPCController.beliefs.RemoveState("IsHungry");
        _nPCController.needsManager.SatiateHunger();
        _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
        return true;
    }
}
