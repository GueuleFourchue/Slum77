using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Door : MonoBehaviour {

	Animator animator; 
	public Transform doorLeft;
    public Transform doorRight;

    public float xRightMoveValue;
    public float xLeftMoveValue;
    float leftOriginXValue;
    float rightOriginXValue;

    //public NavMeshSurface surface;

	bool canOpen;
	bool isClosed = true;

	[Header ("Sounds")]
	public AudioSource Open_Audio;
	public AudioSource Close_Audio;
    public AudioSource Button_Audio;

    void Start () 
	{
		animator = GetComponent<Animator> ();
        leftOriginXValue = doorLeft.localPosition.x;
        rightOriginXValue = doorRight.localPosition.x;
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
                    StartCoroutine (DoorMove (Open_Audio));
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
				StartCoroutine (DoorMoveBack (Close_Audio));
			}

			canOpen = false;
		}
	}
		
	IEnumerator DoorMove(AudioSource audio)
	{
		yield return new WaitForSeconds (0.3f);

		doorRight.transform.DOKill ();
        doorLeft.transform.DOKill();

        doorRight.transform.DOLocalMoveX (xRightMoveValue, 1.2f).SetEase (Ease.OutCubic).OnComplete(() =>
			{
				isClosed = false;
			});
        doorLeft.transform.DOLocalMoveX(xLeftMoveValue, 1.2f).SetEase(Ease.OutCubic );

        audio.Play();
	}

    IEnumerator DoorMoveBack(AudioSource audio)
    {
        yield return new WaitForSeconds(0.3f);

        doorRight.transform.DOKill();
        doorLeft.transform.DOKill();

        doorRight.transform.DOLocalMoveX(rightOriginXValue, 1.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            isClosed = true;
        });
        doorLeft.transform.DOLocalMoveX(leftOriginXValue, 1.2f).SetEase(Ease.OutCubic);

        audio.Play();
    }
}
