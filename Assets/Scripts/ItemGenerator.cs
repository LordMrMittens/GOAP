using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] string itemToGenerate;
    [SerializeField] int amountToGenerate;
    [SerializeField] float generationFrequency;
    float generationTimer;
    ContainerObject containerObject;
    void Start()
    {
        containerObject = GetComponent<ContainerObject>();
    }
    void Update()
    {
        generationTimer += Time.deltaTime;

        if(generationTimer > generationFrequency){

            for (int i = 0; i < amountToGenerate; i++)
            {
                containerObject.DepositObject(itemToGenerate);
            }
            generationTimer=0;
        }
    }
}
