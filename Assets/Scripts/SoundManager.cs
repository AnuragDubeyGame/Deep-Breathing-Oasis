using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    public AudioClip playSound;
    private AudioSource UIAudioSource;
    private void Start()
    {
        UIAudioSource = GetComponent<AudioSource>();
    }
    public void TestingSound( )
    {
        UIAudioSource.clip = playSound;
        UIAudioSource.Play();
    }
}
