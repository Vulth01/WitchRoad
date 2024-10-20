using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        TrainManager trainManager = FindObjectOfType<TrainManager>();
        if (trainManager != null) trainManager.OnDoorOpen += DoDoorOpen;
    }

    private void DoDoorOpen()
    {
        audioSource.clip = clips[6];
    }
}
