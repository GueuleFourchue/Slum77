using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Creep : MonoBehaviour {

	NavMeshAgent agent;

    [Header("Waypoints_Tease")]
    public Transform[] waypointsTease;

    public Transform player;
    public Animator animator;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
	}

    public void Tease()
    {
        StartCoroutine(TeaseCorou());
    }

    public IEnumerator TeaseCorou()
    {
        yield return new WaitForSeconds(2f);
        agent.destination = waypointsTease[0].position;
        animator.SetTrigger("Walk");
        yield return new WaitForSeconds(3f);
        agent.destination = waypointsTease[1].position;
    }
}
