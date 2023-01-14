using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NeedUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI needName;
    [SerializeField] RectTransform priority;
    [SerializeField] TextMeshProUGUI needPoints;
    public void UpdateGoalInfo(string _name, float _priority)
    {
        needName.text = _name;
        needPoints.text = Mathf.CeilToInt(_priority).ToString();
        priority.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _priority*2.7f);
    }
}
