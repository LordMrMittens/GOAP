using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestCharacter : NPCController
{

    protected override void Start()
    {
        base.Start();
    }

    public void GetHungry()
    {
        beliefs.AddSingleState("IsHungry", 0);
        AddSubGoal("HasEaten", 0, true, "Food");
        Debug.Log("Triggering Hunger");
    }

    void GetTired()
    {
        beliefs.AddSingleState("IsTired", 0);
        AddSubGoal("IsRested", 0, true, "Rest");
        Debug.Log("Triggering Rest");
    }
    void GetThirsty()
    {
        beliefs.AddSingleState("IsThirsty", 0);
        AddSubGoal("QuenchThirst", 0, true, "Drink");
        Debug.Log("Triggering Drink");
    }
    void GetTooHot()
    {
        CheckForJacket();
        beliefs.AddSingleState("IsTooHot", 0);
        AddSubGoal("GetCold", 0, true, "Temperature");
        Debug.Log("Triggering temp hot");
    }
    void GetTooCold()
    {
        CheckForJacket();
        beliefs.AddSingleState("IsTooCold", 0);
        AddSubGoal("GetWarm", 0, true, "Temperature");
        Debug.Log("Triggering temp cold");
    }

        public void CheckForJacket()
    {
        if (nPCInventory.itemsEquipped.Contains("Jacket"))
        {
            if (beliefs.HasState("IsWearingJacket") == false)
            {
                beliefs.ChangeState("IsWearingJacket", 0);
            }
            beliefs.RemoveState("IsNotWearingJacket");
        }
        else
        {
            if (beliefs.HasState("IsNotWearingJacket") == false)
            {
                beliefs.ChangeState("IsNotWearingJacket", 0);
            }
            beliefs.RemoveState("IsWearingJacket");
        }
    }
}
