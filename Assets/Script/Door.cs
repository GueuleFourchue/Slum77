using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Door : MonoBehaviour {

	Animator animator; 
	Transform body;

	public float zMoveValue;
	float originZValue;

	public NavMeshSurface surface;

	bool canOpen;
	bool isClosed = true;

	[Header ("Sounds")]
	public AudioSource Open_Audio;
	public AudioSource Close_Audio;
    public AudioSource Button_Audio;

    void Start () 
	{
		animator = GetComponent<Animator> ();	
		body = transform.GetChild (0).transform;
		originZValue = body.transform.localPosition.z;
	}

	void Update () 
	{
		if (Input.GetMouseButtonDown (0) && canOpen) 
		{

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit) && hit.transform.name =="Switch") 
			{
				if (Vector3.Distance (hit.point, transform.position) < 7) 
				{
                    Button_Audio.Play();
                    StartCoroutine (DoorMove (zMoveValue, Open_Audio, false));
				}
			}
		}
			
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			canOpen = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			if (!isClosed) 
			{
				StartCoroutine (DoorMove (originZValue, Close_Audio, true));
			}

			canOpen = false;
		}
	}
		
	IEnumerator DoorMove(float value, AudioSource audio, bool closed)
	{
		yield return new WaitForSeconds (0.3f);

		body.transform.DOKill ();
		body.transform.DOLocalMoveZ (value, 1.2f).SetEase (Ease.OutBack).OnComplete(() =>
			{
				isClosed = closed;
			});
		
		audio.Play();
	}
		
}
