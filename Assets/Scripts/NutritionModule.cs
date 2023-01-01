using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionModule : MonoBehaviour
{
    float maxNutrition = 100;
    [ field : SerializeField] public float currentNutrition {get; private set;}
    [field : SerializeField] public float backgroundDecayRate = .01f;
     
    public bool hasNutrition;
    
    void Start()
    {
        currentNutrition = maxNutrition;
    }

    public void ConsumeEnergy(float value)
    {
        currentNutrition -= value;
        if (currentNutrition < 0)
        {
            currentNutrition = 0;
        }
        hasNutrition = currentNutrition > 0;
    }

    public void AddEnergy(float value)
    {
        currentNutrition += value;
        if (currentNutrition > maxNutrition)
        {
            currentNutrition = maxNutrition;
        }
        hasNutrition = currentNutrition > 0;
        Debug.Log("Hunger Satiated");
    }
}
