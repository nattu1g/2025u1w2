namespace Common.Features.Save
{
    [System.Serializable]
    public class VolumeSave
    {
        public float _bgmVolume;
        public float _seVolume;

        public VolumeSave(float bgmVolume, float seVolume)
        {
            _bgmVolume = bgmVolume;
            _seVolume = seVolume;
        }
    }
}
