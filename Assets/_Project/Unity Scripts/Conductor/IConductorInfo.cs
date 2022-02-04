public interface IConductorInfo
{
    float BeatPulse { get; set; }
    float BeatPulseABS { get; set; }
    float BeatsPerSecond { get; set; }
    int CompletedLoops { get; set; }
    int CurrentBeatIn4x4Time { get; set; }
    float DSPSongTime { get; set; }
    float SecondsPerBeat { get; set; }
    float SongBPM { get; set; }
    float SongPositionInBeats { get; set; }
    float SongPositionInSeconds { get; set; }
}