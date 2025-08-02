using UnityEngine;

public class ColorPickerColor : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject _normal;
    [SerializeField] GameObject _highlight;

    [Header("Selected Status")]
    [SerializeField] bool _isSelected = false;

    public void Select() {
        _normal.SetActive(false);
        _highlight.SetActive(true);
        _isSelected = true;
    }

    public void Deselect() {
        _normal.SetActive(true);
        _highlight.SetActive(false);
        _isSelected = false;
    }
}
