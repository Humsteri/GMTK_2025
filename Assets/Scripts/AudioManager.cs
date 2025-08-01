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
    public void PlayFootStep(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(footStep, pos);
    }
    public void PlayTurn(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(turn, pos, 0.3f);
    }
}
