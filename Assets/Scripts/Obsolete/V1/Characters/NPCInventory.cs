using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory:MonoBehaviour
{
    bool hasSword = false;
    [SerializeField] GameObject Sword;
    
    public void EquipSword(){
        Sword.SetActive(true);
    }
}
