using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDropObjectAtContainer : Actions
{
    [SerializeField] bool dropAllObjects = false;
    public override bool PrePerform()
    {

        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        if (dropAllObjects)
        {
            if (target.GetComponent<ContainerObject>().DepositObject(relatedItemIfAvailable))
            {
                List<string> itemsInInventory = new List<string>();

                foreach (string item in _nPCController.nPCInventory.itemsEquipped)
                {
                    if (item == relatedItemIfAvailable)
                    {
                        itemsInInventory.Add(item);
                    }
                }
                for (int i = 0; i < itemsInInventory.Count; i++)
                {
                    target.GetComponent<ContainerObject>().DepositObject(relatedItemIfAvailable);
                    _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
                }
            }
        }
        else
        {
            if (target.GetComponent<ContainerObject>().DepositObject(relatedItemIfAvailable))
            {
                _nPCController.nPCInventory.RemoveObject(relatedItemIfAvailable);
            }
        }
        return true;
    }
}

