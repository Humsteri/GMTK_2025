using UnityEngine;

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

    [Header("Symbol Puzzle")]
    public bool Next => InputActions.SymbolPuzzle.Next?.WasPressedThisFrame() ?? false;
    public bool Previous => InputActions.SymbolPuzzle.Previous?.WasPressedThisFrame() ?? false;
    public bool Forward => InputActions.SymbolPuzzle.Forward?.WasPressedThisFrame() ?? false;
    public bool Backwards => InputActions.SymbolPuzzle.Backwards?.WasPressedThisFrame() ?? false;
    public bool Confirm => InputActions.SymbolPuzzle.Confirm?.WasPressedThisFrame() ?? false;
    void Start()
    {
        InputActions.Player.Enable();
    }

    public void EnableSymbolInputs() {
        InputActions.SymbolPuzzle.Enable();
    }

    public void DisableSymbolInputs() {
        InputActions.SymbolPuzzle.Disable();
    }
}
