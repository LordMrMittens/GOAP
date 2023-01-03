using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOff : Actions
{
    // Start is called before the first frame update
    [SerializeField] Light lightbulb;
    public override bool PrePerform()
    {
        SwitchLightOff();
        return true;
    }
    public override bool PostPerform()
    {
            World.Instance.GetWorld().ChangeState("LightOff", 1);
            World.Instance.GetWorld().RemoveState("LightOn");
        return true;
    }
    public void SwitchLightOff()
    {

            lightbulb.gameObject.SetActive(false);

    }

}
