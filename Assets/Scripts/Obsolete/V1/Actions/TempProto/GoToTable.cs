using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTable : Actions
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
        Debug.Log("Table postState");
        return true;
    }
}
