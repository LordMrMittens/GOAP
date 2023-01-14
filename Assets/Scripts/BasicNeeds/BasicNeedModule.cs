using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNeedModule : MonoBehaviour
{
    [SerializeField] string displayName;
    public float maxResource = 100;
    public float minResource = 0;
    [ field : SerializeField] public float currentResource {get; private set;}
    [field : SerializeField] public float backgroundDecayRate = .01f;
     
    public bool hasResource;
    
    void Start()
    {
        currentResource = maxResource;
    }

    public void ConsumeResource(float value)
    {
        currentResource -= value;
        if (currentResource < 0)
        {
            currentResource = 0;
        }
        hasResource = currentResource > 0;
        UpdateUIDisplay();
    }

    public void AddResource(float value)
    {
        currentResource += value;
        if (currentResource > maxResource)
        {
            currentResource = maxResource;
        }
        hasResource = currentResource > 0;
    }
    public void UpdateUIDisplay(){
        StatusUI.statusUIInstance.UpdateGoal(this, displayName, currentResource);
    }
}
