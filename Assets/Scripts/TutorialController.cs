using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    [SerializeField] GameObject WelcomePanel;
    [SerializeField] GameObject StatsPanelGuide;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        WorldStatusManager.WSMInstance.timeSpeed=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStatsPanelGuide(){
        StatsPanelGuide.SetActive(true);
    }
}
