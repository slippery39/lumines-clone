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
    private float currentTime=0;
    public float CurrentTime { get { return currentTime; } set { currentTime = value; } }



    // Update is called once per frame
    void Update()
    {
        timeLabel.SetText(FormatTime(currentTime));
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
