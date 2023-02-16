using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : BaseCharacter
{
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        tickCounter += Time.deltaTime;
        if (tickCounter > tickFrequency)
        {
            if (beliefs.GetAllStates().ContainsKey("ShouldBeWorking"))
            {
                GetToWork();
                hasGoal = true;
            }
            if (baseJobModule.isAtWork)
            {
                if (beliefs.GetAllStates().ContainsKey("ShouldCheckStock"))
                {
                    CheckFoodStocks();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoFoodStored"))
                {
                    RestockStoreFood();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoDrinkStored"))
                {
                    RestockStoreDrink();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoWoodStored"))
                {
                    RestockStoreWood();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoToolStored"))
                {
                    RestockStoreTool();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoMetalStored"))
                {
                    RestockStoreMetal();
                    hasGoal = true;
                }
                if (beliefs.GetAllStates().ContainsKey("ShopHasNoCoalStored"))
                {
                    RestockStoreCoal();
                    hasGoal = true;
                }
            }
            tickCounter = 0;
        }
    }

    public void GetToWork()
    {
        AddSubGoal("IsWorking", 5, true, jobGoalRelatedTo);
    }
    public void CheckFoodStocks()
    {
        AddSubGoal("CheckedStock", 7, true, jobGoalRelatedTo);
    }
    public void RestockStoreDrink()
    {
        AddSubGoal("RestockedJobDrink", 11, true, jobGoalRelatedTo);
    }
    public void RestockStoreFood()
    {
        AddSubGoal("RestockedJobFood", 11, true, jobGoalRelatedTo);
    }
    public void RestockStoreCoal()
    {
        AddSubGoal("RestockedJobCoal", 11, true, jobGoalRelatedTo);
    }
    public void RestockStoreMetal()
    {
        AddSubGoal("RestockedJobMetal", 11, true, jobGoalRelatedTo);
    }
        public void RestockStoreWood()
    {
        AddSubGoal("RestockedJobWood", 11, true, jobGoalRelatedTo);
    }
        public void RestockStoreTool()
    {
        AddSubGoal("RestockedJobTool", 15, true, jobGoalRelatedTo);
    }
}
