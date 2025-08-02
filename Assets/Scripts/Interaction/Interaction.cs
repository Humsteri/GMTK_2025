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
                    ActivateInteractionText(true, $"Space to interact with {_npc.npcType.ToString()}");
                    break;
                case "SymbolPuzzle":
                    actionNotifier.Puzzle?.Invoke(Enums.Puzzles.SymbolPuzzle);
                    ActivateInteractionText(true, "Space to interact with Puzzle");
                    break;
                case "Key1":
                    actionNotifier.Item?.Invoke(Enums.Items.Key1);
                    ActivateInteractionText(true, "Space to interact with item");
                    break;
                case "ColorChange":
                    actionNotifier.OpenColorChange?.Invoke();
                    ActivateInteractionText(true, "Space to interact with lanterns");
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
        print(other.tag);
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
                ActivateInteractionText(true, "Space to interact with item");
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
