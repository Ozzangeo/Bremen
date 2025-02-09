using Ozi.ChartEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ozi.ChartPlayer {
    [RequireComponent(typeof(AudioSource))]
    public class BremenChartPlayer : MonoBehaviour {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<BremenNote> _notes;

        private void OnEnable() {
            if (_source == null) {
                _source = GetComponent<AudioSource>();
            }
        }

        public void LoadChart(BremenChart chart, AudioClip clip) {
            _source.pitch = chart.Pitch * 0.01f;
            _source.volume = chart.Volume * 0.01f;
            _source.clip = clip;

            _notes = chart.Notes;
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