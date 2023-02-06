using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRest : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        Debug.Log("IsAsleep");
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        Debug.Log("wokeUp");
        _nPCController.beliefs.RemoveState("IsTired");
        _nPCController.needsManager.RestoreEnergy();
        return true;
    }
}
