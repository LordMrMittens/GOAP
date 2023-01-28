using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCheckForStock : Actions
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (!target.GetComponent<ContainerObject>().storedObjects.Contains(relatedItemIfAvailable))
        {
            _nPCController.beliefs.AddSingleState($"ShopHasNo{relatedItemIfAvailable}Stored", 0);
        }
        _nPCController.beliefs.RemoveState($"ShouldCheck{relatedItemIfAvailable}Stock");
        return true;
    }
}
