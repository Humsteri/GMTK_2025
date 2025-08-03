using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue secondDialogue;
    public NpcType npcType;
    bool hasBeenInteractedWith = false;
    public enum NpcType
    {
        JapMeister,
        Minotaur,
        Door
    }
    public void Interacted()
    {
        hasBeenInteractedWith = true;
        if (npcType == NpcType.Minotaur)
        {
            dialogue = secondDialogue;
        }
    }
}
