using System.Collections.Generic;
using UnityEngine;

public class SelectAnimationForLight : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animation _animator;

    [Header("Animations")]
    [SerializeField] List<AnimationClip> _animations = new();
    void Start() {
        _animator.clip = _animations[Random.Range(0, _animations.Count)];
        _animator.Play();
    }
}
