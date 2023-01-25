using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : BaseCharacter
{
    [SerializeField] string jobGoalRelatedTo; // eg MarketJob
    // Start is called before the first frame update
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
            tickCounter = 0;
        }
    }

    public void GetToWork()
    {
        AddSubGoal("IsWorking", 0, true, jobGoalRelatedTo);
    }
    public void CheckFoodStocks()
    {
        AddSubGoal("CheckedFoodStock", 0, true, jobGoalRelatedTo);
    }
    public void CheckDrinkStocks()
    {
        AddSubGoal("CheckedDrinkStock", 0, true, jobGoalRelatedTo);
    }
    public void RestockStoreDrink()
    {
        AddSubGoal("RestockedJobDrink", 0, true, jobGoalRelatedTo);
    }
    public void RestockStoreFood(){
        AddSubGoal("RestockedJobFood", 0, true, jobGoalRelatedTo);
    }
}
