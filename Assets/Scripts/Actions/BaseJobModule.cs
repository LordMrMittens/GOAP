using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseJobModule : MonoBehaviour
{
    [SerializeField] int shiftStart;
    [SerializeField] int shiftEnd;
    [SerializeField] bool nightShift = false;

    public NPCController nPCController { get; set; }
    public bool isAtWork;
    [SerializeField] float minimumJobDistance = 2;
    [SerializeField] Transform jobLocation;
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
                float distanceFromJob = Vector3.Distance(transform.position, jobLocation.transform.position);
                if (distanceFromJob > minimumJobDistance)
                {
                    nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0);
                }
                else
                {
                    nPCController.beliefs.RemoveState($"ShouldBeWorking");
                }
                isAtWork = true;
            }
            else
            {
                nPCController.beliefs.RemoveState($"ShouldBeWorking");
                isAtWork = false;
            }
        }
        else if (WorldStatusManager.WSMInstance.timeOfDay > shiftStart || WorldStatusManager.WSMInstance.timeOfDay < shiftEnd)
        {
            float distanceFromJob = Vector3.Distance(transform.position, jobLocation.transform.position);
            if (distanceFromJob > minimumJobDistance)
            { nPCController.beliefs.AddSingleState($"ShouldBeWorking", 0); }
            else
            {
                nPCController.beliefs.RemoveState($"ShouldBeWorking");
            }
            isAtWork = true;
        }
        else
        {
            nPCController.beliefs.RemoveState($"ShouldBeWorking");
            isAtWork = false;
        }
    }
}
