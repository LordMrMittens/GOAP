using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayManager : MonoBehaviour
{
    NPCInventory nPCInventory;
    [SerializeField] GameObject[] displayableObjects;
    // Start is called before the first frame update
    void Start()
    {
        nPCInventory = GetComponent<NPCInventory>();
    }
    public void DisplayObject(string _name){
        foreach (GameObject item in displayableObjects)
        {
            if (item.name == _name)
            {
                item.SetActive(true);
                break;
            }
        }
    }
    public void HideObject(string _name)
    {
        foreach (GameObject item in displayableObjects)
        {
            if (item.name == _name)
            {
                item.SetActive(false);
                break;
            }
        }
    }
    }
