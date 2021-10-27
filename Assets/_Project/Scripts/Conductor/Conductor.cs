using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Making the Conductor a Singleton, as it is used by almost all of our UI elements. It is unlikely I will need to change it from a Singleton for this project since it is just a 
//small project.
public class Conductor : MonoBehaviour
{


    public static Conductor Instance;

    private ConductorInfo previousFrameInfo;
    private ConductorInfo currentFrameInfo;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    [SerializeField]
    private float songBpm;

    //The number of seconds for each song beat
    private float secPerBeat;

    //The numer of beats for each second
    private float beatsPerSecond;


    /*These fields here, i want to be able to compare the previous frame position to this frames position*/

    //Current song position, in seconds
    private float songPosition;

    //Current song position, in beats
    private float songPositionInBeats;

    //How many seconds have passed since the song started
    private float dspSongTime;

    //The Beat as expressed as a pulse.
    private float beatPulse;

    private float beatPulseABS;

    //The amount of beats that have passed since the last update (fractional);
    private float deltaBeats;

    private int currentBeatIn4x4Time;

    /** done */

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;


    public event Action<ConductorInfo> OnBeat;  

    [SerializeField]
    TextMesh debugText;

    public float SongBpm { get => songBpm; set => songBpm = value; }
    public float SecPerBeat { get => secPerBeat; set => secPerBeat = value; }
    public float BeatsPerSecond { get => beatsPerSecond; set => beatsPerSecond = value; }
    public float SongPosition { get => songPosition; set => songPosition = value; }
    public float SongPositionInBeats { get => songPositionInBeats; set => songPositionInBeats = value; }
    public float DspSongTime { get => dspSongTime; set => dspSongTime = value; }
    public float BeatPulse { get => beatPulse; set => beatPulse = value; }
    public float BeatPulseABS { get => beatPulseABS; set => beatPulseABS = value; }
    public float DeltaBeats { get => deltaBeats; set => deltaBeats = value; }
    public int CurrentBeatIn4x4Time { get => currentBeatIn4x4Time; set => currentBeatIn4x4Time = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            throw new Exception("ERROR: Tried to initialize 2 Conductors. There should only be one Conductor. Check and make sure there are no duplicate conductors in the scene");
        }
        Instance = this;
    }

    void Start()
    {

        if (songBpm == 0)
        {
            throw new Exception("Song BMP has not been set in the conductor. Nothing will move. Reccomend setting to something between 60 and 180");
        }

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

    // Update is called once per frame
    void Update()
    {
        UpdateBeatInfo();
        DebugInfo();
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



    private void UpdateBeatInfo()
    {
        //determine how many seconds since the song started
        var previousSongPositionInBeats = songPositionInBeats;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
        deltaBeats = songPositionInBeats - previousSongPositionInBeats;
        currentBeatIn4x4Time = Convert.ToInt32(Math.Floor(songPositionInBeats) % 4);

        if (Math.Floor(songPositionInBeats) > Math.Floor(previousSongPositionInBeats))
        {
            OnBeat?.Invoke(new ConductorInfo()
            {
                SongBPM = songBpm,
                SecondsPerBeat = secPerBeat,
                BeatsPerSecond = beatsPerSecond,
                SongPosition = songPosition,
                SongPositionInBeats = songPositionInBeats,
                DSPSongTime = dspSongTime,
                BeatPulse = beatPulse,
                BeatPulseABS = beatPulseABS,
                CurrentBeatIn4x4Time = currentBeatIn4x4Time
            });
        }
    }

   

}
