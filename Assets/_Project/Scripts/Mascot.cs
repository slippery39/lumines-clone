using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mascot : MonoBehaviour
{

    [SerializeField]
    public float songPositionInBeats= 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //what is time.y? -song position in beats?
        float signedPulse = Mathf.Cos(songPositionInBeats * Mathf.PI) % Mathf.PI;
        float pulseValue = Mathf.Abs(signedPulse);
        pulseValue = EaseOutSine(pulseValue);
        float scaleValue = (pulseValue * 0.5f) + 2;
        transform.localScale = new Vector3(scaleValue,scaleValue,scaleValue);

        transform.eulerAngles = new Vector3(0, 0 , signedPulse * 25);
    }

    private float EaseOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
