using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Color")]
    [SerializeField] WorldColor _roomColor;

    [Header("Is Player In Room")]
    [SerializeField] bool _playerInRoom = false;
    [SerializeField] bool _lightsOnInRoom = false;

    [Header("Togleable Lights")]
    [SerializeField] List<GameObject> _lights = new();

    [Header("Light Delay")]
    [SerializeField] bool _useDelay = true;
    [SerializeField] float _lightDelayTime = 1f;

    private void Awake() {
        DisableLights();
    }

    private void OnTriggerEnter(Collider other) {

        if (WorldColorManager.Instance.WorldColor == _roomColor || WorldColorManager.Instance.WorldColor == WorldColor.All) {

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player == null) { return; }

            _playerInRoom = true;

            if (_useDelay) {
                StartCoroutine(EnableLightsWithDelay());
            }
            else {
                EnableLights();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!_lightsOnInRoom) { return; }
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) { return; }

        _playerInRoom = false;
        DisableLights();
    }

    IEnumerator EnableLightsWithDelay() {
        yield return new WaitForSeconds(_lightDelayTime);
        if (_playerInRoom) { 
            EnableLights();
        }
    }

    public void EnableLights() {
        foreach (GameObject obj in _lights) { 
            obj.SetActive(true);
        }
        _lightsOnInRoom = true;
    }
    public void DisableLights() {
        foreach (GameObject obj in _lights) {
            obj.SetActive(false);
        }
        _lightsOnInRoom = false;
    }
}
