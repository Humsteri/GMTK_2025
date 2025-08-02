using UnityEngine;

public class CursorLocker : MonoBehaviour {
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
