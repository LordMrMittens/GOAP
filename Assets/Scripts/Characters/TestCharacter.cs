using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestCharacter : NPCController
{

    protected override void Start()
    {
        base.Start();

        /*
        SubGoal s1 = new SubGoal("IsRested", 0, false, "Rest");
        goals.Add(s1, 5);
        SubGoal s2 = new SubGoal("HasEaten", 0, false, "Food");
        goals.Add(s2, 5);
        SubGoal s3 = new SubGoal("GetWarm", 0, false, "Temperature");
        goals.Add(s3, 5);
        SubGoal s4 = new SubGoal("GetCold", 0, false, "Temperature");
        goals.Add(s4, 5);
        SubGoal s5 = new SubGoal("QuenchThirst", 0, false, "Drink");
        goals.Add(s5, 5);
        */
    }

    public void GetHungry()
    {
        beliefs.ChangeState("IsHungry", 0);
        AddSubGoal("IsRested", 0, true, "Rest");
    }

    void GetTired()
    {
        beliefs.ChangeState("IsTired", 0);
        AddSubGoal("HasEaten", 0, false, "Food");
    }
    void GetThirsty()
    {
        beliefs.ChangeState("IsThirsty", 0);
    }
    void GetTooHot()
    {
        CheckForJacket();
        beliefs.ChangeState("IsTooHot", 0);
    }
    void GetTooCold()
    {
        CheckForJacket();
        beliefs.ChangeState("IsTooCold", 0);
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
