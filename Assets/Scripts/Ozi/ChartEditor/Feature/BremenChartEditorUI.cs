using Ozi.Extension;
using Ozi.Extension.Component;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ozi.ChartEditor.Feature {
    public class BremenChartEditorUI : BremenChartEditorFeature {
        [Header("UI Components")]
        [SerializeField] private SequenceButtonUI _playSequenceUI;
        [SerializeField] private ElementFieldUI _songFilenameField;
        [SerializeField] private ElementFieldUI _bpmField;
        [SerializeField] private ElementFieldUI _offsetField;
        [SerializeField] private ElementFieldUI _volumeField;
        [SerializeField] private ElementFieldUI _pitchField;

        [SerializeField] private Text _chartTitleText;

        private void Start() {
            _editor.OnSongLoaded += (path, clip) => {
                _songFilenameField.InputField.text = Path.GetFileName(path);
            };
            _editor.OnChartReseted += () => {
                _songFilenameField.InputField.text = "";

                _chartTitleText.text = "";

                UpdateFieldUI();
            };

            _bpmField.InputField.onEndEdit.AddListener(o => {
                if (float.TryParse(o, out var bpm)) {
                    _editor.Chart.BPM = bpm;
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _offsetField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var offset)) {
                    _editor.Chart.Offset = offset;
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _volumeField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var volume)) {
                    _editor.Chart.Volume = Mathf.Clamp(volume, 0, 100);
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _pitchField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var pitch)) {
                    _editor.Chart.Pitch = Mathf.Clamp(pitch, 1, 200);
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
        }

        private static string GetTextBPM(BremenChart chart) => $"{chart.BPM:###,###,##0.00}";
        private static string GetTextOffset(BremenChart chart) => $"{chart.Offset:###,###,##0}";
        private static string GetTextVolume(BremenChart chart) => $"{chart.Volume:##0}";
        private static string GetTextPitch(BremenChart chart) => $"{chart.Pitch:##0}";

        private void UpdateFieldUI() {
            _bpmField.InputField.text = GetTextBPM(_editor.Chart);
            _offsetField.InputField.text = GetTextOffset(_editor.Chart);
            _volumeField.InputField.text = GetTextVolume(_editor.Chart);
            _pitchField.InputField.text = GetTextPitch(_editor.Chart);
        }

        private void Update() {
            if (_editor.Data.LastOpenedFilePath is null) {
                _chartTitleText.text = $"저장하지 않음";
            }
            else {
                var filename = Path.GetFileName(_editor.OpenFilePath);
                var dirty = _editor.Dirty ? "*" : "";

                _chartTitleText.text = $"{filename}{dirty}";
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
                _playSequenceUI.Next();
            }
        }
        public void StopChart() {
            if (_editor.StopChart()) {
                _playSequenceUI.Next();
            }
        }

        public void Save() {
            _editor.Save();
        }
        public void LoadWithDialog() {
            if (_editor.LoadWithDialog()) {
                UpdateFieldUI();
            }
        }
    }
}