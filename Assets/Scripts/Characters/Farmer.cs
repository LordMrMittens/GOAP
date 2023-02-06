using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : BaseCharacter
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
            if (beliefs.GetAllStates().ContainsKey("ShouldPickProduct"))
            {
                //Debug.Log("Should be mining at the moment");
                PickProduct();
                hasGoal = true;
            }
            if (beliefs.GetAllStates().ContainsKey("ShouldDepositProduct"))
            {
                //Debug.Log("Should stop mining");
                DepositProduct();
                hasGoal = true;
            }
            tickCounter = 0;
        }
    }
    public void PickProduct()
    {
        AddSubGoal("PickProduct", 0, true, jobGoalRelatedTo);
    }
    public void DepositProduct()
    {
        AddSubGoal("DepositProduct", 11, true, jobGoalRelatedTo);
    }
}
