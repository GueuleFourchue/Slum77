using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Audio;

public class AI_End : MonoBehaviour {

    NavMeshAgent agent;
    Transform player;

    public Canvas_End canvasEnd;
    public GameObject mesh;
    public Animator anim;
    public AudioMixer ambientAudioMixer;
    public AudioMixer SfxAudioMixer;

    [Header("Lights")]
    public GameObject light1;
    public GameObject light2;
    public GameObject light3;

    [Header("Audio")]
    public AudioSource Hum_Audio_01;
    public AudioSource Hum_Audio_02;
    public AudioSource Hum_Audio_03;
    public AudioSource Flicker_Audio;
    public AudioSource End_Audio;

    bool hasDoneAnim;
    bool rush;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

    public void EndBehaviour()
    {
        if (!hasDoneAnim)
            StartCoroutine(EndRush());
    }

    private void Update()
    {
        if (rush)
        agent.destination = player.transform.position;
    }
    IEnumerator EndRush()
    {
        light1.SetActive(false);
        Hum_Audio_01.Stop();
        Flicker_Audio.pitch = Random.Range(0.9f, 1.1f);
        Flicker_Audio.volume = 0.1f;
        Flicker_Audio.Play();

        yield return new WaitForSeconds(0.7f);
        light2.SetActive(false);
        Hum_Audio_02.Stop();
        Flicker_Audio.pitch = Random.Range(0.9f, 1.1f);
        Flicker_Audio.volume = 0.2f;
        Flicker_Audio.Play();

        yield return new WaitForSeconds(0.4f);
        light3.SetActive(false);
        Hum_Audio_03.Stop();
        Flicker_Audio.pitch = Random.Range(0.9f, 1.1f);
        Flicker_Audio.volume = 0.3f;
        Flicker_Audio.Play();

        hasDoneAnim = true;

        //EndRush
        mesh.SetActive(true);
        anim.SetTrigger("Run");
        rush = true;
        yield return new WaitForSeconds(0.7f);

        ambientAudioMixer.DOSetFloat("AmbientVolume", -80f, 8f);
        SfxAudioMixer.DOSetFloat("SfxVolume", -80f, 8f);

        End_Audio.DOFade(1f, 0.5f);
        End_Audio.Play();

        yield return new WaitForSeconds(0.8f);
        canvasEnd.EndAnim();
    }
}
