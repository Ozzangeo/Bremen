using Ozi.ChartEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozi.ChartPlayer {
    [RequireComponent(typeof(AudioSource))]
    public class BremenChartPlayer : MonoBehaviour {
        private const float VISUALIZE_NOTE_POOL_BEAT = 8.0f;
        private const float CHART_START_OFFSET_BEAT = 4.0f;

        private const float JUDGMENT_TIMING_MS = 100;   // (ms)
        private const float JUDGMENT_TIMING = JUDGMENT_TIMING_MS * 0.001f;
        private const float HALF_JUDGMENT_TIMING = JUDGMENT_TIMING * 0.5f;

        [SerializeField] private BremenChartAudioPlayer _audioPlayer;
        [SerializeField] private BremenNote _notePrefab;

        [SerializeField] private List<float> _timings;
        [SerializeField] private int _combo;

        public event Action<int> OnComboAdd;    // combo: int
        public event Action OnComboReset;

        public IObjectPool<BremenNote> NotePool { get; private set; }
        [SerializeField] private Transform _notesParent;
        [SerializeField] private List<BremenNote> _notes = new();
        [SerializeField] private int _noteIndex = 0;
        [SerializeField] private int _visualizedNoteIndex = 0;
        [SerializeField] private float _visualizeTiming = 0.0f;

        private void Awake() {
            NotePool = new ObjectPool<BremenNote>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);
        }

        private BremenNote OnCreateObject() {
            var note = Instantiate(_notePrefab, _notesParent);

            note.gameObject.SetActive(false);

            return note;
        }
        private void OnGetObject(BremenNote note) {
            note.gameObject.SetActive(true);

            note.Image.color = Color.white;
            note.AudioPlayer = _audioPlayer;
        }
        private void OnReleaseObject(BremenNote note) {
            note.gameObject.SetActive(false);
        }
        private void OnDestroyObject(BremenNote note) {
            Destroy(note.gameObject);
        }

        private void Update() {
            if (_noteIndex >= _timings.Count) {
                return;
            }
            
            var time = _audioPlayer.Time;
            var min_timing = time - HALF_JUDGMENT_TIMING;
            var max_timing = time + HALF_JUDGMENT_TIMING;
            var timing = _timings[_noteIndex];

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (min_timing <= timing && timing <= max_timing) {
                    RemoveFrontNote();

                    _combo++;
                    _noteIndex++;

                    OnComboAdd?.Invoke(_combo);

                    return;
                }
            }

            if (timing < min_timing) {
                RemoveFrontNote();

                _combo = 0;
                _noteIndex++;

                OnComboReset?.Invoke();
            }

            VisualizeNote();
        }
        private void RemoveFrontNote() {
            if (_notes.Count > 0) {
                var front = _notes[0];

                _notes.Remove(front);
                NotePool.Release(front);
            }
        }

        public void LoadChart(BremenChart chart, AudioClip clip) {
            var pitch = chart.pitch * 0.01f;
            _audioPlayer.Clip = clip;
            _audioPlayer.Pitch = pitch;
            _audioPlayer.Volume = chart.volume * 0.01f;

            var seconds_per_beat = chart.SecondsPerBeat;
            _visualizeTiming = VISUALIZE_NOTE_POOL_BEAT * seconds_per_beat * pitch;

            var offset = CHART_START_OFFSET_BEAT * seconds_per_beat * pitch;
            _audioPlayer.Offset = offset;

            _timings = chart.ToTimings();
        }

        public void Play(float time = 0.0f) {
            ResetPlayer();

            while (_noteIndex < _timings.Count 
                && _timings[_noteIndex] < time) {
                _noteIndex++;
            }

            VisualizeNote();

            if (_notes.Count > 0) {
                _notes[_noteIndex].Image.color = Color.green;
            }

            _audioPlayer.Play();
            _audioPlayer.RealTime = time;
        }
        public void Stop() {
            _audioPlayer.Stop();

            foreach (var note in _notes) {
                NotePool.Release(note);
            }

            _notes.Clear();
        }

        private void ResetPlayer() {
            _combo = 0;
            _noteIndex = 0;
            _visualizedNoteIndex = 0;
        }
        private void VisualizeNote() {
            var visualize_time = _audioPlayer.Time + _visualizeTiming;

            while (_visualizedNoteIndex < _timings.Count
                && _timings[_visualizedNoteIndex] <= visualize_time) {
                var note = NotePool.Get();

                note.Timing = _timings[_visualizedNoteIndex];
                note.VisualizeTiming = _visualizeTiming;

                _notes.Add(note);

                _visualizedNoteIndex++;
            }
        }
    }
}