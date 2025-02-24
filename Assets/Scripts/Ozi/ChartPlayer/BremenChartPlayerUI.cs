using Ozi.ChartEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartPlayer {
    public class BremenChartPlayerUI : MonoBehaviour {
        private const float VISUALIZE_NOTE_POOL_BEAT = 8.0f;

        [SerializeField] private BremenChartPlayer _player;

        [Header("Settings")]
        [SerializeField] private BremenNote _notePrefab;
        [SerializeField] private GameObject _judgmentLinePrefab;
        [SerializeField] private float _noteHeight = 0.0f;

        [Header("Debugs")]
        [SerializeField] private int _visualizeNoteIndex = 0;
        [SerializeField] private float _visualizeTiming = 0.0f;
        [SerializeField] private GameObject _judgmentLine;

        [field: SerializeField] public BremenNotePool LeftNotePool { get; private set; }
        [field: SerializeField] public BremenNotePool RightNotePool { get; private set; }

        private List<BremenNotePool> _notePools = new();

        private Transform GenerateNoteParent(string name) {
            var parent = new GameObject(name);
            parent.AddComponent<RectTransform>();
            parent.transform.SetParent(transform);
            parent.transform.localPosition = Vector3.zero;

            return parent.transform;
        }

        private void Awake() {
            if (_player == null) {
                _player = GameObject.FindAnyObjectByType<BremenChartPlayer>();
            }

            _judgmentLine = Instantiate(_judgmentLinePrefab, transform);

            var left_parent = GenerateNoteParent("Left Notes");
            var right_parent = GenerateNoteParent("Right Notes");
            var right_scale = right_parent.localScale;
            right_scale.x = -1.0f;
            right_parent.localScale = right_scale;

            LeftNotePool = new BremenNotePool(_player.AudioPlayer, _notePrefab, left_parent) { };
            RightNotePool = new BremenNotePool(_player.AudioPlayer, _notePrefab, right_parent) { 
                Color = Color.gray,
            };

            _notePools = new() {
                LeftNotePool,
                RightNotePool,
            };

            _player.OnLoadedChart += OnLoadedChart;
            _player.OnReseted += OnReseted;
            _player.OnPlayed += OnPlayed;
            _player.OnStoped += OnStoped;
        }

        private void Update() {
            MatchToHeight();

            if (_player.AudioPlayer.IsPlaying) {
                ReleaseSomePool();

                ShowSomePool();
            }
        }

        private void ReleaseSomePool() {
            foreach (var pool in _notePools) {
                pool.ReleaseSome(_player.NoteIndex);
            }
        }
        private void ShowSomePool() {
            if (_visualizeNoteIndex >= _player.Timings.Count) {
                return;
            }

            var visualize_timing = _player.AudioPlayer.Time + _visualizeTiming;

            while (_visualizeNoteIndex < _player.Timings.Count
                && _player.Timings[_visualizeNoteIndex] <= visualize_timing) {
                foreach (var pool in _notePools) {
                    pool.Generate(_player.Timings[_visualizeNoteIndex], _visualizeTiming);
                }

                _visualizeNoteIndex++;
            }
        }

        private void MatchToHeight() {
            SetHeight(_judgmentLine.transform, _noteHeight);
            
            foreach (var pool in _notePools) {
                SetHeight(pool.Parent, _noteHeight);
            }
        }
        private static void SetHeight(Transform transform, float height) {
            var position = transform.position;
            position.y = height;

            transform.position = position;
        }

        private void OnLoadedChart(BremenChart chart) {
            _visualizeTiming = VISUALIZE_NOTE_POOL_BEAT * chart.SecondsPerBeat;
        }
        private void OnReseted() {
            _visualizeNoteIndex = 0;
        }
        private void OnPlayed(float start_time) {
            ShowSomePool();

            if (start_time != 0.0f) {
                foreach (var pool in _notePools) {
                    pool.Focus(_player.NoteIndex);
                }
            }

            _judgmentLine.SetActive(true);
        }
        private void OnStoped() {
            ReleaseSomePool();

            foreach (var pool in _notePools) {
                pool.Clear();
            }

            _judgmentLine.SetActive(false);
        }
    }
}