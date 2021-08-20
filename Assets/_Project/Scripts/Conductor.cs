using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    // Start is called before the first frame update

    private float _bpm = 123.97f;
    public float BPM { get { return BPM; } set { BPM = value; } }


    private float BPS = 123.97f / 60f;

    int lastBeat = -99;

    AudioSource source;

    [SerializeField]
    TextMesh text;

    [SerializeField]
    TextMesh bpsText;

    void Start()
    {

        source = GetComponent<AudioSource>();
        source.Play();

        bpsText.text = BPS.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //current beat position
        //Debug.Log(GetComponent<AudioSource>().time * BPS);

        // 4/4 beat position
        int beat = 1 + Mathf.CeilToInt(source.time * BPS) % 4;
        if (beat != lastBeat)
        {

            text.text = beat.ToString();
        }
        lastBeat = beat;

        //TODO - move the timeline.
    }

}
