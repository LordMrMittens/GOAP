using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionModule : BasicNeedModule
{
    protected override void Start()
    {
        base.Start();
        resourceType = "Nutrition";
    }
}
