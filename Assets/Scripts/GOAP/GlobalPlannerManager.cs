using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlannerManager : MonoBehaviour
{
    public static GlobalPlannerManager GPMInstance;
    NPCController currentController;
    public int maxSimultaneousPlanners;
    //public List<NPCController> queuedPlanners = new List<NPCController>();
    public Queue<NPCController> queuedPlanners = new Queue<NPCController>();
    public int currentNumberOfPlanners;
    public bool currentlyPlanning = false;

    private void Awake() {
        GPMInstance = this;
    }
    private void Update() {

        if(queuedPlanners.Count > 0 && !currentlyPlanning){
        Invoke("DequeuePlanner",.5f);
        }
    }

    public void EnqueuePlanner(NPCController controllerToEnqueue){
        queuedPlanners.Enqueue(controllerToEnqueue);
    }
    public void DequeuePlanner(){    
        if(queuedPlanners.Count > 0 )
{        currentController = queuedPlanners.Dequeue();
        currentController.canPlan = true;}
    }
}
