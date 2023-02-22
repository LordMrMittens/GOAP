using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComentaryDialogueUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI jobText;
    [SerializeField] TextMeshProUGUI nightOwlText;
    [SerializeField] TextMeshProUGUI dialogueText;
     
    [SerializeField] GameObject requestPanel;
    [SerializeField] TextMeshProUGUI requestDialogueText;
    
    public void UpdateNameText(string _nPCName, string _nPCJob, string nightOwl = "")
    {
        nameText.text = _nPCName;
        jobText.text = "Job: " + _nPCJob;
        nightOwlText.text = nightOwl;
    }
        public void UpdateDialogueText(string _dialogue)
    {
        dialogueText.text = _dialogue;
    }

    public void ActivateRequestPanel(string _dialogue)
    {
        requestPanel.SetActive(true);
        requestDialogueText.text = _dialogue;
    }
    public void DeactivateRequestPanel()
    {
        requestPanel.SetActive(false);
        requestDialogueText.text = "";
    }
}
