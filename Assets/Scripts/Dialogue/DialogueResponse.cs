using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;
    [Serializable]
    public class ResponseEvent : UnityEvent
    {
    }
    [SerializeField]
    private ResponseEvent responseEvent = new ResponseEvent();
    public ResponseEvent responseEventSetter
    {
        get
        {
            return responseEvent;
        }
        set
        {
            responseEvent = value;
        }
    }
}