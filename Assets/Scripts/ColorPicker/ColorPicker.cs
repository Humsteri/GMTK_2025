using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [Header("Test")]
    public bool OpenBool = false;
    public bool CloseBool = false;

    [Header("Color Picker Object")]
    [SerializeField] GameObject _colorPicker;

    [Header("Selectable")]
    [SerializeField] List<ColorPickerColor> _colors;
    [SerializeField] ColorPickerColor _selectedColor;
    [SerializeField] int _colorIndex = 0;

    [Header("Audio")]
    [SerializeField] AudioClip _selectClip;
    [SerializeField] List<AudioClip> _browseClips;

    private void Update() {
        if (OpenBool) {
            OpenBool = false;
            OpenColorPicker();
        }
        else if (CloseBool) {
            CloseBool = false;
            CloseColorPicker();
        }
        if (InputManager.Instance.ColorNext) {
            NextColor();
        }
        if (InputManager.Instance.ColorPrevious) {
            PreviousColor();
        }
        if (InputManager.Instance.ColorConfirm) {

            AudioSource.PlayClipAtPoint(_selectClip, Camera.main.transform.position, 0.5f);

            if (_colorIndex == 0) {
                ActionNotifier.Instance.WorldColor?.Invoke(WorldColor.Red);
            }
            else if (_colorIndex == 1) {
                ActionNotifier.Instance.WorldColor?.Invoke(WorldColor.Green);
            }
            else if (_colorIndex == 2) {
                ActionNotifier.Instance.WorldColor?.Invoke(WorldColor.Blue);
            }
            CloseColorPicker();
        }

    }

    public void OpenColorPicker() {
        InputManager.Instance.EnableColorPicker();
        _colorPicker.SetActive(true);
        _colorIndex = 0;
        SelectColor(_colorIndex);
    }

    public void CloseColorPicker() {
        InputManager.Instance.DisableColorPicker();
        _colorPicker.SetActive(false);
    }

    void NextColor() {
        if (_colorIndex == _colors.Count - 1) {
            _colorIndex = 0;
        }
        else {
            _colorIndex++;
        }
        SelectColor(_colorIndex);
    }

    void PreviousColor() {
        if (_colorIndex == 0) {
            _colorIndex = _colors.Count - 1;
        }
        else {
            _colorIndex--;
        }
        SelectColor(_colorIndex);
    }

    void SelectColor(int index) {
        foreach (ColorPickerColor color in _colors) {
            color.Deselect();
        }
        _colors[index].Select();
        _selectedColor = _colors[index];
        AudioSource.PlayClipAtPoint(SelectRandomClip(), Camera.main.transform.position, 0.5f);
    }

    AudioClip SelectRandomClip() {
        return _browseClips[Random.Range(0, _browseClips.Count)];
    }
}
