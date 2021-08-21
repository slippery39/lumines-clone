using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
 
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //The numer of beats for each second
    public float beatsPerSecond;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

  

    [SerializeField]
    TextMesh debugText;

    [SerializeField]
    GameController gameController;

    void Start()
    {

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Calculate the number of beats per second
        beatsPerSecond = songBpm / 60f;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();            

    }

    void DebugInfo()
    {
        debugText.text =
           "Beats Per Minute :" + songBpm.ToString() + Environment.NewLine +
           "Beats Per Second : " + beatsPerSecond.ToString() + Environment.NewLine +
           "Seconds Per Beat : " + secPerBeat.ToString() + Environment.NewLine +
           "Current Song Time : " + musicSource.time.ToString() + Environment.NewLine +
           "Current DSP Song Time : " + dspSongTime + Environment.NewLine +
           "Current Song Position : " + songPosition.ToString() + Environment.NewLine +
           "Song Position In Beats : " + songPositionInBeats.ToString() + Environment.NewLine;
    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        DebugInfo();

    }

}
