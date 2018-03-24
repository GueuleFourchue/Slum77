using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFlickeringLight : MonoBehaviour {

    public float minOnTime;
    public float maxOnTime;
    public float minOffTime;
    public float maxOffTime;

	public AudioSource Flicker_Audio;

    float originIntensity;

	AreaLight light;

	void Start ()
    {
		light = GetComponent<AreaLight>();
		originIntensity = light.m_Intensity;

        StartCoroutine(Flicker());
	}


    IEnumerator Flicker()
    {
		light.m_Intensity = originIntensity;
        yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));
		light.m_Intensity = 0;
		Flicker_Audio.pitch = Random.Range (0.9f, 1.1f);
		Flicker_Audio.Play ();
        yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));

        StartCoroutine(Flicker());
    }
}
