using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAction : MonoBehaviour
{
    protected CharacterAgent agent;
    protected AwarenessSystem sensors;
    protected BaseGoal linkedGoal;

    private void Awake() {
        agent = GetComponent<CharacterAgent>();
        sensors = GetComponent<AwarenessSystem>();
    }

    public virtual List<System.Type> GetGoalsAchievedByAction()
    {
        return null;
    }
    public virtual float GetActionCost()
    {
        return 0f;
    }

    public virtual void OnActionActivated(BaseGoal _linkedGoal)
    {
        linkedGoal = _linkedGoal;
    }

    public virtual void OnActionDeactivated()
    {
        linkedGoal = null;
    }
    public virtual void OnActionUpdate()
    {

    }
}
