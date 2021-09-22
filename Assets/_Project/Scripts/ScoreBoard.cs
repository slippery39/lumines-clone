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
    private float _currentTime=0;
    public float CurrentTime { get { return _currentTime; } set { _currentTime = value; } }

    [SerializeField]
    private int _blocksErased;
    public int BlocksErased { get { return _blocksErased; } set{ _blocksErased = value; } }

    public int Score { get => _score; set => _score = value; }
    [SerializeField]
    private int _score;
 








    // Update is called once per frame
    void Update()
    {
        timeLabel.SetText(FormatTime(_currentTime));
        erasedLabel.SetText(_blocksErased.ToString());
        scoreLabel.SetText(_score.ToString());
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
