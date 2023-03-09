using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    [SerializeField] GameObject WelcomePanel;
    [SerializeField] GameObject StatsPanelGuide;
    void Start()
    {
        instance = this;
        WorldStatusManager.WSMInstance.timeSpeed=0;
    }
    public void ShowStatsPanelGuide(){
        StatsPanelGuide.SetActive(true);
    }
}
