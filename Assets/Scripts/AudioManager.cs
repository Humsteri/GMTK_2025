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
    [SerializeField] float maxPitchChange;
    [SerializeField] float minPitchChange;
    public void PlaySound()
    {
        /* if (audioSource.isPlaying)
        {
            audioSource.Stop();
        } */
        audioSource.clip = bassSound;
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
}
