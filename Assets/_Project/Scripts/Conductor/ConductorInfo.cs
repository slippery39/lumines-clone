using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class ConductorInfo : IConductorInfo
{

    #region Properties
    /// <summary>
    /// Beats Per Minute for the Song
    /// </summary>
    public float SongBPM { get { return _songBpm; } set { _songBpm = value;  UpdateFromBPM(value); } }
    /// <summary>
    /// the number of seconds it takes for each beat in the song
    /// </summary>
    public float SecondsPerBeat { get { return _secPerBeat; } set { _secPerBeat = value; } }
    //The numer of beats for each second

    /// <summary>
    /// The amount of beats per second
    /// </summary>
    public float BeatsPerSecond { get { return _beatsPerSecond; } set { _beatsPerSecond = value; } }

    /// <summary>
    /// Current Song Position in Seconds
    /// </summary>
    public float SongPositionInSeconds { get { return _songPositionInSeconds; } set { _songPositionInSeconds = value; } }

    /// <summary>
    /// Current Song Position in Beats
    /// </summary>
    public float SongPositionInBeats { get { return _songPositionInBeats; } set { _songPositionInBeats = value; } }

    /// <summary>
    /// When the music started... grabbed from Unity's AudioSource.dspSongTime.
    /// </summary>
    public float DSPSongTime { get { return _dspSongTime; } set { _dspSongTime = value; } }

    /// <summary>
    /// A pulsing value normalixed between 0 and 1 based on the beat.
    /// </summary>
    public float BeatPulse { get { return _beatPulse; } set { _beatPulse = value; } }
    public float BeatPulseABS { get { return _beatPulseABS; } set { _beatPulseABS = value; } }

    /// <summary>
    /// The current beat normalized to 4x4 time. Where 0 is the first beat and 3 is the last beat.
    /// </summary>
    public int CurrentBeatIn4x4Time { get { return _currentBeatIn4x4Time; } set { _currentBeatIn4x4Time = value; } }


    /// <summary>
    /// The smount of loops that have been passed.
    /// </summary>
    public int CompletedLoops { get { return _completedLoops; } set { _completedLoops = value; } }

    #endregion

    #region Helper Methods
    public string DebugInfo()
    {
        return "Beats Per Minute :" + SongBPM.ToString() + Environment.NewLine +
           "Beats Per Second : " +  BeatsPerSecond.ToString() + Environment.NewLine +
           "Seconds Per Beat : " + SecondsPerBeat.ToString() + Environment.NewLine +
           "Current Song Time : " + SongPositionInSeconds.ToString() + Environment.NewLine +
           "Song Position In Beats : " + SongPositionInBeats.ToString() + Environment.NewLine;
    }

    public ConductorInfo Clone()
    {
        return new ConductorInfo()
        {
            SongBPM = SongBPM,
            SecondsPerBeat = SecondsPerBeat,
            BeatsPerSecond = BeatsPerSecond,
            SongPositionInSeconds = SongPositionInSeconds,
            SongPositionInBeats = SongPositionInBeats,
            DSPSongTime = DSPSongTime,
            BeatPulse = BeatPulse,
            BeatPulseABS = BeatPulseABS,
            CurrentBeatIn4x4Time = CurrentBeatIn4x4Time,
            CompletedLoops = CompletedLoops,
        };
    }
    #endregion

    #region Private Methods
    private void UpdateFromBPM(float bpmValue)
    {
        //Calculate the number of seconds in each beat
        SecondsPerBeat = 60f / SongBPM;
        //Calculate the number of beats per second
        BeatsPerSecond = SongBPM / 60f;
    }
    #endregion


    #region Private Fields
    [SerializeField]
    private float _songBpm;
    [SerializeField]
    private float _secPerBeat;
    [SerializeField]
    private float _beatsPerSecond;
    [SerializeField]
    private float _songPositionInSeconds;
    [SerializeField]
    private float _songPositionInBeats;
    [SerializeField]
    private float _dspSongTime;
    [SerializeField]
    private float _beatPulse;
    [SerializeField]
    private float _beatPulseABS;
    [SerializeField]
    private int _currentBeatIn4x4Time;
    [SerializeField]
    private int _completedLoops;
    #endregion

}

