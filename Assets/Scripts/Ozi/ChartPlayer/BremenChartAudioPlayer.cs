using UnityEngine;

namespace Ozi.ChartPlayer {
    public class BremenChartAudioPlayer : MonoBehaviour {
        [field: SerializeField] public float Offset { get; set; }
        [SerializeField] private float _offsetTime = 0.0f;   // value_length = 0.0f ~ (Offset: float);
        [field: SerializeField] public int SourceId { get; private set; }

        public ManagedAudioSource AudioSources => AudioManager.GetAudioSource(PlayType.BGM);
        public AudioSource AudioSource => AudioSources[SourceId];

        public float Time {
            get => AudioSource.time;
            set => AudioSource.time = value;
        }
        [field: SerializeField] public bool IsPlaying { get; private set; }

        private void Update() {
            if (!IsPlaying) {
                return;
            }

            if (_offsetTime > 0.0f) {
                _offsetTime -= UnityEngine.Time.deltaTime * AudioSource.pitch;
            }
            else {
                AudioSource.time += Mathf.Abs(_offsetTime);
                _offsetTime = 0.0f;

                AudioSource.UnPause();
            }
        }

        public void Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, float time = 0.0f) {
            SourceId = AudioManager.Play(PlayType.BGM, volume, pitch, clip: clip);

            if (time > Offset) {
                AudioSource.time = time - Offset;
            } else {
                AudioSource.Pause();

                _offsetTime = Offset - time;
            }

            IsPlaying = true;
        }
        public void Stop() {
            AudioManager.Stop(PlayType.BGM, SourceId);

            IsPlaying = false;
        }
    }
}