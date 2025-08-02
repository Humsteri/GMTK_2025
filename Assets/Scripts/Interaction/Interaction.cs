using System;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    ActionNotifier actionNotifier => ActionNotifier.Instance;
    InputManager inputManager => InputManager.Instance;
    [SerializeField] GameObject interactionObj;
    TextMeshProUGUI interactionText;
    [SerializeField] GameObject collidingObj;
    void Start()
    {
        interactionText = interactionObj.GetComponentInChildren<TextMeshProUGUI>();
        interactionObj.SetActive(false);
        ActionNotifier.Instance.DialogueEnable += DialogueStarted;
    }
    void OnDestroy()
    {
        ActionNotifier.Instance.DialogueEnable -= DialogueStarted;
    }
    private void DialogueStarted(bool obj)
    {
        interactionObj.SetActive(!obj);
    }

    void Update()
    {
        if (collidingObj != null && inputManager.Interact)
        {
            switch (collidingObj.tag)
            {
                case "NPC":
                    NPC _npc = collidingObj.GetComponent<NPC>();
                    actionNotifier.NpcInteract?.Invoke(_npc.dialogue, _npc.npcType.ToString());
                    _npc.Interacted();
                    break;
                case "SymbolPuzzle":
                    actionNotifier.Puzzle?.Invoke(Enums.Puzzles.SymbolPuzzle);
                    break;
                case "Key1":
                    actionNotifier.Item?.Invoke(Enums.Items.Key1);
                    Destroy(collidingObj);
                    ActivateInteractionText(false, "");
                    break;
                case "ColorChange":
                    actionNotifier.OpenColorChange?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    void ActivateInteractionText(bool enable, string txt)
    {
        interactionObj.SetActive(enable);
        interactionText.text = txt;
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "NPC":
                NPC _npc = other.GetComponent<NPC>();
                ActivateInteractionText(true, $"Space to interact with {_npc.npcType.ToString()}");
                break;
            case "SymbolPuzzle":
                ActivateInteractionText(true, "Space to interact with Puzzle");
                break;
            case "Key1":
                ActivateInteractionText(true, "Space to interact with Key");
                break;
            case "ColorChange":
                ActivateInteractionText(true, "Space to interact with lanterns");
                break;
            default:
                break;
        }
    }
    void OnTriggerStay(Collider other)
    {
        collidingObj = other.gameObject;
        
    }
    void OnTriggerExit(Collider other)
    {
        collidingObj = null;
        ActivateInteractionText(false, "");
    }
}
