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
        inputActions = new InputSystem_Actions();
    }
    #endregion
    public InputSystem_Actions inputActions;

    public bool W => inputActions.Player.W?.WasPressedThisFrame() ?? false;
    public bool S => inputActions.Player.S?.WasPressedThisFrame() ?? false;
    public bool A => inputActions.Player.A?.WasPressedThisFrame() ?? false;
    public bool D => inputActions.Player.D?.WasPressedThisFrame() ?? false;
    void Start()
    {
        inputActions.Player.Enable();
    }
}
