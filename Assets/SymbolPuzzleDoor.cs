using UnityEngine;

public class SymbolPuzzleDoor : MonoBehaviour
{
    [Header("Pieces")]
    [SerializeField] GameObject _doorPiece;
    [SerializeField] GameObject _wallPiece;

    [Header("Audio")]
    [SerializeField] AudioClip _clip;
    private void Start() {
        _doorPiece.SetActive(false);
        ActionNotifier.Instance.SymbolPuzzleStatus += ReactToPuzzle;
    }

    public void ReactToPuzzle(bool status) {
        if (status) {
            _doorPiece.SetActive(true);
            _wallPiece.SetActive(false);
            AudioSource.PlayClipAtPoint(_clip, transform.position);
        }
    }
}
