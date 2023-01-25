using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseJobModule : MonoBehaviour
{
    [SerializeField] int shiftStart;
    [SerializeField] int shiftEnd;
    [SerializeField] bool nightShift = false;
    NPCController nPCController;
    public bool isAtWork;
    protected virtual void Start()
    {
        nPCController = GetComponent<NPCController>();
    }
    protected virtual void Update()
    {
        if (!nightShift)
        {
            if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart && WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
            {
                nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0);
                isAtWork = true;
            } else {
                nPCController.beliefs.RemoveState($"ShouldBeWorking");
                isAtWork = false;
            }
        }
        else if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart || WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
        {
            nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0);
            isAtWork = true;
        } else {
            nPCController.beliefs.RemoveState($"ShouldBeWorking");
            isAtWork = false;
        }
    }
}
