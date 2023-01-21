using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingZone : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        World.Instance.GetWorld().ChangeState("Tourist_Waiting",1);
        World.Instance.AddTourist(this.gameObject);
        belief.ChangeState("AtWaitingArea",1);
        return true;
    }
}
