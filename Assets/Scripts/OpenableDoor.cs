using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] bool _isOpen;
    [Header("Pieces")]
    [SerializeField] GameObject _doorPiece;
    [SerializeField] GameObject _wallPiece;

    [Header("Audio")]
    [SerializeField] AudioClip _clip;
    protected virtual void Start() {
        _doorPiece.SetActive(false);
    }

    public virtual void OpenDoor() {
        if (_isOpen) { return; }
        _doorPiece.SetActive(true);
        _wallPiece.SetActive(false);
        AudioSource.PlayClipAtPoint(_clip, transform.position);
        _isOpen = true;
    }
    public virtual void CloseDoor() {
        if (!_isOpen) { return; }
        _doorPiece.SetActive(false);
        _wallPiece.SetActive(true);
        AudioSource.PlayClipAtPoint(_clip, transform.position);
        _isOpen = false;
    }

    protected virtual void OnDestroy() {

    }
}
