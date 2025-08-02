using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class DialogueNode
{
    //public string DialogueText;
    public List<string> DialogueText = new();
    public List<DialogueResponse> Responses = new List<DialogueResponse>();
    [Serializable]
    public class DialogueEvent : UnityEvent
    {
    }
    [SerializeField]
    private DialogueEvent dialogueEvent = new DialogueEvent();
    public DialogueEvent dialogueEventSetter
    {
        get
        {
            return dialogueEvent;
        }
        set
        {
            dialogueEvent = value;
        }
    }
    internal bool IsLastNode()
    {
        return Responses.Count <= 0;
    }
}