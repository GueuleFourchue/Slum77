using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Canvas_End : MonoBehaviour {

    public GameObject slumTitle;
    public Image questionsImage;

    public GameObject canvasCursor;

    public void EndAnim()
    {
        StartCoroutine(EndCorou());
    }

    IEnumerator EndCorou()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        canvasCursor.SetActive(false);
        yield return new WaitForSeconds(2.7f);
        slumTitle.SetActive(true);
        yield return new WaitForSeconds(5.5f);
        slumTitle.GetComponent<Image>().DOFade(0, 1).OnComplete(() =>
        {
            questionsImage.DOFade(1, 2f);
        });
        
    }
}
