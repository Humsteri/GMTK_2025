using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldColorManager : MonoBehaviour
{
    public static WorldColorManager Instance;

    [Header("World Color")]
    [SerializeField] WorldColor _worldColor;

    public WorldColor WorldColor {
        get => _worldColor;
        set {
            _worldColor = value;
            ChangeWorldColor(value);
        }
    }

    [Header("Post Processing")]
    [SerializeField] VolumeProfile _volume;
    [SerializeField] VolumeParameter<float> _hueShift = new();
    [SerializeField] VolumeParameter<Color> _colorFilter = new();
    ColorAdjustments _colorAdjust;

    [Header("Color Filter Values")]
    [ColorUsage(true, true)]
    [SerializeField] Color _white;
    [ColorUsage(true, true)]
    [SerializeField] Color _rgbbq;

    [Header("Hue Shift Values")]
    [SerializeField] float _redColor;
    [SerializeField] float _blueColor;
    [SerializeField] float _greenColor;
    [SerializeField] float _whiteColor;

    private void OnValidate() {
        if (_colorAdjust == null) { return; }
        WorldColor = _worldColor;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _volume.TryGet<ColorAdjustments>(out _colorAdjust);
        if (_colorAdjust == null) {
            Debug.LogError("No ColorAdjustments found on profile");
        }

        ChangeWorldColor(WorldColor);
    }

    void ChangeWorldColor(WorldColor color) {
        switch (color) {
            case WorldColor.Red:
                _hueShift.value = _redColor;
                _colorFilter.value = _rgbbq;
                break;
            case WorldColor.Green:
                _hueShift.value = _greenColor;
                _colorFilter.value = _rgbbq;
                break;
            case WorldColor.Blue:
                _hueShift.value = _blueColor;
                _colorFilter.value = _rgbbq;
                break;
            case WorldColor.All:
                _hueShift.value = _whiteColor;
                _colorFilter.value = _white;
                break;
            case WorldColor.None:
                _hueShift.value = _whiteColor;
                _colorFilter.value = _white;
                break;
            default:
                Debug.LogError("Something fuked");
                break;
        }
        _colorAdjust.colorFilter.SetValue(_colorFilter);
        _colorAdjust.hueShift.SetValue(_hueShift);
    }
}
