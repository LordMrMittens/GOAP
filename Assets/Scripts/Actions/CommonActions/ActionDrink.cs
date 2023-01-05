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
    public override bool PostPerform()
    {
        belief.RemoveState("IsThirsty");
        needsManager.QuenchThirst();
        return true;
    }
}
