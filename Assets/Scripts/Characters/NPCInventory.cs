using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    public List<string> itemsEquipped = new List<string>();
    ItemDisplayManager itemDisplayManager;
    NPCController nPCController;
    float maxToolDurability = 100;
    float minToolDurability = 0;
    float currentToolDurability;
    public GameObject[] maleNPCModels;
    public GameObject[] femaleNPCModels;
    private void Start()
    {
        currentToolDurability = 100;
        
    }
    private void Update()
    {
        if (nPCController == null){
        nPCController = GetComponent<NPCController>();
        }
       
        if (itemDisplayManager == null)
        {
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
    public void ConsumeToolDurability(float durabilityConsumed)
    {
        if (itemsEquipped.Contains("Tool"))
        {
            currentToolDurability -= durabilityConsumed;
            if (currentToolDurability < minToolDurability)
            {
                BreakTool();
            }
        }
    }

    void BreakTool()
    {
        if (itemsEquipped.Contains("Tool"))
        {
            RemoveObject("Tool");
        }
        
        nPCController.beliefs.AddSingleState("NeedTool", 5);
    }
    public void RestoreTool()
    {
        currentToolDurability = maxToolDurability;
    }

    public bool CheckForTool()
    {
        if (!itemsEquipped.Contains("Tool"))
        {
            if (currentToolDurability < minToolDurability)
            {
                BreakTool();
            }
            return false;
        }
        return true;
    }
    public bool CheckForLight()
    {
        if (itemsEquipped.Contains("Light"))
        {
            return true;
        }
        return false;
    }
    public bool CheckForJacket()
    {
        if (itemsEquipped.Contains("Jacket"))
        {
            return true;
        }
        return false;
    }
}
