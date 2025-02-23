using System.Windows.Forms.VisualStyles;
using UnityEngine;

namespace Ozi.ChartPlayer {
    [RequireComponent(typeof(AudioSource))]
    public class BremenChartAudioPlayer : MonoBehaviour {
        [SerializeField] private AudioSource _source;

        [field: SerializeField] public float Offset { get; set; }
        [SerializeField] private float _offsetTime = 0.0f;   // value_length = 0.0f ~ (Offset: float);

        public AudioClip Clip {
            get => _source.clip;
            set {
                _source.clip = value;
                _offsetTime = 0.0f;

                _source.Stop();
            }
        }

        public float Time {
            get => _source.time - _offsetTime;
            set => _source.time = value;
        }

        public float Pitch {
            get => _source.pitch;
            set => _source.pitch = value;
        }
        public float Volume {
            get => _source.volume;
            set => _source.volume = value;
        }

        public bool SourceIsPlaying => _source.isPlaying;
        [field: SerializeField] public bool IsPlaying { get; private set; }

        private void Awake() {
            if (_source == null) {
                _source = GetComponent<AudioSource>();
            }
        }
        private void Update() {
            if (SourceIsPlaying
                || !IsPlaying) {
                return;
            }

            if (_offsetTime > 0.0f) {
                _offsetTime -= UnityEngine.Time.deltaTime * _source.pitch;
            }
            else {
                Time = Mathf.Abs(_offsetTime);
                _offsetTime = 0.0f;

                _source.UnPause();
            }
        }

        public void Play(float time = 0.0f) {
            _source.Play();


            if (time > Offset) {
                Time = time - Offset;
            } else {
                _source.Pause();

                _offsetTime = Offset - time;
            }

            IsPlaying = true;
        }
        public void Stop() {
            _source.Stop();

            IsPlaying = false;
        }
    }
}