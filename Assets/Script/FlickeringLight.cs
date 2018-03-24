using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

	public float minOnTime;
	public float maxOnTime;
	public float minOffTime;
	public float maxOffTime;

	public AudioSource Flicker_Audio;

	float originIntensity;

	Light light;

	void Start ()
	{
		light = GetComponent<Light>();
		originIntensity = light.intensity;

		StartCoroutine(Flicker());
	}


	IEnumerator Flicker()
	{
		light.intensity = originIntensity;
		yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));
		light.intensity = 0;
		Flicker_Audio.pitch = Random.Range (0.9f, 1.1f);
		Flicker_Audio.Play ();
		yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));

		StartCoroutine(Flicker());
	}
}
