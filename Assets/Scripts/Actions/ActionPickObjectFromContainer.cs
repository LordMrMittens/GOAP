using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPickObjectFromContainer : Actions
{
    [SerializeField] bool pickAllObjectsPossible = false;
    [SerializeField] int numberOfItemsToTake;
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (pickAllObjectsPossible)
        {
            ContainerObject targetContainer = target.GetComponent<ContainerObject>();
            for (int i = 0; i < numberOfItemsToTake; i++)
            {
                if (targetContainer.RemoveObject(relatedItemIfAvailable))
                {
                    _nPCController.nPCInventory.DepositObject(relatedItemIfAvailable);
                }
            }
            return true;

        }
        else
        {
            if (target.GetComponent<ContainerObject>().RemoveObject(relatedItemIfAvailable))
            {
                _nPCController.nPCInventory.DepositObject(relatedItemIfAvailable);
                return true;
            }
        }
        return false;
    }
}
