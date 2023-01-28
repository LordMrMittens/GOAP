using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseCharacter : NPCController
{
    
    protected override void Start()
    {
        base.Start();
    }

    public void BeIdle()
    {
        AddSubGoal("Wander", 0, true, "Idle");
    }

    public void GetHungry()
    {
        beliefs.AddSingleState("IsHungry", 0);
        AddSubGoal("HasEaten", 0, true, "Food");
    }

    void GetTired()
    {
        beliefs.AddSingleState("IsTired", 0);
        AddSubGoal("IsRested", 0, true, "Rest");
    }
    void GetThirsty()
    {
        beliefs.AddSingleState("IsThirsty", 0);
        AddSubGoal("QuenchThirst", 0, true, "Drink");
    }
    void GetTooHot()
    {
        CheckForJacket();
        beliefs.AddSingleState("IsTooHot", 0);
        AddSubGoal("GetCold", 0, true, "Temperature");
    }
    void GetTooCold()
    {
        CheckForJacket();
        beliefs.AddSingleState("IsTooCold", 0);
        AddSubGoal("GetWarm", 0, true, "Temperature");
    }

    void PrepForNightTime()
    {
            beliefs.AddSingleState("IsTooDark", 0);
            AddSubGoal("GetLight", 0, true, "Light");
    }
    void PrepForDayTime()
    {
            beliefs.AddSingleState("IsTooBright", 0);
            AddSubGoal("SnuffLight", 0, true, "Light");
    }


    void RestockFood()
    {
        AddSubGoal("StoredFood", 0, true, "Groceries");
    }
        void RestockDrink()
    {
        AddSubGoal("StoredDrink", 0, true, "Groceries");
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
