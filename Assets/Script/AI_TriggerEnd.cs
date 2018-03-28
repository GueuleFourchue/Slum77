using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TriggerEnd : MonoBehaviour {

    public AI_End AI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AI.EndBehaviour();
        }
    }
}
