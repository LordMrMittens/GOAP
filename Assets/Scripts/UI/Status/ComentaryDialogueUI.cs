using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComentaryDialogueUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    
    public void UpdateNameText(string _nPCName)
    {
        nameText.text = _nPCName;
    }
        public void UpdateDialogueText(string _dialogue)
    {
        dialogueText.text = _dialogue;
    }
}
