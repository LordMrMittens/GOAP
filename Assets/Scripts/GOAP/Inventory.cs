using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject item){
        items.Add(item);
    }

    public GameObject FindItemWithTag(string tag){
        foreach (GameObject item in items)
        {
            if(item.tag == tag){
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject item){
        int indexOfItemToRemove = -1;
        foreach (GameObject GObject in items)
        {
            indexOfItemToRemove++;
            if (GObject == item)
            break;
        }
        if (indexOfItemToRemove >= -1){
            items.RemoveAt(indexOfItemToRemove);
        }
    }



}

