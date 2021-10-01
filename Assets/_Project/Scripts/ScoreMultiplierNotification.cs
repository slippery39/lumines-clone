using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject screenFlash;

    [SerializeField]
    private TextMeshProUGUI multiplierLabel;
    void Start()
    {
        canvas.EnsureInitialized(this);
        multiplierLabel.EnsureInitialized(this);
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

    public void SetMultiplier(int multiplier)
    {
        multiplierLabel.SetText($"X{multiplier}");   
    }

    private void ResetAnimation()
    {
        labels.transform.position = labelsInitialPosition;
        arrows.transform.position = arrows.transform.position;
        canvas.GetComponent<CanvasGroup>().alpha = 1;
        screenFlash.SetActive(true);
        screenFlash.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void PlayAnimation()
    {
        ResetAnimation();
        var sequence = DOTween.Sequence();


        var color = screenFlash.GetComponent<Image>().color;


        
        sequence.Append(arrows.transform.DOMoveX(-800f, 0.5f).SetEase(Ease.OutExpo));
        sequence.Insert(0.1f,DOTween.To(() => color, x => { color = x; screenFlash.GetComponent<Image>().color = color; }, new Color(0, 0, 0, 0), 0.7f).SetEase(Ease.OutExpo));
        sequence.Insert(0.2f, labels.transform.DOMoveX(canvas.GetComponent<RectTransform>().rect.width * 0.65f, 0.5f).SetEase(Ease.OutExpo));
        sequence.OnComplete(() =>
        {
            screenFlash.SetActive(false);
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
