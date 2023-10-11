﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip swordSlashSound;
    private PlayerCharacter playerCharacter;
    private bool isRunSoundPlaying = false; // Biến để kiểm tra trạng thái phát của runSound
    public AudioSource audioSource;

    private void Start()
    {
        // Code khởi động
        audioSource = GetComponent<AudioSource>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (playerCharacter.isMove && playerCharacter.isGrounded)
        {
            if (!isRunSoundPlaying)
            {
                // Phát runSound nếu nó chưa đang phát
                audioSource.clip = runSound;
                audioSource.Play();
                isRunSoundPlaying = true;
            }
        }
        else
        {
            // Dừng runSound nếu nó đang phát
            if (isRunSoundPlaying)
            {
                audioSource.Stop();
                isRunSoundPlaying = false;
            }
        }
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayClipOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
