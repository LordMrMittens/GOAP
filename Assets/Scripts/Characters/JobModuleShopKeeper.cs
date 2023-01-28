using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobModuleShopKeeper : BaseJobModule
{
    float stockCheckTimer;
    [SerializeField] float stockCheckFrequency;
    [SerializeField] string[] stockItemsToCheck;


    protected override void Update()
    {
        base.Update();
        if (isAtWork)
        {
            if (stockCheckTimer > stockCheckFrequency)
            {
                foreach (string itemTocheckFor in stockItemsToCheck)
                {
                    nPCController.beliefs.AddSingleState($"ShouldCheck{itemTocheckFor}Stock", 0);
                }
                stockCheckTimer = 0;
            }
            stockCheckTimer += Time.deltaTime;
        }
    }
}
