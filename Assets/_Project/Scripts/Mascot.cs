using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mascot : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        Conductor.Instance.OnBeat += (info) =>
         {
             if (info.CurrentBeatIn4x4Time == 0)
             EmitParticles();
         };
    }

    // Update is called once per frame
    void Update()
    {
        
        float signedPulse = Mathf.Cos(Conductor.Instance.SongPositionInBeats * Mathf.PI) % Mathf.PI; //what is this?
        float pulseValue = Mathf.Abs(signedPulse); //what is this?
        pulseValue = EaseOutSine(pulseValue); //what is this? (easing functoion)
        

        float scaleValue = (pulseValue * 0.5f) + 2;
        transform.localScale = new Vector3(scaleValue,scaleValue,scaleValue);

        transform.eulerAngles = new Vector3(0, 0 , signedPulse * 25);
    }

    private void EmitParticles()
    {
        _particleSystem.Play();
    }

    private float EaseOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
