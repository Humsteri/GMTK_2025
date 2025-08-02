using UnityEngine;

public class ColorPickerDoor : OpenableDoor
{
    protected override void Start() {
        base.Start();
        ActionNotifier.Instance.InteractedWithColorChange += OpenDoor;
    }
    public override void OpenDoor() {
        base.OpenDoor();
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        ActionNotifier.Instance.InteractedWithColorChange -= OpenDoor;
    }
}
