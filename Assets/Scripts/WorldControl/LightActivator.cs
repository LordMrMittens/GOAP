using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightActivator : MonoBehaviour
{
    [SerializeField] GameObject lights;
    int peopleInBuilding;
    private void Start() {
        lights.gameObject.SetActive(false);
    }
    private void Update() {
        if(peopleInBuilding > 0 && lights.gameObject.activeSelf == false){
            lights.gameObject.SetActive(true);
        } else if (peopleInBuilding <= 0 && lights.gameObject.activeSelf == true){
            lights.gameObject.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "NPC"){
            peopleInBuilding++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            peopleInBuilding--;
        }
    }


}
