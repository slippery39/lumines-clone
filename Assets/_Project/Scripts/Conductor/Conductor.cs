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

    public float SongBpm { get => songBpm; set => SetBPM(value); }
    public float SecPerBeat { get => secPerBeat; }
    public float BeatsPerSecond { get => beatsPerSecond;}
    public float SongPosition { get => songPosition;}
    public float SongPositionInBeats { get => songPositionInBeats; }
    public float DspSongTime { get => dspSongTime; }
    public float BeatPulse { get => beatPulse;}
    public float BeatPulseABS { get => beatPulseABS;}
    public float DeltaBeats { get => deltaBeats; }
    public int CurrentBeatIn4x4Time { get => currentBeatIn4x4Time;}

    //the number of beats in each loop
    public float beatsPerLoop = 8;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

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
        /*
        if (songBpm == 0)
        {
            throw new Exception("Song BMP has not been set in the conductor. Nothing will move. Reccomend setting to something between 60 and 180");
        }
        */

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();



        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;     

        //Start the music
        musicSource.Play();       

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeatInfo();

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;



        DebugInfo();
    }

    public void SetFromSkin(Skin skin)
    {
        musicSource.Stop();
        SetBPM(skin.BPM);
        musicSource.clip = skin.Music.clip;
        musicSource.Play();
    }

    private void DebugInfo()
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

    private void SetBPM(float newBPM)
    {
        songBpm = newBPM;
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Calculate the number of beats per second
        beatsPerSecond = songBpm / 60f;
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
