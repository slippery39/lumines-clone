using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplierNotification : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 labelsInitialPosition;
    private Vector3 arrowsInitialPosition;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject labels;
    [SerializeField]
    private GameObject arrows;
    void Start()
    {
        canvas.EnsureInitialized(this);
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        labelsInitialPosition = labels.transform.position;
        arrowsInitialPosition = arrows.transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayAnimation();          
        }
    }

    private void ResetAnimation()
    {
        labels.transform.position = labelsInitialPosition;
        arrows.transform.position = arrows.transform.position;
        canvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void PlayAnimation()
    {
        ResetAnimation();
        var sequence = DOTween.Sequence();
        sequence.Append(arrows.transform.DOMoveX(-800f, 0.5f).SetEase(Ease.OutExpo));
        sequence.Insert(0.2f, labels.transform.DOMoveX(canvas.GetComponent<RectTransform>().rect.width * 0.65f, 0.5f).SetEase(Ease.OutExpo));
        sequence.OnComplete(() =>
        {
            StartCoroutine(FadeCoroutine());
        });
    }

    IEnumerator FadeCoroutine()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            canvas.GetComponent<CanvasGroup>().alpha = alpha;
            yield return new WaitForSeconds(0.03f);
        }
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }
}
