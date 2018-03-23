using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PrezManager : MonoBehaviour {

    CanvasGroup canvasGroup1;
	public CanvasGroup canvasGroup2;

    public GameObject canvasCursor;
    public GameObject player;
	public HeadBob headbob;

    public GameObject[] slides;
    int slideIndex = 0;

	Transform camera;

	Vector3 originPoz;
	Vector3 originRot;

	bool canDeactivate = true;

	public AudioMixer SfxAudioMixer;
	public AudioMixer ambientAudioMixer;

	[Header ("Audio")]
	public AudioSource Open_Audio;
	public AudioSource Close_Audio;
	public AudioSource ChangeSlide_Audio;

    private void Start()
    {
		canvasGroup1 = GetComponent<CanvasGroup>();
		camera = Camera.main.transform;
		this.enabled = false;
    }

    public void ActivateCanvas()
    {
		Open_Audio.Play ();

		canDeactivate = false;
		headbob.enabled = false;
		originPoz = camera.transform.position;
		originRot = camera.transform.eulerAngles;

		camera.DOKill ();

		camera.DOMove (new Vector3 (26.5f, 2, 7.15f), 2f).OnComplete(() =>
			{
				canvasGroup1.DOFade(1, 0.5f).OnComplete(() =>
					{
						canvasGroup2.DOFade(1, 1.5f);
						canDeactivate = true;
						SfxAudioMixer.DOSetFloat("SfxVolume", -50f, 3f);
						ambientAudioMixer.DOSetFloat("AmbientVolume", -20f, 10f);
					});
			});
		camera.DORotate (Vector3.zero, 2f);
    }

    void DeactivateCanvas()
    {
		SfxAudioMixer.DOSetFloat("SfxVolume", 0f, 0.2f);
		ambientAudioMixer.DOSetFloat("AmbientVolume", 0f, 0.2f);

		Close_Audio.Play ();

		canDeactivate = false;
		camera.DOKill ();

		canvasGroup2.DOFade(0, 0.5f).OnComplete(() =>
			{
				canvasGroup1.DOFade(0, 1f);
				camera.DOMove (originPoz, 2f).OnComplete(() =>
					{
						canvasCursor.SetActive(true);
						player.GetComponent<FpsController>().enabled = true;
						player.GetComponent<ActivateCanvas>().enabled = true;
						headbob.enabled = true;
						this.enabled = false;

					});
				camera.DORotate (originRot, 2f);
			});
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.D) && slideIndex != 7)
        {
            changeSlide(true);
        }
		if (Input.GetKeyDown(KeyCode.Q) && slideIndex != 0)
        {
            changeSlide(false);
        }

		if (Input.GetKeyDown(KeyCode.Escape) && canDeactivate)
            DeactivateCanvas();
    }

    void changeSlide(bool indexUp)
    {
		ChangeSlide_Audio.pitch = Random.Range (0.9f, 1.1f);
		ChangeSlide_Audio.Play ();

        slides[slideIndex].SetActive(false);

        if (indexUp)
            slideIndex += 1;
        else
            slideIndex -= 1;

        slides[slideIndex].SetActive(true);
    }
		
}
