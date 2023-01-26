using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTendToShop : Actions
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        _nPCController.canPlan = true;
        return true;
    }
}
