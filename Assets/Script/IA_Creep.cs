using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using DG.Tweening;
using Colorful;

public class IA_Creep : MonoBehaviour {

	NavMeshAgent agent;

    [Header("Audio")]
    public AudioSource Walk_Audio;
    public AudioSource Growl_Audio;

    [Header("Waypoints_Tease")]
    public Transform[] waypointsTease;

    public Transform player;
    public Animator animator;
    public GameObject light;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
	}

    public void Tease()
    {
        StartCoroutine(TeaseCorou());
        light.SetActive(true);
    }

    public IEnumerator TeaseCorou()
    {
        //
        Camera.main.GetComponent<Colorful.Glitch>().enabled = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>().enabled = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().enabled = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = true;
        Camera.main.DOFieldOfView(90, 3f);

        yield return new WaitForSeconds(3f);
        Walk_Audio.Play();
        agent.destination = waypointsTease[0].position;
        animator.SetTrigger("Walk");

     

        yield return new WaitForSeconds(3f);
        agent.destination = waypointsTease[1].position;

        yield return new WaitForSeconds(6f);
        Growl_Audio.Play();

        yield return new WaitForSeconds(4.5f);
        Camera.main.GetComponent<Colorful.Glitch>().enabled = false;
        yield return new WaitForSeconds(1);
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>().enabled = false;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = false;
        yield return new WaitForSeconds(1);
        Camera.main.DOFieldOfView(70, 2f);
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().enabled = false;

        yield return new WaitForSeconds(8f);
        Walk_Audio.Stop();
    }
}
