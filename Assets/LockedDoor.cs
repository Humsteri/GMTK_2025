using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool IsOpen;
    [Header("Audio")]
    [SerializeField] AudioClip _clip;
    Animator animator => GetComponent<Animator>();
    void Start()
    {
        
    }
    public void OpenLockedDoor()
    {
        if (IsOpen) { return; }
        print("Door opens");
        AudioSource.PlayClipAtPoint(_clip, transform.position);
        animator.Play("DoorOpening", -1, 0);
        gameObject.tag = "Untagged";
    }
}
