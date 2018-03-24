using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabObjects : MonoBehaviour {

	public Material[] alphaMaterials;

	public Transform hand;
	public float throwSpeed;

    public Material alphaMat;
    Material grabbedMat;

	UnityStandardAssets.ImageEffects.DepthOfField depthOfField;

	Transform grabbedObj;
	bool isGrabbing;

	void Start () 
	{
		depthOfField = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>();
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

		/*
        //Material
        Renderer rend = obj.GetChild(0).GetComponent<Renderer>();
        grabbedMat = rend.material;
        rend.material = alphaMat;
		*/

		//Material
		Renderer rend = obj.GetChild(0).GetComponent<Renderer>();
		grabbedMat = rend.material;
		if (obj.GetComponentInChildren<GrabableObject_MatIndex> ()) 
		{
			int index = obj.GetComponentInChildren<GrabableObject_MatIndex> ().index;
			rend.material = alphaMaterials [index];
		}


        //Audio
        GrabObjectSfx script = grabbedObj.GetComponent<GrabObjectSfx>();
        script.isGrabbed = true;
        script.Grab_Audio.pitch = Random.Range(0.9f, 1.1f);
        script.PlaySFX(script.Grab_Audio);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

		obj.GetComponentInChildren<MeshCollider> ().enabled = false;

        obj.parent = hand;
		obj.DOLocalMove (Vector3.zero, 0.3f);

		depthOfField.enabled = true;
	}

	void ThrowObject(Transform obj)
	{
		isGrabbing = false;
		obj.parent = null;

        //Material
        Renderer rend = obj.GetChild(0).GetComponent<Renderer>();
        rend.material = grabbedMat;


        Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.useGravity = true;
        rb.isKinematic = false;
		obj.GetComponentInChildren<MeshCollider> ().enabled = true;
		rb.velocity = Camera.main.transform.forward * throwSpeed / rb.mass;

		depthOfField.enabled = false;
	}

	void DropObject(Transform obj)
	{
		isGrabbing = false;
		obj.parent = null;

        //Material
        Renderer rend = obj.GetChild(0).GetComponent<Renderer>();
        rend.material = grabbedMat;

        Rigidbody rb = obj.GetComponent<Rigidbody> ();
        rb.isKinematic = false;
        rb.useGravity = true;
		obj.GetComponentInChildren<MeshCollider> ().enabled = true;

		depthOfField.enabled = false;
	}
}
