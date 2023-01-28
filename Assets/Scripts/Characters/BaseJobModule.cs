using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseJobModule : MonoBehaviour
{
    public int shiftStart;
    public int shiftEnd;
    public bool nightShift = false;

    public NPCController nPCController { get; set; }
    public bool isAtWork;

    float callBackToWorkTimer = 10;
    float timeOffWorkAllowed = 0;
    protected virtual void Awake()
    {
        nPCController = GetComponent<NPCController>();
    }
    protected virtual void Update()
    {
        if (!nightShift)
        {
            if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart && WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
            {
                UpdateWorkStatus();
            }
            else
            {
                nPCController.beliefs.RemoveState($"ShouldBeWorking");
                isAtWork = false;
            }
        }
        else if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart || WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
        {
            UpdateWorkStatus();
        }
        else
        {
            nPCController.beliefs.RemoveState($"ShouldBeWorking");
            isAtWork = false;
        }
    }

    private void UpdateWorkStatus()
    {
        List<string> goalsOfCurrentAction = new List<string>();
        if (nPCController.currentAction)
        {
            for (int i = 0; i < nPCController.currentAction.goalsRelatedTo.Length; i++)
            {
                goalsOfCurrentAction.Add(nPCController.currentAction.goalsRelatedTo[i]);
            }
            if (!goalsOfCurrentAction.Contains(nPCController.jobGoalRelatedTo))
            {
                callBackToWorkTimer += Time.deltaTime;
                if (callBackToWorkTimer > timeOffWorkAllowed)
                {
                    nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0);
                    timeOffWorkAllowed = 0;
                }
            }
            else
            {
                nPCController.beliefs.RemoveState($"ShouldBeWorking");
                timeOffWorkAllowed = 0;
            }
        } else{
            nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0);
        }
        isAtWork = true;
    }
}
