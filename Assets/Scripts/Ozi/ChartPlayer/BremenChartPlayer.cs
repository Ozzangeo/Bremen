using Ozi.ChartEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozi.ChartPlayer {
    public class BremenChartPlayer : MonoBehaviour {
        private const float VISUALIZE_NOTE_POOL_BEAT = 8.0f;
        private const float CHART_START_OFFSET_BEAT = 4.0f;

        private const float JUDGMENT_TIMING_MS = 100;   // (ms)
        private const float JUDGMENT_TIMING = JUDGMENT_TIMING_MS * 0.001f;
        private const float HALF_JUDGMENT_TIMING = JUDGMENT_TIMING * 0.5f;

        [Header("Requires")]
        [SerializeField] private BremenChartAudioPlayer _audioPlayer;
        [SerializeField] private BremenNote _notePrefab;

        [Header("Settings")]
        [SerializeField] private float _noteHeight = 0.0f;

        [Header("Debugs")]
        [SerializeField] private bool _isAutoPlay = false;
        [SerializeField] private int _combo;
        [SerializeField] private List<float> _timings;

        public event Action<int> OnComboAdd;    // combo: int
        public event Action OnComboReset;

        public IObjectPool<BremenNote> NotePool { get; private set; }
        [Header("Note Debugs")]
        [SerializeField] private int _noteIndex = 0;
        [SerializeField] private int _usingNoteIndex = 0;
        [SerializeField] private int _visualizedNoteIndex = 0;
        [SerializeField] private float _visualizeTiming = 0.0f;
        [SerializeField] private Transform _notesParent;
        [SerializeField] private List<BremenNote> _usingNotes = new();

        private void Awake() {
            NotePool = new ObjectPool<BremenNote>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);

            var position = _notesParent.position;
            position.y = _noteHeight;
            _notesParent.position = position;
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

            if ((Input.GetKeyDown(KeyCode.Space) || _isAutoPlay)
                && min_timing <= timing && timing <= max_timing) {
                _combo++;
                _noteIndex++;
                
                OnComboAdd?.Invoke(_combo);
            }
            else if (timing < min_timing) {
                _combo = 0;
                _noteIndex++;

                OnComboReset?.Invoke();
            }

            ReleaseVisualizeNote();

            VisualizeNote();
        }

        public void LoadChart(BremenChart chart, AudioClip clip) {
            var pitch = chart.pitch * 0.01f;
            _audioPlayer.Clip = clip;
            _audioPlayer.Pitch = pitch;
            _audioPlayer.Volume = chart.volume * 0.01f;

            var seconds_per_beat = chart.SecondsPerBeat;
            _audioPlayer.Offset = CHART_START_OFFSET_BEAT * seconds_per_beat * pitch;
            _visualizeTiming = VISUALIZE_NOTE_POOL_BEAT * seconds_per_beat;

            _timings = chart.ToTimings();
        }

        public void Play(float time = 0.0f, bool is_auto_play = false) {
            ResetPlayer();

            _isAutoPlay = is_auto_play;

            while (_noteIndex < _timings.Count 
                && _timings[_noteIndex] < time) {
                _noteIndex++;
            }

            VisualizeNote();

            if (time != 0.0f && _usingNotes.Count > 0) {
                _usingNotes[_noteIndex].Image.color = Color.green;
            }

            _audioPlayer.Play(time);
        }
        public void Stop() {
            _audioPlayer.Stop();

            ReleaseVisualizeNote();

            _usingNotes.Clear();
        }

        private void ResetPlayer() {
            _combo = 0;
            _noteIndex = 0;
            _visualizedNoteIndex = 0;
            _usingNoteIndex = 0;
        }
        private void ReleaseVisualizeNote() {
            for (int i = _usingNoteIndex; i < _noteIndex; i++) {
                var using_note = _usingNotes[i];

                NotePool.Release(using_note);

                _usingNoteIndex++;
            }
        }
        private void VisualizeNote() {
            var visualize_time = _audioPlayer.Time + _visualizeTiming;

            while (_visualizedNoteIndex < _timings.Count
                && _timings[_visualizedNoteIndex] <= visualize_time) {
                var note = NotePool.Get();

                note.Timing = _timings[_visualizedNoteIndex];
                note.VisualizeTiming = _visualizeTiming;

                _usingNotes.Add(note);

                _visualizedNoteIndex++;
            }
        }
    }
}