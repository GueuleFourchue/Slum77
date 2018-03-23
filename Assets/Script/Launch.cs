using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Launch : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public Colorful.GrainyBlur blur;
    public UnityStandardAssets.ImageEffects.Bloom bloom;
    public Animator anim;
    public FpsController fps;   

    float lerp;

	void Start ()
    {
        Cursor.visible = false;
        StartCoroutine(LaunchAnim());
        anim.Play("LaunchGame");
    }
	
	
	void Update ()
    {
        if (blur.Radius != 0)
        {
            lerp += Time.deltaTime / 5;
            blur.Radius = Mathf.Lerp(200, 0, lerp);
            bloom.bloomIntensity = Mathf.Lerp(1, 0, lerp);
        }
            
    }

    IEnumerator LaunchAnim()
    {
        yield return new WaitForSeconds(0.5f);
        canvasGroup.DOFade(0, 5f);
        yield return new WaitForSeconds(5);
        blur.enabled = false;
        bloom.enabled = false;
        fps.enabled = true;
    }

}
