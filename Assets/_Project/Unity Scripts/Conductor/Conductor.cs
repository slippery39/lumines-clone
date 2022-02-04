using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Making the Conductor a Singleton, as it is used by almost all of our UI elements. It is unlikely I will need to change it from a Singleton for this project since it is just a 
//small project.
public class Conductor : MonoBehaviour, IConductorInfo, ILuminesGameUpdateable
{


    public static Conductor Instance;

    [SerializeField]
    private ConductorInfo info = new ConductorInfo();

    private bool _isPaused;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;


    public event Action<ConductorInfo> OnBeat;
    public event Action<ConductorInfo> OnLoopBegin;
    public event Action<ConductorInfo> OnLoopEnd;

    public float testAudioSourceTime = 0.0f;
    public float testAudioSourceTimeSamples = 0.0f;

    #region don't touch these variables for now
    //The amount of beats that have passed since the last update (fractional);
    private float deltaBeats;
    public float DeltaBeats { get => deltaBeats; }

    //the number of beats in each loop
    public float beatsPerLoop = 8;


    //TODO check references of this.
    //The current position of the song within the loop in beats.
    private float loopPositionInBeats;

    public float TimeLinePosition { get => loopPositionInBeats / 8; }
    #endregion

    public float SongBPM { get => info.SongBPM; set => info.SongBPM = value; }
    public float SecondsPerBeat { get => info.SecondsPerBeat; set => info.SecondsPerBeat = value; }
    public float BeatsPerSecond { get => info.BeatsPerSecond; set=>info.BeatsPerSecond = value; }
    public float SongPositionInSeconds { get => info.SongPositionInSeconds; set => info.SongPositionInSeconds = value; }
    public float SongPositionInBeats { get => info.SongPositionInBeats; set => info.SongPositionInBeats = value; }

    //TODO - our conductor should not have to worry about the DSPSongTime. We should move this out of this class. 
    public float DSPSongTime { get => info.DSPSongTime; set => info.DSPSongTime = value; }
    public float BeatPulse { get => info.BeatPulse; set => info.BeatPulse = value; }
    public float BeatPulseABS { get => info.BeatPulseABS; set => info.BeatPulseABS = value; }
    public int CurrentBeatIn4x4Time { get => info.CurrentBeatIn4x4Time; set => info.CurrentBeatIn4x4Time = value; }
    public int CompletedLoops { get => info.CompletedLoops; set => info.CompletedLoops = value; }

    private float savedDSPTime = 0f;



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
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

       
        //Record the time when the music starts
        DSPSongTime = (float)AudioSettings.dspTime;  
        //Start the music
        musicSource.Play();    

    }

    // Update is called once per frame
    public void LuminesGameUpdate()
    {
        UpdateBeatInfo();

        //calculate the loop position
        if (SongPositionInBeats >= (CompletedLoops + 1) * beatsPerLoop)
        {
            CompletedLoops++;
            this.OnLoopBegin?.Invoke(CreateBeatInfo());
        }

        loopPositionInBeats = SongPositionInBeats - CompletedLoops * beatsPerLoop;
    }

    public void SetFromSkin(Skin skin)
    {
        musicSource.Stop();
        SongBPM = skin.BPM;
        musicSource.clip = skin.Music.clip;
        musicSource.Play();
        CompletedLoops = 0;
        DSPSongTime = (float)AudioSettings.dspTime;
    }

    private void UpdateBeatInfo()
    {
        //determine how many seconds since the song started
        var previousSongPositionInBeats = SongPositionInBeats;

        SongPositionInSeconds = (float)(AudioSettings.dspTime - DSPSongTime);

        //determine how many beats since the song started
        SongPositionInBeats = SongPositionInSeconds / SecondsPerBeat;
        deltaBeats = SongPositionInBeats - previousSongPositionInBeats;
        CurrentBeatIn4x4Time = Convert.ToInt32(Math.Floor(SongPositionInBeats) % 4);

        if (Math.Floor(SongPositionInBeats) > Math.Floor(previousSongPositionInBeats))
        {
            OnBeat?.Invoke(CreateBeatInfo());
        }
    }

    private ConductorInfo CreateBeatInfo()
    {
        return info.Clone();
    }

   

}
