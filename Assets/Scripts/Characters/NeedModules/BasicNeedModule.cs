using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNeedModule : MonoBehaviour
{
    [field :SerializeField] public string displayName {get; private set;}
    public float maxResource = 100;
    public float minResource = 0;
    [ field : SerializeField] public float currentResource {get; set;}
    [field : SerializeField] public float backgroundDecayRate = .01f;
    public bool isCommonNeed;
    [SerializeField] float randomnessOffset;
    public bool hasResource;

    public string resourceType {get; set;} // this is just used for the dialogue
    
    protected virtual void Start()
    {
        currentResource = Random.Range(minResource + randomnessOffset, maxResource-randomnessOffset);
    }

    public virtual void ConsumeResource(float value)
    {
        currentResource -= value;
        if (currentResource < 0)
        {
            currentResource = 0;

        }
        hasResource = currentResource > 0;
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
}
