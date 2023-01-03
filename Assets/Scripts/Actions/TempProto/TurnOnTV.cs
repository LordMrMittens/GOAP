using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnTV : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        belief.RemoveState("IsBored");
        return true;
    }
}
