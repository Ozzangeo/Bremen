using Ozi.Extension;
using Ozi.Extension.Component;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Ozi.ChartEditor.Feature {
    public class BremenChartEditorUI : BremenChartEditorFeature {
        [Header("UI Components")]
        [SerializeField] private SequenceSelectableUI _play_sequence_ui;
        [SerializeField] private UIElementField _song_filename_field;
        [SerializeField] private UIElementField _bpm_field;
        [SerializeField] private UIElementField _offset_field;
        [SerializeField] private GameObject _dirty_ui;

        private void Start() {
            _editor.OnSongLoaded += OnSongLoaded;
            _editor.OnChartReseted += OnChartReseted;
        }

        private void OnSongLoaded(string path, AudioClip clip) {
            if (_song_filename_field != null) {
                _song_filename_field.InputField.text = Path.GetFileName(path);
            }
        }
        private void OnChartReseted() {
            if (_bpm_field != null) {
                _bpm_field.InputField.text = $"{_editor.Chart.bpm:###,###,###.##}";
            }

            if (_offset_field != null) {
                _offset_field.InputField.text = $"{_editor.Chart.offset:###,###,##0}";
            }

            if (_song_filename_field != null) {
                _song_filename_field.InputField.text = "";
            }
        }

        private void Update() {
            if (_dirty_ui != null) {
                _dirty_ui.SetActive(_editor.Dirty);
            }
        }

        public void GenerateNewChart() {
            _editor.ResetChart();
        }

        public void LoadSongWithDialog() {
            if (!_editor.Data.IsExistWorkSpace) {
                // display logic with exception

                return;
            }

            _editor.LoadSongWithDialog();
        }
        public void PlayChart() {
            if (_editor.PlayChart()) {
                if (_play_sequence_ui != null) {
                    _play_sequence_ui.Next();
                }
            }
        }
        public void StopChart() {
            if (_editor.StopChart()) {
                if (_play_sequence_ui != null) {
                    _play_sequence_ui.Next();
                }
            }
        }

        public void Save() {
            _editor.Save();
        }
        public void LoadWithDialog() {
            _editor.LoadWithDialog();
        }
    }
}