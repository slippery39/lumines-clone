using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    public TMP_Text  timeLabel;
    [SerializeField]
    public TMP_Text scoreLabel;
    [SerializeField]
    public TMP_Text erasedLabel;

    [SerializeField]
    private TMP_Text scoreAddedLabel;

    [SerializeField]
    private float _currentTime=0;
    public float CurrentTime { get { return _currentTime; } set { _currentTime = value; } }

    [SerializeField]
    private int _blocksErased;
    public int BlocksErased { get { return _blocksErased; } set{ _blocksErased = value; } }

    public int Score { get => _score; set => _score = value; }
    [SerializeField]
    private int _score;


    [SerializeField]
    private float scoreAnimationRiseAmount = 25.0f;
    [SerializeField]
    private float scoreAnimationTime = 1.5f;
    [SerializeField]
    private Vector3 initialScoreAddedPosition;

    private void Awake()
    {
        this.EnsureInitialized(timeLabel);
        this.EnsureInitialized(scoreLabel);
        this.EnsureInitialized(erasedLabel);
        this.EnsureInitialized(scoreAddedLabel);

        initialScoreAddedPosition = scoreAddedLabel.transform.position;
    }



    private void Start()
    {
        scoreAddedLabel.fontMaterial.SetColor("_FaceColor", Color.clear);
    }

    IEnumerator FadeCoroutine()
    {
        float waitTime = 0;
        float fadeTime = scoreAnimationTime;

        Vector3 initialPosition = initialScoreAddedPosition;
        Vector3 endPosition = initialPosition + new Vector3(0, scoreAnimationRiseAmount, 0);

        


        while (waitTime < 1)
        {
            scoreAddedLabel.transform.position = Vector3.Lerp(initialPosition, endPosition, waitTime);
            scoreAddedLabel.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.yellow, Color.clear,waitTime));
            scoreAddedLabel.fontMaterial.SetColor("_OutLineColor", Color.Lerp(Color.yellow, Color.clear, waitTime));
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
    }

    public void AnimateScore(int amount)
    {
        scoreAddedLabel.SetText($"+ {amount.ToString()}");
        StartCoroutine(FadeCoroutine());
    }




    // Update is called once per frame
    void Update()
    {
        timeLabel.SetText(FormatTime(_currentTime));
        erasedLabel.SetText(_blocksErased.ToString());
        scoreLabel.SetText(_score.ToString());

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(FadeCoroutine());
        }


    }    

    int GetMinutes(float totalSeconds)
    {
        return (int)(totalSeconds / 60);

    }
    int GetSeconds(float totalSeconds)
    {
        return (int)(totalSeconds % 60);
    }



    string FormatTime(float totalSeconds)
    {
        return GetMinutes(totalSeconds).ToString().PadLeft(2,'0') + ":" + GetSeconds(totalSeconds).ToString().PadLeft(2,'0');
    }
}
