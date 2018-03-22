using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCanvas : MonoBehaviour {

    public PrezManager prez;
    public GameObject canvasCursor;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.name == "Screen")
            {
                if (Vector3.Distance(hit.point, transform.position) < 6)
                {
					Activation ();
                }
            }
        }
    }

	void Activation()
	{
		GetComponent<FpsController>().StopSFX (GetComponent<FpsController>().Walk_Audio);

		GetComponent<FpsController>().enabled = false;
		GetComponent<ActivateCanvas>().enabled = false;
		canvasCursor.SetActive(false);

		prez.enabled = true;
		prez.ActivateCanvas();
	}
}
