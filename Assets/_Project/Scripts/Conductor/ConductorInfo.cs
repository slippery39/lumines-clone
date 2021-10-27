using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class ConductorInfo
    {
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float SongBPM { get; set; }
    //The number of seconds for each song beat
    public float SecondsPerBeat { get; set; }
    //The numer of beats for each second

    public float BeatsPerSecond { get; set; }
    /*These fields here, i want to be able to compare the previous frame position to this frames position*/

    //Current song position, in seconds
    public float SongPosition { get; set; }
    //Current song position, in beats
    public float SongPositionInBeats { get; set; }
    //How many seconds have passed since the song started
    public float DSPSongTime { get; set; }
    //The Beat as expressed as a pulse.
    public float BeatPulse { get; set; }
    public float BeatPulseABS { get; set; }
    
    public int CurrentBeatIn4x4Time { get; set; }
}

