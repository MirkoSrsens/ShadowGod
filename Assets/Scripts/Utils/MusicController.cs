using FMODUnity;
using FMOD.Studio;
using UnityEngine;

namespace Razorhead.Core
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private EventReference musicEvent = default; // event:/Music/Main
        private EventInstance _music;
    
        private void Start()
        {
            if (musicEvent.IsNull) { Debug.LogError("Music event not assigned."); return; }
            _music = RuntimeManager.CreateInstance(musicEvent);
            _music.start();
        }
    
        public void SetIntensity(float value) // 0..2 from our FMOD setup
        {
            if (_music.isValid()) _music.setParameterByName("Intensity", value);
        }
    
        private void OnDestroy()
        {
            if (_music.isValid())
            {
                _music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _music.release();
            }
        }
    }
}