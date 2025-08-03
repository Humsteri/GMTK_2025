using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueResponse : MonoBehaviour
{
    public bool IsSelected = false;
    public TextMeshProUGUI playerDialogueResponseText => GetComponentInChildren<TextMeshProUGUI>();
    DialogueNode dialogueNode;
    public Color SelectedColor;
    public Color OgColor;
    Image image => GetComponent<Image>();
    public PlayerDialogueResponse SetText(DialogueResponse respondeNode)
    {
        SetSelected(IsSelected);
        playerDialogueResponseText.text = respondeNode.responseText;
        dialogueNode = respondeNode.nextNode;
        return this;
    }
    public string GetResponseText()
    {
        return playerDialogueResponseText.text;
    }
    public void SetSelected(bool enable)
    {
        IsSelected = enable;
        if (enable)
        {
            image.color = SelectedColor;
        }
        else
        {
            image.color = OgColor;
        }
    }
    public DialogueNode SelectedResponse()
    {
        return dialogueNode;
    }
}
