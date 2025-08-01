using System.Collections.Generic;
using UnityEngine;

public class SymbolPuzzleComponents : MonoBehaviour
{
    public List<GameObject> Symbols = new();
    public int SymbolIndex = 0;

    [SerializeField] protected List<Behaviour> _highlights = new();
    [SerializeField] protected bool _isSelected = false;

    protected virtual void Awake() {

    }

    protected virtual void OnEnable() {
        SymbolIndex = 0;
    }

    public virtual void Select() {
        foreach (Behaviour highlight in _highlights) {
            highlight.enabled = true;
        }
        _isSelected = true;
    }

    public virtual void Deselect() {
        foreach (Behaviour highlight in _highlights) {
            highlight.enabled = false;
        }
        _isSelected = false;
    }

    public virtual void Forward() {
        if (SymbolIndex == Symbols.Count - 1) {
            SymbolIndex = 0;
        }
        else {
            SymbolIndex++;
        }
        SelectSymbol(SymbolIndex);
    }

    public virtual void Backward() {
        if (SymbolIndex == 0) {
            SymbolIndex = Symbols.Count - 1;
        }
        else {
            SymbolIndex--;
        }
        SelectSymbol(SymbolIndex);
    }

    protected virtual void SelectSymbol(int index) {

    }
}
