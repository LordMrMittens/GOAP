using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : MonoBehaviour
{
    public List<string> typeOfObjectsContained = new List<string>();
    [SerializeField] int maxNumberOfItems = 5;
    [SerializeField] int numberOfObjectsToAdd = 1;

    public List<string> storedObjects = new List<string>();

    private void Start()
    {
        foreach (string item in typeOfObjectsContained)
        {
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                DepositObject(item);
            }
        }
    }

    public bool DepositObject(string objectToAdd)
    {
        if (typeOfObjectsContained.Contains(objectToAdd))
        {
            if (storedObjects.Count + 1 <= maxNumberOfItems)
            {
                storedObjects.Add(objectToAdd);
                return true;
            }
        }
        return false;
    }
    public bool RemoveObject(string objectToRemove){
        if (storedObjects.Contains(objectToRemove)){
            storedObjects.Remove(objectToRemove);
            return true;
        }

        return false;
    }
}
