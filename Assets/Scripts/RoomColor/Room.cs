using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> _roomObjects = new();
    public void EnableObjects() {
        foreach (GameObject obj in _roomObjects) { 
            obj.SetActive(true);
        }
    }
    public void DisableObjects() {
        foreach (GameObject obj in _roomObjects) {
            obj.SetActive(false);
        }
    }
}
