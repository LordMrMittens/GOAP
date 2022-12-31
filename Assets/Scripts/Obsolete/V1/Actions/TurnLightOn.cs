using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOn : Actions
{
    // Start is called before the first frame update
    [SerializeField] Light lightbulb;
    public override bool PrePerform()
    {
        SwitchLightOn();
        return true;
    }
    public override bool PostPerform()
    {
            World.Instance.GetWorld().ChangeState("LightOn", 1);
            World.Instance.GetWorld().RemoveState("LightOff");
        return true;
    }
    public void SwitchLightOn()
    {

            lightbulb.gameObject.SetActive(true);

    }

}
