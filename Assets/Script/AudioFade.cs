using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class AudioFade : MonoBehaviour {

    public AudioMixer SfxAudioMixer;
    public AudioMixer ambientAudioMixer;

    bool volumeDown;

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.O))
        {
            if (volumeDown)
            {
                SfxAudioMixer.DOSetFloat("SfxVolume", -40f, 2f);
                ambientAudioMixer.DOSetFloat("AmbientVolume", -40f, 2f);

            }
            else
            {
                SfxAudioMixer.DOSetFloat("SfxVolume", 0f, 2f);
                ambientAudioMixer.DOSetFloat("AmbientVolume", 0f, 2f);
            }
            
            volumeDown = !volumeDown;
        }
	}
}
