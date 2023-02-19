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

    protected override bool CheckIfItemsAvailable(NPCController _nPCController)
    {
        if (targetTag == "")
        {
            return base.CheckIfItemsAvailable(_nPCController);
        }
        else
        {
            CheckTargetAvailability();
            foreach (GameObject target in freeTargets)
            {
                if (target.GetComponent<ContainerObject>().storedObjects.Contains(relatedItemIfAvailable))
                {
                    return true;
                }
            }
            return false;
        }
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
                    if (multipleTargetsAreContainers)
                    {
                        _nPCController.nPCInventory.ConsumeToolDurability(10);
                        //ResetTarget(); reseting target may not be needed
                    }
                }
            }
            return true;

        }
        else
        {
            if (target.GetComponent<ContainerObject>().RemoveObject(relatedItemIfAvailable))
            {
                _nPCController.nPCInventory.DepositObject(relatedItemIfAvailable);
                if (multipleTargetsAreContainers)
                {
                    _nPCController.nPCInventory.ConsumeToolDurability(10);
                    //ResetTarget();reseting target may not be needed
                }
                return true;
            }
        }


        return false;
    }

    void CheckTargetAvailability(){
        foreach (GameObject target in targetsInUse)
        {
            if(target.GetComponent<ContainerObject>().storedObjects.Count >0){
                freeTargets.Add(target);
                targetsInUse.Remove(target);
                break;
            }
        }
    }
}
