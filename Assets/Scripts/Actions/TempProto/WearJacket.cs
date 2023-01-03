using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearJacket : Actions
{

    // Start is called before the first frame update
    public override bool PrePerform()
    {
        if (target.GetComponent<BaseContainer>().RemoveObject(relatedItemIfAvailable))
        {
            nPCInventory.DepositObject(relatedItemIfAvailable);
            needsManager.ToggleJacket(true);
        }
        return true;
    }
    public override bool PostPerform()
    {
        belief.ChangeState("IsWearingJacket", 0);
        belief.RemoveState("IsTooCold");
        belief.RemoveState("IsNotWearingJacket");
        return true;
    }
}
