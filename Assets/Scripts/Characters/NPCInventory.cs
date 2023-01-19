using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    public List<string> itemsEquipped = new List<string>();

    public void DepositObject(string objectToAdd)
    {
        itemsEquipped.Add(objectToAdd);
    }
    public void RemoveObject(string objectToRemove)
    {
        if (itemsEquipped.Contains(objectToRemove))
        {
            itemsEquipped.Remove(objectToRemove);
        }
    }
}
