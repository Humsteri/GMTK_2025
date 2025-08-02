using UnityEngine;

public class CheckForCorrectLight : MonoBehaviour
{
    [SerializeField] WorldColor _colorOfRoom;

    private void Start() {
        ActionNotifier.Instance.WorldColorChange += CheckForLight;
    }

    public void CheckForLight(WorldColor color) {
        if (color == _colorOfRoom) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
        }
    }
}
