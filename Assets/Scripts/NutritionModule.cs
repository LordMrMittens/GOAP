using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionModule : MonoBehaviour
{
    float maxNutrition = 100;
    public float currentNutrition {get; private set;}
    [SerializeField] float backgroundDecayRate = .01f;
    float timer;
     
    public bool hasNutrition;
    
    void Start()
    {
        currentNutrition = maxNutrition;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ConsumeBackgroundEnergy();

        hasNutrition = currentNutrition > 0;
    }

    private void ConsumeBackgroundEnergy()
    {
        if (timer > 1)
        {
            currentNutrition -= backgroundDecayRate;
            timer = 0;
        }
    }

    public void ConsumeEnergy(float value)
    {
        currentNutrition -= value;
        if (currentNutrition < 0)
        {
            currentNutrition = 0;
        }
    }

    public void AddEnergy(float value)
    {
        currentNutrition += value;
        if (currentNutrition > maxNutrition)
        {
            currentNutrition = maxNutrition;
        }
    }
}
