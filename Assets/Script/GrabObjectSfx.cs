using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectSfx : MonoBehaviour {

    public bool isGrabbed;

    [Header("Audio")]
    public AudioSource Grab_Audio;
    public AudioSource Hit_Audio;

    public void PlaySFX(AudioSource audio)
    {
        audio.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrabbed)
        {
            Hit_Audio.pitch = Random.Range(0.9f, 1.1f);
            PlaySFX(Hit_Audio);
            isGrabbed = false;
        }
            
    }
}
