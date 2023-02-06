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
                 if (beliefs.GetAllStates().ContainsKey("ShouldCheckFoodStock"))
            {
                CheckFoodStocks();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShouldCheckDrinkStock"))
            {
                CheckDrinkStocks();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShouldCheckCoalStock"))
            {
                CheckCoalStocks();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShouldCheckMetalStock"))
            {
                CheckMetalStocks();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShopHasNoFoodStored"))
            {
                Debug.Log("Triggering here");
                RestockStoreFood();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShopHasNoDrinkStored"))
            {
                RestockStoreDrink();
                hasGoal = true;
            }
            }
           
            tickCounter = 0;
        }
    }

    public void GetToWork()
    {
        AddSubGoal("IsWorking", 7, true, jobGoalRelatedTo);
    }
    public void CheckFoodStocks()
    {
        AddSubGoal("CheckedFoodStock", 7, true, jobGoalRelatedTo);
    }
    public void CheckDrinkStocks()
    {
        AddSubGoal("CheckedDrinkStock", 7, true, jobGoalRelatedTo);
    }
    
    public void CheckCoalStocks()
    {
        AddSubGoal("CheckedCoalStock", 7, true, jobGoalRelatedTo);
    }
    public void CheckMetalStocks()
    {
        AddSubGoal("CheckedMetalStock", 7, true, jobGoalRelatedTo);
    }
    public void RestockStoreDrink()
    {
        AddSubGoal("RestockedJobDrink", 11, true, jobGoalRelatedTo);
    }
    public void RestockStoreFood(){
        AddSubGoal("RestockedJobFood", 11, true, jobGoalRelatedTo);
    }
        public void RestockStoreCoal()
    {
        AddSubGoal("RestockedJobCoal", 11, true, jobGoalRelatedTo);
    }
    public void RestockStoreMetal(){
        AddSubGoal("RestockedJobMetal", 11, true, jobGoalRelatedTo);
    }
}
