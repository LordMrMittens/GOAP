using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWater : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        belief.RemoveState("IsThirsty");
        needsManager.Invoke("QuenchThirst", 0);
        return true;
    }
}
