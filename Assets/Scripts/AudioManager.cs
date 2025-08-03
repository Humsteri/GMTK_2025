using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Instance
    public static AudioManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion

    AudioSource audioSource => GetComponent<AudioSource>();
    [SerializeField] AudioClip bassSound;
    [SerializeField] AudioClip footStep;
    [SerializeField] AudioClip turn;
    [SerializeField] AudioClip moveInteract;
    [SerializeField] AudioClip cat;
    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip selectedInteraction;
    [SerializeField] float maxPitchChange;
    [SerializeField] float minPitchChange;
    float volumeNormal = 0.146f;
    public void PlaySound()
    {
        if (!DialogueManager.Instance.DialogueGoing)
        {
            audioSource.Stop();
            return;
        }
        audioSource.clip = bassSound;
        audioSource.volume = volumeNormal;
        audioSource.pitch = UnityEngine.Random.Range(minPitchChange, maxPitchChange);
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PlayInteraction()
    {
        audioSource.volume = volumeNormal;
        audioSource.pitch = 0.07f;
        audioSource.clip = moveInteract;
        audioSource.Play();
    }
    public void PlaySelectedInteraction()
    {
        audioSource.volume = volumeNormal;
        audioSource.pitch = 0.5f;
        audioSource.clip = selectedInteraction;
        audioSource.Play();
    }
    public void PlayFootStep(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(footStep, pos, 0.7f);
    }
    public void PlayPickup()
    {
        audioSource.pitch = 1f;
        audioSource.volume = 0.6f;
        audioSource.clip = pickup;
        audioSource.Play();
       // AudioSource.PlayClipAtPoint(pickup, pos, 0.7f);
    }
    public void PlayCatSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(cat, pos);
    }
    public void PlayTurn(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(turn, pos, 0.3f);
    }
}
