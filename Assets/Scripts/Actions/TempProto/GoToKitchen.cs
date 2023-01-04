using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToKitchen : Actions
{
    // Start is called before the first frame update
    public override bool PrePerform()
    {

        return true;
    }
    public override bool PostPerform()
    {
        belief.ChangeState("HasFood", 0);
        return true;
    }
}
