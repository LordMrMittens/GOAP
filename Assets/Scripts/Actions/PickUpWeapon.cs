using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : Actions
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> activeWeaponsToTake = new List<GameObject>();
    List<GameObject> inactiveWeaponsToTake = new List<GameObject>();

    public override bool PrePerform()
    {
        TakeWeapon();
        return true;
    }
    public override bool PostPerform()
    {
        return true;
    }
    void TakeWeapon(){
        if(activeWeaponsToTake.Count>0){

            GameObject takenWeapon = activeWeaponsToTake[activeWeaponsToTake.Count - 1];
            activeWeaponsToTake.Remove(takenWeapon);
            inactiveWeaponsToTake.Add(takenWeapon);
            takenWeapon.SetActive(false);
            //this.GetComponent<NPCInventory>().EquipSword();
        }
    }
}
