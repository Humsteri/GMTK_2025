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
    [SerializeField] VolumeParameter<Color> _colorFilter = new();
    ColorAdjustments _colorAdjust;

    [Header("Color Filter Values")]
    [ColorUsage(true, true)]
    [SerializeField] Color _white;
    [ColorUsage(true, true)]
    [SerializeField] Color _red;
    [ColorUsage(true, true)]
    [SerializeField] Color _green;
    [ColorUsage(true, true)]
    [SerializeField] Color _blue;
    [ColorUsage(true, true)]
    [SerializeField] Color _black;


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
                _colorFilter.value = _red;
                break;
            case WorldColor.Green:
                _colorFilter.value = _green;
                break;
            case WorldColor.Blue:
                _colorFilter.value = _blue;
                break;
            case WorldColor.All:
                _colorFilter.value = _white;
                break;
            case WorldColor.None:
                _colorFilter.value = _black;
                break;
            default:
                Debug.LogError("Something fuked");
                break;
        }
        _colorAdjust.colorFilter.SetValue(_colorFilter);
    }
}
