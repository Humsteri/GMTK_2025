using UnityEngine;

public class SymbolPuzzleDoor : OpenableDoor
{
    protected override void Start() {
        base.Start();
        ActionNotifier.Instance.SymbolPuzzleCompleted += OpenDoor;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        ActionNotifier.Instance.SymbolPuzzleCompleted -= OpenDoor;
    }
}
