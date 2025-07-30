using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] WorldColor _worldColor;
    public WorldColor WorldColor { 
        get => _worldColor;
        set {
            _worldColor = value;
            UpdateRooms();
        } 
    }

    [Header("Rooms")]
    [SerializeField] List<Room> _redRooms = new();
    [SerializeField] List<Room> _greenRooms = new();
    [SerializeField] List<Room> _blueRooms = new();

    [Header("Materials")]
    [SerializeField] Material _redMaterial;
    [SerializeField] Material _greenMaterial;
    [SerializeField] Material _blueMaterial;

    [Header("Colors")]
    [SerializeField]Color _redColor;
    [SerializeField]Color _greenColor;
    [SerializeField]Color _blueColor;
    Color _disabledColor;

    private void OnValidate() {
        WorldColor = _worldColor;
    }

    void Awake() {
        GetColors();
        UpdateRooms();
    }

    private void OnDestroy() {
        _redMaterial.color = _redColor;
        _greenMaterial.color = _greenColor;
        _blueMaterial.color = _blueColor;
    }

    public void UpdateRooms() {
        DisableAllRooms();

        switch (WorldColor) {
            case WorldColor.Red:
                EnableRedRooms();
                break;
            case WorldColor.Green:
                EnableGreenRooms();
                break;
            case WorldColor.Blue:
                EnableBlueRooms();
                break;
            case WorldColor.All:
                EnableRedRooms();
                EnableGreenRooms();
                EnableBlueRooms();
                break;
        }
    }

    void EnableRedRooms() {
        _redMaterial.color = _redColor;
        foreach (var room in _redRooms) {
            room.EnableObjects();
        }
    }
    void EnableGreenRooms() {
        _greenMaterial.color = _greenColor;
        foreach (var room in _greenRooms) {
            room.EnableObjects();
        }
    }
    void EnableBlueRooms() {
        _blueMaterial.color = _blueColor;
        foreach (var room in _blueRooms) {
            room.EnableObjects();
        }
    }
    void DisableAllRooms() {
        _redMaterial.color = _disabledColor;
        _greenMaterial.color = _disabledColor;
        _blueMaterial.color = _disabledColor;
        foreach (var room in _redRooms) {
            room.DisableObjects();
        }
        foreach (var room in _greenRooms) {
            room.DisableObjects();
        }
        foreach (var room in _blueRooms) {
            room.DisableObjects();
        }
    }

    void GetColors() {
        _redColor = _redMaterial.color;
        _greenColor = _greenMaterial.color;
        _blueColor = _blueMaterial.color;
    }
}
