using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDropTool : Actions
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
        }
        _nPCController.beliefs.RemoveState("ShouldStoreTool");
        return true;
    }
}
