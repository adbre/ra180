namespace Ra180.UI
{
    public interface IAudio
    {
        void Play(AudioFile file);
        void PlaySync(AudioFile file);
    }

    public enum AudioFile
    {
        Data,
        OPM,
    }
}
