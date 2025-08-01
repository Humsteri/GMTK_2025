using System.Collections.Generic;
using UnityEngine;

public class SymbolPuzzleButtons : SymbolPuzzleComponents
{
    [SerializeField] List<Behaviour> ButtonHighLights;

    protected override void Awake() {
        base.Awake();

    }

    protected override void OnEnable() {
        base.OnEnable();
        SelectSymbol(SymbolIndex);
    }

    public override void Forward() {
        base.Forward();
    }

    public override void Backward() {
        base.Backward();
    }

    public override void Select() {
        base.Select();
        SelectSymbol(SymbolIndex);
    }

    public override void Deselect() {
        base.Deselect();
        SelectSymbol(SymbolIndex);
    }

    protected override void SelectSymbol(int index) {
        foreach (var button in ButtonHighLights) {
            button.enabled = false;
        }
        if (_isSelected) {
            ButtonHighLights[SymbolIndex].enabled = true;
        }
    }
}
