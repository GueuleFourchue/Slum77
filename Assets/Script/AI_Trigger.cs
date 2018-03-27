using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Trigger : MonoBehaviour {

    public IA_Creep AI;
    public AudioSource audio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AI.Tease();
            audio.Play();
        }
    }
}
