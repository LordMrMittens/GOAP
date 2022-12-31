using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTourist : Actions
{
    GameObject resource;

    // Start is called before the first frame update
    public override bool PrePerform()
    {
        target = World.Instance.RemoveTourist();
        if (target == null)
        {
            return false;
        }
        resource = World.Instance.RemoveDeer();
        if(resource){
            inventory.AddItem(resource);
        } else{
            World.Instance.AddTourist(target);
            target = null;
            return false;
        }
        World.Instance.GetWorld().ChangeState("AvailableDeer", -1);
        return true;
    }
    public override bool PostPerform()
    {
        World.Instance.GetWorld().ChangeState("Tourist_Waiting",-1);
        if(target){
            target.GetComponent<NPCController>().inventory.AddItem(resource);
        }
        return true;
    }
}
