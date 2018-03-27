using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class FpsController : MonoBehaviour {

    public HeadBob headbob;
    public Light torchLight;

    [Header("Values")]
	public float mouseSensitivity;
	public float walkSpeed;
	public float runSpeed;
	public float slowWalkSpeed;
    public float crawlSpeed;

    [Header("Sounds")]
	public AudioSource Walk_Audio;
	public AudioSource Run_Audio;
	public AudioSource WalkSlow_Audio;
	public AudioSource Crouch_Audio;
    public AudioSource Torch_Audio;

    [Header("Anim")]
    public Animator animator;

    private AudioSource actual_Audio;

	float horizontal;
	float vertical;

	float moveX;
	float moveZ;

	Camera camera;
	bool isRunning;
	bool isSlowWalking;
	bool isCrouching;
    bool isCrawling;

	float cameraOriginYPosition;

	VignetteAndChromaticAberration effect;

	bool audioIsStopping;
    bool torchOn = true;

	void Start ()
	{
		camera = Camera.main;

		effect = camera.GetComponent <VignetteAndChromaticAberration>();
		cameraOriginYPosition = camera.transform.localPosition.y;
	}

	void Update () 
	{
		MouseRotation ();
		IsRunning ();
		IsSlowWalking ();

		if (Input.GetKeyDown (KeyCode.C)) 
		{
			Crouch ();
		}
        if (Input.GetKeyDown(KeyCode.E))
        {
            Torch();
        }

		Movement ();

        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(0);
	}



	void MouseRotation ()
	{
		horizontal = Input.GetAxis ("Mouse X") * mouseSensitivity;
		vertical -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		vertical = Mathf.Clamp (vertical, -80, 80);

		this.transform.Rotate (0, horizontal, 0);
		camera.transform.localRotation = Quaternion.Euler(vertical, 0, 0);
	}

	void Movement()
	{
		if (isRunning) 
		{
			moveX = Input.GetAxisRaw ("Horizontal") * Time.deltaTime * runSpeed;
			moveZ = Input.GetAxisRaw ("Vertical") * Time.deltaTime * runSpeed;
		} 
		else if (isSlowWalking || isCrouching) 
		{
			moveX = Input.GetAxis ("Horizontal") * Time.deltaTime * slowWalkSpeed;
			moveZ = Input.GetAxis ("Vertical") * Time.deltaTime * slowWalkSpeed;
		}
        else if (isCrawling)
        {
            moveX = Input.GetAxis("Horizontal") * Time.deltaTime * crawlSpeed;
            moveZ = Input.GetAxis("Vertical") * Time.deltaTime * crawlSpeed;
        }
        else 
		{
			moveX = Input.GetAxis ("Horizontal") * Time.deltaTime * walkSpeed;
			moveZ = Input.GetAxis ("Vertical") * Time.deltaTime * walkSpeed;
		}

		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) 
		{
			if (isSlowWalking || isCrouching) 
			{
				PlaySFX (WalkSlow_Audio, 0.3f);
				Walk_Audio.DOFade (0, 0.2f);	
				Run_Audio.DOFade (0, 0.2f);

				headbob.Idle ();

                if (isSlowWalking)
                    animator.SetTrigger("Idle");
                if (isCrouching)
                    animator.SetTrigger("Crouch");
            }
			else if (isRunning)
			{
				PlaySFX (Run_Audio, 0.55f);
				Walk_Audio.DOFade (0, 0.2f);	
				WalkSlow_Audio.DOFade (0, 0.2f);

				headbob.Run ();
                animator.SetTrigger("Walk");
            }
            else if (isCrawling)
            {
                animator.SetTrigger("Crawl");
            }
            else 
			{
				PlaySFX (Walk_Audio, 0.4f);
				Run_Audio.DOFade (0, 0.2f);	
				WalkSlow_Audio.DOFade (0, 0.2f);

				headbob.Walk ();
                animator.SetTrigger("Walk");
            }
				
		}

		if (moveX == 0 && moveZ == 0 && actual_Audio != null && !audioIsStopping) 
		{
			StopSFX (Walk_Audio);
            StopSFX(WalkSlow_Audio);
            headbob.Idle ();
            animator.SetTrigger("Idle");
        }
			
		transform.Translate(moveX, 0, moveZ);


	}

	void IsRunning ()
	{
		if (Input.GetKeyDown (KeyCode.LeftShift)) 
		{
			if (moveX != 0 || moveZ != 0) 
			{
				isRunning = true;
				camera.DOKill ();
				camera.DOFieldOfView (85, 0.8f);

				effect.enabled = true;
            }


		} 
		else if (Input.GetKeyUp (KeyCode.LeftShift)) 
		{
			isRunning = false;
			camera.DOKill ();
			camera.DOFieldOfView (70, 0.5f);
		
			effect.enabled = false;

			if (!audioIsStopping)
				StopSFX (Run_Audio);
        }
			
	}

	void IsSlowWalking()
	{
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			isSlowWalking = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftControl)) 
		{
			isSlowWalking = false;

            if (!audioIsStopping)
                StopSFX(WalkSlow_Audio);
        }
	}

	void Crouch()
	{
		if (isCrouching) 
		{
            Crouch_Audio.Play ();

            GetComponent<CapsuleCollider>().height = 2.5f;
            GetComponent<CapsuleCollider>().center = Vector3.zero;

			camera.DOKill ();
			camera.transform.DOLocalMoveY (cameraOriginYPosition, 1f);
        }
        if (!isCrouching || isCrawling)
        {
            if (GetComponent<CapsuleCollider>().enabled == false)
            {
                GetComponent<CapsuleCollider>().enabled = true;
                GetComponent<BoxCollider>().enabled = false;
            }

            Crouch_Audio.Play();

            GetComponent<CapsuleCollider>().height = 1.25f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, -0.6f, 0);

            camera.DOKill();
            camera.transform.DOLocalMoveY(cameraOriginYPosition - 0.7f, 1f).SetEase(Ease.OutBack);

            isCrawling = false;
        }

        isCrouching = !isCrouching;

        StartCoroutine(Crawl());
	}

    IEnumerator Crawl()
    {
        yield return new WaitForSeconds(0.4f);
        if (Input.GetKey(KeyCode.C))
        {
            Crouch_Audio.Play();

            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = true;

            camera.DOKill();
            camera.transform.DOLocalMoveY(cameraOriginYPosition - 1.2f, 0.8f).SetEase(Ease.OutBack);

            isCrouching = false;
            isCrawling = true;
        }
    }

    void PlaySFX(AudioSource audio, float volume)
	{
		if (audio.volume == 0) 
		{
			audio.DOKill ();
			audio.DOFade (volume, 0.2f);
			actual_Audio = audio;
		}
	}

	public void StopSFX(AudioSource audio)
	{
		if (audio.volume != 0) 
		{
			audio.DOKill ();
			audio.DOFade (0, 0.2f).OnComplete(() =>
				{
					audioIsStopping = false;
				});
			audioIsStopping = true;
		}
	}

    void Torch()
    {
        if (torchOn)
        {
            torchLight.enabled = false;
            torchOn = false;
        }
           
        else
        {
            torchLight.enabled = true;
            torchOn = true;
        }

        Torch_Audio.pitch = Random.Range(0.9f, 1.1f);
        Torch_Audio.Play();
    }
}
