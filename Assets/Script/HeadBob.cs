using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class HeadBob : MonoBehaviour {

	private float timer = 0.0f;
	public float bobbingSpeed = 0.18f;
	public float bobbingAmount = 0.01f;
	float midpoint = 0f;

	void Start()
	{
		Idle ();
	}

	void Update () 
	{
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 cSharpConversion = transform.localPosition; 

		waveslice = Mathf.Sin(timer);
		timer = timer + bobbingSpeed;
		if (timer > Mathf.PI * 2) {
			timer = timer - (Mathf.PI * 2);
		}

			
		if (waveslice != 0) {
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			//totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			//translateChange = totalAxes * translateChange;
			cSharpConversion.y = midpoint + translateChange;
		}
		else {
			cSharpConversion.y = midpoint;
		}

		transform.localPosition = cSharpConversion;
	}


	public void Idle()
	{
		if (bobbingAmount != 0.01f)
		{
			bobbingAmount = 0.01f;
			bobbingSpeed = 0.05f;
		}
	}
	public void Walk()
	{
		if (bobbingAmount != 0.012f) 
		{
			bobbingAmount = 0.012f;
			bobbingSpeed = 0.3f;
		}
	}
	public void Run()
	{
		if (bobbingAmount != 0.03f) 
		{
			bobbingAmount = 0.03f;
			bobbingSpeed = 0.35f;
		}
	}
}

