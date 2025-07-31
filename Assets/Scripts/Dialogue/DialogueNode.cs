using System;
using System.Collections.Generic;
 
[System.Serializable]
public class DialogueNode
{
    public string DialogueText;
    public List<DialogueResponse> Responses = new List<DialogueResponse>();
 
    internal bool IsLastNode()
    {
        return Responses.Count <= 0;
    }
}