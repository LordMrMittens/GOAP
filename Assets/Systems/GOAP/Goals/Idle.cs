using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseGoal
{
    [SerializeField] int Priority = 10;
    public override int OnCalculatePriority()
    {
        return Priority;
    }

    public override bool CanDo()
    {
        return true;
    }
}
