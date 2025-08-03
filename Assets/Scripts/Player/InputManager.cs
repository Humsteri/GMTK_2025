using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Instance
    public static InputManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        InputActions = new InputSystem_Actions();
    }
    #endregion
    public InputSystem_Actions InputActions;

    public bool W => InputActions.Player.W?.WasPressedThisFrame() ?? false;
    public bool S => InputActions.Player.S?.WasPressedThisFrame() ?? false;
    public bool A => InputActions.Player.A?.WasPressedThisFrame() ?? false;
    public bool D => InputActions.Player.D?.WasPressedThisFrame() ?? false;
    public bool Interact => InputActions.Player.Interact?.WasPressedThisFrame() ?? false;
    public bool Esc => InputActions.Tutorial.Esc?.WasPressedThisFrame() ?? false;
    public bool TutorialInteraction => InputActions.Tutorial.Interact?.WasPressedThisFrame() ?? false;

    [Header("Dialogue")]
    public bool DialogueUp => InputActions.Dialogue.MoveUp?.WasPressedThisFrame() ?? false;
    public bool DialogueDown => InputActions.Dialogue.MoveDown?.WasPressedThisFrame() ?? false;
    public bool DialogueRight => InputActions.Dialogue.MoveRight?.WasPressedThisFrame() ?? false;
    public bool DialogueLeft => InputActions.Dialogue.MoveLeft?.WasPressedThisFrame() ?? false;
    public bool DialogueSelect => InputActions.Dialogue.Select?.WasPressedThisFrame() ?? false;

    [Header("Symbol Puzzle")]
    public bool SymbolNext => InputActions.SymbolPuzzle.Next?.WasPressedThisFrame() ?? false;
    public bool SymbolPrevious => InputActions.SymbolPuzzle.Previous?.WasPressedThisFrame() ?? false;
    public bool SymbolForward => InputActions.SymbolPuzzle.Forward?.WasPressedThisFrame() ?? false;
    public bool SymbolBackwards => InputActions.SymbolPuzzle.Backwards?.WasPressedThisFrame() ?? false;
    public bool SymbolConfirm => InputActions.SymbolPuzzle.Confirm?.WasPressedThisFrame() ?? false;

    [Header("Color Picker")]
    public bool ColorNext => InputActions.ColorPicker.Next?.WasPressedThisFrame() ?? false;
    public bool ColorPrevious => InputActions.ColorPicker.Previous?.WasPressedThisFrame() ?? false;
    public bool ColorConfirm => InputActions.ColorPicker.Select?.WasPressedThisFrame() ?? false;
    InputActionMap activeMap;
    void Start()
    {
        InputActions.Player.Enable();
        InputActions.Tutorial.Enable();
    }
    public void EnableTutorialPrompt()
    {
        if (InputActions.Player.enabled)
        {
            activeMap = InputActions.Player;
        }
        if (InputActions.Dialogue.enabled)
        {
            activeMap = InputActions.Dialogue;
        }
        if (InputActions.SymbolPuzzle.enabled)
        {
            activeMap = InputActions.SymbolPuzzle;
        }
        InputActions.Player.Disable();
        InputActions.Dialogue.Disable();
        InputActions.SymbolPuzzle.Disable();
        
    }
    public void DisableTutorialPrompt()
    {
        activeMap.Enable();
    }
    public void EnableDialogue()
    {
        InputActions.Player.Disable();
        InputActions.Dialogue.Enable();
    }
    public void DisableDialogue()
    {
        InputActions.Player.Enable();
        InputActions.Dialogue.Disable();
    }
    public void EnableSymbolInputs()
    {
        InputActions.Player.Disable();
        InputActions.SymbolPuzzle.Enable();
    }
    public void DisableSymbolInputs() {
        InputActions.Player.Enable();
        InputActions.SymbolPuzzle.Disable();
    }
    public void EnableColorPicker() {
        InputActions.ColorPicker.Enable();
        InputActions.Player.Disable();
    }
    public void DisableColorPicker() {
        InputActions.ColorPicker.Disable();
        InputActions.Player.Enable();
    }

}
