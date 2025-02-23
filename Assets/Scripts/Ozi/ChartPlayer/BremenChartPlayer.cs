using Ozi.ChartEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartPlayer {
    public enum BremenNoteResult {
        Perfect,
        Miss,
        None,
    }

    public class BremenChartPlayer : MonoBehaviour {
        private const float CHART_START_OFFSET_BEAT = 4.0f;

        private const int MISS_TIMING_RANGE_MS = 120;
        private const int PERFECT_TIMING_RANGE_MS = 100;

        private const float MISS_TIMING_RANGE = MISS_TIMING_RANGE_MS * 0.001f;
        private const float PERFECT_TIMING_RANGE = PERFECT_TIMING_RANGE_MS * 0.001f;

        private const float HALF_MISS_TIMING = MISS_TIMING_RANGE * 0.5f;
        private const float HALF_PERFECT_TIMING = PERFECT_TIMING_RANGE * 0.5f;

        [field: Header("Requires")]
        [field: SerializeField] public BremenChartAudioPlayer AudioPlayer { get; private set; }

        [field: Header("Settings")]
        [field: SerializeField] public bool IsAutoPlay { get; set; } = false;

        [field: Header("Debugs")]
        [field: SerializeField] public int NoteIndex { get; private set; } = 0;
        [field: SerializeField] public int Combo { get; private set; }
        [field: SerializeField] public BremenChart Chart { get; private set; }
        [field: SerializeField] public List<float> Timings { get; private set; }

        public event Action<int> OnComboAdd;    // combo: int
        public event Action OnComboReset;
        public event Action<BremenChart> OnLoadedChart; // chart: BremenChart
        public event Action<float> OnPlayed;  // start_time: float
        public event Action OnStoped;
        public event Action OnReseted;

        private void Update() {
            if (NoteIndex >= Timings.Count) {
                return;
            }
            
            var time = AudioPlayer.Time;
            var min_timing = time - HALF_PERFECT_TIMING;
            var max_timing = time + HALF_PERFECT_TIMING;
            var timing = Timings[NoteIndex];

            if (IsAutoPlay) {
                if (time <= timing) {
                    OnHitNote();
                }
            } 
            else if (timing < min_timing) {
                OnMissNote();
            }
        }

        public BremenNoteResult TryProcessNote() {
            var time = AudioPlayer.Time;
            var miss_min_timing = time - HALF_MISS_TIMING;
            var miss_max_timing = time - HALF_MISS_TIMING;
            var perfect_min_timing = time - HALF_PERFECT_TIMING;
            var perfect_max_timing = time + HALF_PERFECT_TIMING;

            var timing = Timings[NoteIndex];

            if (perfect_min_timing <= timing && timing <= perfect_max_timing) {
                OnHitNote();
                
                return BremenNoteResult.Perfect;
            }
            else if (miss_min_timing <= timing && timing <= miss_max_timing) {
                OnMissNote();

                return BremenNoteResult.Miss;
            }

            return BremenNoteResult.None;
        }

        private void OnMissNote() {
            Combo = 0;
            NoteIndex++;

            OnComboReset?.Invoke();
        }
        private void OnHitNote() {
            Combo++;
            NoteIndex++;

            OnComboAdd?.Invoke(Combo);
        }

        public void LoadChart(BremenChart chart, AudioClip clip) {
            var pitch = chart.pitch * 0.01f;
            AudioPlayer.Clip = clip;
            AudioPlayer.Pitch = pitch;
            AudioPlayer.Volume = chart.volume * 0.01f;

            AudioPlayer.Offset = (CHART_START_OFFSET_BEAT * chart.SecondsPerBeat) * pitch;

            Timings = chart.ToTimings();

            Chart = chart;

            OnLoadedChart?.Invoke(chart);
        }

        public void Play(float start_time = 0.0f, bool is_auto_play = false) {
            ResetPlayData();

            IsAutoPlay = is_auto_play;

            while (NoteIndex < Timings.Count 
                && Timings[NoteIndex] < start_time) {
                NoteIndex++;
            }

            AudioPlayer.Play(start_time);

            OnPlayed?.Invoke(start_time);
        }
        public void Stop() {
            AudioPlayer.Stop();

            OnStoped?.Invoke();
        }

        private void ResetPlayData() {
            Combo = 0;
            NoteIndex = 0;

            OnReseted?.Invoke();
        }
    }
}