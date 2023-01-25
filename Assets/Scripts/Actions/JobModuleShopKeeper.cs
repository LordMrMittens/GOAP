using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobModuleShopKeeper : BaseJobModule
{
    [SerializeField] float stockCheckTimer;
    [SerializeField] float stockCheckFrequency;

    protected override void Update()
    {
        base.Update();
        if (isAtWork)
        {
            if (stockCheckTimer > stockCheckFrequency)
            {
                nPCController.beliefs.AddSingleState($"ShouldCheckStock", 0);
                stockCheckTimer = 0;
            }
            stockCheckTimer += Time.deltaTime;
        }
    }
}
