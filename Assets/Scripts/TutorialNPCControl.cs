using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPCControl : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    TutorialController.instance.ShowStatsPanelGuide();
                }
            }
        }
    }
}
