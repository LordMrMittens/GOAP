using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearJacket : Actions
{

    // Start is called before the first frame update
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (target.GetComponent<ContainerObject>().RemoveObject(relatedItemIfAvailable))
        {
            _nPCController.nPCInventory.DepositObject(relatedItemIfAvailable);
            _nPCController.needsManager.ToggleJacket(true);
        }
        _nPCController.beliefs.ChangeState("IsWearingJacket", 0);
        _nPCController.beliefs.RemoveState("IsTooCold");
        _nPCController.beliefs.RemoveState("IsNotWearingJacket");
        return true;
    }
}
