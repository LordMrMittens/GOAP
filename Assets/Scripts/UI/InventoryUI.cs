using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
     [SerializeField] TextMeshProUGUI inventoryText;
     public void UpdateInventoryText(string _inventory){
        inventoryText.text = _inventory;
     }
}
