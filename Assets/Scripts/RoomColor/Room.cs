using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> Lights = new();

    private void OnTriggerEnter(Collider other) {
        
    }

    public void EnableObjects() {
        foreach (GameObject obj in Lights) { 
            obj.SetActive(true);
        }
    }
    public void DisableObjects() {
        foreach (GameObject obj in Lights) {
            obj.SetActive(false);
        }
    }
}
