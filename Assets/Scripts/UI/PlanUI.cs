using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlanUI : MonoBehaviour
{
     [SerializeField] TextMeshProUGUI planText;
     public void UpdatePlanText(string _plan){
        planText.text = _plan;
     }
}
