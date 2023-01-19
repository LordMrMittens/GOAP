using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveLight : Actions
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
        }
        belief.RemoveState("IsTooBright");
        return true;
    }
}
