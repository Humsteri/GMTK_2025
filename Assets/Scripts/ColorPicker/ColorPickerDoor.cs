using UnityEngine;

public class ColorPickerDoor : OpenableDoor
{
    protected override void Start() {
        base.Start();
        ActionNotifier.Instance.InteractedWithColorChange += OpenDoor;
        print("Subiscribed");
    }
    public override void OpenDoor() {
        base.OpenDoor();
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        print("UniSubiscribed");
        ActionNotifier.Instance.InteractedWithColorChange -= OpenDoor;
    }
}
