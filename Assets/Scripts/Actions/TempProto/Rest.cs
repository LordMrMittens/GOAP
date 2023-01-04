using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        belief.RemoveState("IsTired");
        needsManager.Invoke("RestoreEnergy", 0);
        return true;
    }
}
