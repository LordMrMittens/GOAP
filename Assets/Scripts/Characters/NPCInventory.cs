using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    public List<string> itemsEquipped = new List<string>();
    ItemDisplayManager itemDisplayManager;
    private void Update() {
        if (itemDisplayManager ==null){
        itemDisplayManager = GetComponent<ItemDisplayManager>();
        }
    }
    public void DepositObject(string objectToAdd)
    {
        itemsEquipped.Add(objectToAdd);
        itemDisplayManager.DisplayObject(objectToAdd);
    }
    public void RemoveObject(string objectToRemove)
    {
        if (itemsEquipped.Contains(objectToRemove))
        {
            itemsEquipped.Remove(objectToRemove);
            itemDisplayManager.HideObject(objectToRemove);
        }
        
    }
}
