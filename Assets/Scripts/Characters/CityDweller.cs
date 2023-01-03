using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityDweller : NPCController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal(SetGoal, 1, true);
        goals.Add(s1,5);

        base.Start();
        SubGoal s2 = new SubGoal("IsAtHome", 1, true);
        goals.Add(s2,5);
    }
        
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
