using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabObjects : MonoBehaviour {

	public Transform hand;
	public float throwSpeed;

	Transform grabbedObj;
	bool isGrabbing;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		if (Input.GetMouseButtonDown (1) && isGrabbing) 
		{
			DropObject (grabbedObj);
		}

		if (Input.GetMouseButtonDown (1) && !isGrabbing) 
		{
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit) && hit.transform.CompareTag ("Grabable")) 
			{
				if (Vector3.Distance (hit.point, transform.position) < 3) 
				{
					GrabObject (hit.transform);
				}
			}
		}

		if (Input.GetMouseButtonDown (0) && isGrabbing) 
		{
			ThrowObject (grabbedObj);
		}
	}

	void GrabObject(Transform obj)
	{
		isGrabbing = true;
		grabbedObj = obj;

		obj.GetComponent<Rigidbody> ().useGravity = false;
		obj.GetComponent<BoxCollider> ().enabled = false;

		obj.parent = hand;
		obj.DOLocalMove (Vector3.zero, 0.3f);
	}

	void ThrowObject(Transform obj)
	{
		isGrabbing = false;

		obj.parent = null;

		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.useGravity = true;
		obj.GetComponent<BoxCollider> ().enabled = true;
		rb.velocity = Camera.main.transform.forward * throwSpeed / rb.mass;
	}

	void DropObject(Transform obj)
	{
		isGrabbing = false;

		obj.parent = null;

		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.useGravity = true;
		obj.GetComponent<BoxCollider> ().enabled = true;

		Debug.Log ("drop");
	}
}
