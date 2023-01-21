using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccompanyTourist : Actions
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
    public override bool PostPerform(NPCController _nPCController)
    {
        World.Instance.GetWorld().ChangeState("HuntingWithTourist",1);
        World.Instance.AddDeer(target);
        inventory.RemoveItem(target);
        World.Instance.GetWorld().ChangeState("AvailableDeer", 1);
        return true;
    }
}
