using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToForest : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {

        target = inventory.FindItemWithTag("Deer");
        if (target == null)
        {

            return false;
        }
        return true;
    }
    public override bool PostPerform()
    {
        World.Instance.GetWorld().ChangeState("CanHunt",1);
        //belief.ChangeState("IsHunting",1);
        inventory.RemoveItem(target);
        return true;
    }
}
