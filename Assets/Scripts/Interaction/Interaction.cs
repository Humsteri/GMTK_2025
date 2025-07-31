using UnityEngine;

public class Interaction : MonoBehaviour
{
    ActionNotifier actionNotifier => ActionNotifier.Instance;
    InputManager inputManager => InputManager.Instance;
    GameObject collidingObj;
    void Update()
    {
        if (collidingObj != null && inputManager.Interact)
        {
            switch (collidingObj.tag)
            {
                case "NPC":
                    actionNotifier.JapMaster?.Invoke();
                    break;
                case "SymbolPuzzle":
                    actionNotifier.Puzzle?.Invoke(Enums.Puzzles.SymbolPuzzle);
                    break;
                case "Key1":
                    actionNotifier.Item?.Invoke(Enums.Items.Key1);
                    break;
                default:
                    break;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        collidingObj = other.gameObject;
    }
    void OnTriggerExit(Collider other)
    {
        collidingObj = null;
    }
}
