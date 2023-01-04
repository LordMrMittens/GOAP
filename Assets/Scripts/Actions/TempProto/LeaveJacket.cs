using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveJacket : Actions
{

    // Start is called before the first frame update
    public override bool PrePerform()
    {

        return true;
    }
    public override bool PostPerform()
    {
        if (target.GetComponent<BaseContainer>().DepositObject(relatedItemIfAvailable))
        {
            nPCInventory.RemoveObject(relatedItemIfAvailable);
            needsManager.ToggleJacket(false);
        }
        belief.ChangeState("IsNotWearingJacket", 0);
        belief.RemoveState("IsWearingJacket");
        belief.RemoveState("IsTooHot");
        return true;
    }
}
