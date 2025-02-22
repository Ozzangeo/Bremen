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

                IsPlaying = false;
            }
        }

        public float Time {
            get => _source.time + _offsetTime;
            set {
                var time = value - _offsetTime;

                if (time > 0.0f) {
                    _source.time = time;
                } else {
                    _source.time = 0.0f;

                    _offsetTime += time;
                }
            }
        }
        public float RealTime {
            get => _source.time;
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

        [field: SerializeField] public bool IsPlaying { get; private set; }

        private void Awake() {
            if (_source == null) {
                _source = GetComponent<AudioSource>();
            }
        }
        private void Update() {
            if (!IsPlaying) {
                return;
            }
            
            if (_offsetTime < Offset) {
                _offsetTime += UnityEngine.Time.deltaTime;

                if (_offsetTime >= Offset) {
                    var offset = _offsetTime - Offset;

                    _source.time += offset;
                    _offsetTime = Offset;

                    _source.UnPause();
                }
            }
        }

        public void Play(float offset = 0.0f) {
            IsPlaying = true;
            
            _offsetTime = offset;

            _source.Play();
            if (Offset > 0.0f) {
                _source.Pause();
            }
        }
        public void Stop() {
            IsPlaying = false;

            _source.Stop();
        }
    }
}