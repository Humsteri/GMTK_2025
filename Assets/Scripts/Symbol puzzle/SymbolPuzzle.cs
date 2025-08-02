using System.Collections.Generic;
using UnityEngine;

public class SymbolPuzzle : MonoBehaviour
{
    [Header("Test")]
    public bool OpenPuzzleBool = false;
    public bool ClosePuzzleBool = false;

    [Header("Puzzle")]
    [SerializeField] GameObject _puzzleObject;
    public bool Completed = false;

    [Header("Apparatuses")]
    [SerializeField] List<SymbolPuzzleComponents> _components = new();
    [SerializeField] SymbolPuzzleComponents _selectedComponent;
    [SerializeField] int _apparatusIndex = 0;

    [Header("SelectedSymbols")]
    [SerializeField] int _firstApparatusSymbol;
    [SerializeField] int _secondApparatusSymbol;
    [SerializeField] int _thirdApparatusSymbol;
    [SerializeField] int _ButtonIndex;

    [Header("Answer")]
    [SerializeField] int _firstApparatusAnswer;
    [SerializeField] int _secondApparatusAsnwer;
    [SerializeField] int _thirdApparatusAnswer;

    [Header("Audio")]
    [SerializeField] AudioClip _audioClip;

    private void Update() {
        if (OpenPuzzleBool) {
            OpenPuzzleBool = false;
            OpenPuzzle();
        }
        else if (ClosePuzzleBool) {
            ClosePuzzleBool = false;
            ClosePuzzle();
        }

        if (InputManager.Instance.SymbolNext) {
            NextApparatus();
        }
        if (InputManager.Instance.SymbolPrevious) {
            PreviousApparatus();
        }
        if (InputManager.Instance.SymbolForward) {
            if (_apparatusIndex != 3) {
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);
            }
            _selectedComponent.Forward();
            GetApparatusSymbolIndex(_selectedComponent);
        }
        if (InputManager.Instance.SymbolBackwards) {
            if (_apparatusIndex != 3) {
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);
                
            }
            _selectedComponent.Backward();
            GetApparatusSymbolIndex(_selectedComponent);
        }
        if (InputManager.Instance.SymbolConfirm) {
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);
            if (_apparatusIndex == 3) { // 3 == Buttons
                if (_ButtonIndex == 0) { // 0 == Confirmation Button
                    SubmitAnswer();
                }
                else if (_ButtonIndex == 1) { // 1 == Cancel Button
                    ClosePuzzle();
                }
            }
        }
    }

    public void OpenPuzzle() {
        if (Completed) { return; }
        InputManager.Instance.EnableSymbolInputs();
        _puzzleObject.SetActive(true);
        _apparatusIndex = 0;
        SelectApparatus(_apparatusIndex);
        foreach (var component in _components) {
            GetApparatusSymbolIndex(component);
        }
    }

    public void ClosePuzzle() {
        InputManager.Instance.DisableSymbolInputs();
        _puzzleObject.SetActive(false);
    }

    void NextApparatus() {
        if (_apparatusIndex == _components.Count - 1) {
            _apparatusIndex = 0;
        }
        else {
            _apparatusIndex++;
        }
        SelectApparatus(_apparatusIndex);
    }

    void PreviousApparatus() {
        if (_apparatusIndex == 0) {
            _apparatusIndex = _components.Count - 1;
        }
        else {
            _apparatusIndex--;
        }
        SelectApparatus(_apparatusIndex);
    }

    void SelectApparatus(int index) {
        foreach (var apparatus in _components) {
            apparatus.Deselect();
        }
        _components[index].Select();
        _selectedComponent = _components[index];
    }

    void GetApparatusSymbolIndex(SymbolPuzzleComponents apparatus) {
        int index = _components.IndexOf(apparatus);
        switch (index) {
            case 0:
                _firstApparatusSymbol = _components[index].SymbolIndex;
            break;
            case 1:
                _secondApparatusSymbol = _components[index].SymbolIndex;
            break;
            case 2:
                _thirdApparatusSymbol = _components[index].SymbolIndex;
            break;
            case 3:
                _ButtonIndex = _components[index].SymbolIndex;
            break;
        }
    }

    void SubmitAnswer() {
        if (_firstApparatusSymbol == _firstApparatusAnswer && 
            _secondApparatusSymbol == _secondApparatusAsnwer && 
            _thirdApparatusSymbol == _thirdApparatusAnswer) 
        {
            ActionNotifier.Instance.SymbolPuzzleStatus?.Invoke(true);
            Completed = true;
            ClosePuzzle();
        }
        else { 
            print("Answer Was Incorrect");
        }

    }
}
