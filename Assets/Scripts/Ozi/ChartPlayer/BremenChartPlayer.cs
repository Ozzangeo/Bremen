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
            _source.volume = chart.volume;
            _source.clip = clip;

            _notes = chart.notes;
        }

        public void Play(float time = 0.0f) {
            _source.time = time;
            _source.Play();
        }
    }
}