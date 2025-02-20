using Ozi.ChartEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ozi.ChartPlayer {
    [RequireComponent(typeof(AudioSource))]
    public class BremenChartPlayer : MonoBehaviour {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<float> _timings;

        private void OnEnable() {
            if (_source == null) {
                _source = GetComponent<AudioSource>();
            }
        }

        public void LoadChart(BremenChart chart, AudioClip clip) {
            _source.pitch = chart.pitch * 0.01f;
            _source.volume = chart.volume * 0.01f;
            _source.clip = clip;

            _timings = chart.ToTimings();
        }

        public void Play(float time = 0.0f) {
            _source.time = time;
            _source.Play();
        }
        public void Stop() {
            _source.Stop();
        }
    }
}