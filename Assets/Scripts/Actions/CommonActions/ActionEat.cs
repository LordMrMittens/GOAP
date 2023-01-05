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
    public override bool PostPerform()
    {
        belief.RemoveState("HasFood");
        belief.RemoveState("IsHungry");
        needsManager.SatiateHunger();
        return true;
    }
}
