using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Creep : MonoBehaviour {

	NavMeshAgent agent;
	public Transform player;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
	}
	

	void LateUpdate () 
	{
		agent.destination = player.position;
	}
}
