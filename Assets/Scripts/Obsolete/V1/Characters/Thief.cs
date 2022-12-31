using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Thief : NPCController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        
        SubGoal s1 = new SubGoal("IsWatchingTV", 0, false);
        goals.Add(s1,9);

        SubGoal s2 = new SubGoal("HasEaten", 0, false);
        goals.Add(s2,5);

        beliefs.ChangeState("IsHungry", 0);
        Invoke("GetHungry", Random.Range(5.0f, 7.0f));
        Invoke("GetBored", Random.Range(10f, 20f));
    }

    void GetHungry()
    {

        beliefs.ChangeState("IsHungry", 0);

        Invoke("GetHungry", Random.Range(0.0f, 20.0f));
        Debug.Log("Invoking Is Hungry");
    }

        void GetBored()
    {

        beliefs.ChangeState("IsBored", 0);
        Invoke("GetBored", Random.Range(0.0f, 20.0f));
        Debug.Log("Invoking Is Bored");
    }

    
}
