using TMPro;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText => GetComponentInChildren<TextMeshProUGUI>();
    public NpcDialogue SetText(string txt)
    {
        dialogueText.text = txt;
        return this;
    }
}
