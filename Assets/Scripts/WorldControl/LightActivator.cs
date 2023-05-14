using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightActivator : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] GameObject roof;
    int peopleInBuilding;
    private void Start() {
        lights.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "NPC")
        {
            lights.gameObject.SetActive(true);
            roof.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "NPC")
        {
            lights.gameObject.SetActive(false);
            roof.gameObject.SetActive(true);
        }
    }


}
