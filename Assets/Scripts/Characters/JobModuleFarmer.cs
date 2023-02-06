using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobModuleFarmer : BaseJobModule
{
 [SerializeField] int maxProductCarried;
 [SerializeField] string itemBeingFarmed;
 NPCInventory nPCInventory;


    protected override void Update()
    {
        if (nPCInventory == null)
        {
            nPCInventory = GetComponent<NPCInventory>();
        }
        if (!nightShift)
        {
            if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart && WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
            {
                isAtWork = true;
            }
            else
            {
                isAtWork = false;
            }
        }
        else if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart || WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
        {
            isAtWork = true;
        }
        else
        {
            isAtWork = false;
        }
        if (isAtWork )
        {
            if (nPCInventory.itemsEquipped.Contains(itemBeingFarmed))
            {
                List<string> itemsInInventory = new List<string>();
                foreach (string item in nPCInventory.itemsEquipped)
                {
                    if (item == itemBeingFarmed)
                    {
                        itemsInInventory.Add(item);
                    }
                }
                if (itemsInInventory.Count < maxProductCarried)
                {
                    nPCController.beliefs.AddSingleState($"ShouldPickProduct", 0);
                }
                else
                {
                    nPCController.beliefs.RemoveState($"ShouldPickProduct");
                    nPCController.beliefs.AddSingleState($"ShouldDepositProduct", 0);
                }
            }
            else
            {
                nPCController.beliefs.AddSingleState($"ShouldPickProduct", 0);
            }
        }
        else
        {
            if (nPCInventory.itemsEquipped.Contains(itemBeingFarmed))
            {
                nPCController.beliefs.RemoveState($"ShouldPickProduct");
                nPCController.beliefs.AddSingleState($"ShouldDepositProduct", 0);
            } else {
                nPCController.beliefs.RemoveState($"ShouldDepositProduct");
                nPCController.beliefs.RemoveState($"ShouldPickProduct");
            }

        }
    }
}
