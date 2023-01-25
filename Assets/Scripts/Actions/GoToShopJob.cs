using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToShopJob : Actions
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform(NPCController _nPCController)
    {
        return true;
    }
}
