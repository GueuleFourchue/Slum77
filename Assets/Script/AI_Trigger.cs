using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Trigger : MonoBehaviour {

    public IA_Creep AI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AI.Tease();
        }
    }
}
