using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdleWander : Actions
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        Debug.Log("Completed Idling in action");
        return true;
    }
}
