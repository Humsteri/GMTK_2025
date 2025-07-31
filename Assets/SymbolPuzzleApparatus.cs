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
        print("Going Forward In Apparatus");
    }

    public override void Backward() {
        base.Backward();
        print("Going Backwards In Apparatus");
    }

    protected override void SelectSymbol(int index) {
        foreach (var symbol in Symbols) {
            symbol.SetActive(false);
        }
        Symbols[index].SetActive(true);
    }
}
