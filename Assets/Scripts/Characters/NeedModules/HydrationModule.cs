using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrationModule : BasicNeedModule
{
    protected override void Start()
    {
        base.Start();
        resourceType = "Hydration";
    }

}
