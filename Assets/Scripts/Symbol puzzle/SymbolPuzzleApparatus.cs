using System.Collections.Generic;
using UnityEngine;

public class SymbolPuzzleApparatus : SymbolPuzzleComponents
{
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

    protected override void SelectSymbol(int index) {
        foreach (var symbol in Symbols) {
            symbol.SetActive(false);
        }
        Symbols[index].SetActive(true);
    }
}
