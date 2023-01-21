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
    public override bool PostPerform(NPCController _nPCController)
    {
        if (target.GetComponent<ContainerObject>().DepositObject(relatedItemIfAvailable))
        {
            _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
            _nPCController.needsManager.ToggleJacket(false);
        }
        _nPCController.beliefs.ChangeState("IsNotWearingJacket", 0);
        _nPCController.beliefs.RemoveState("IsWearingJacket");
        _nPCController.beliefs.RemoveState("IsTooHot");
        return true;
    }
}
