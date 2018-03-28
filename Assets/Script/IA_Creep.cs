using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

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
        yield return new WaitForSeconds(2f);
        Walk_Audio.Play();
        agent.destination = waypointsTease[0].position;
        animator.SetTrigger("Walk");

        yield return new WaitForSeconds(3f);
        agent.destination = waypointsTease[1].position;

        yield return new WaitForSeconds(6f);
        Growl_Audio.Play();
        yield return new WaitForSeconds(20f);
        Walk_Audio.Stop();
    }
}
