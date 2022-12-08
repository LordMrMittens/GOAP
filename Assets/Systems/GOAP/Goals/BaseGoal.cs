using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseGoal{
    bool CanDo();
    int OnCalculatePriority();
    void OnUpdate();
    void OnGoalActivated(BaseAction _linkedAction);
    void OnGoalDeactivated();
    
}
public class BaseGoal : MonoBehaviour, IBaseGoal
{
    [SerializeField] protected string givenName;
    protected CharacterAgent agent;
    protected AwarenessSystem sensors;
    protected GOAPUI debugUI;

    protected BaseAction LinkedAction;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<CharacterAgent>();
        sensors = GetComponent<AwarenessSystem>();
    }

    void Start()
    {
        debugUI = FindObjectOfType<GOAPUI>();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        debugUI.UpdateGoal(this, GetType().Name,LinkedAction ? "Active": "Paused", OnCalculatePriority(), givenName);
    }
    public virtual bool CanDo()
    {
        return false;
    }

    public virtual int OnCalculatePriority()
    {
        return -1;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnGoalActivated(BaseAction _linkedAction)
    {
        LinkedAction = _linkedAction;
    }
    public virtual void OnGoalDeactivated()
    {
        LinkedAction = null;
    }
}
