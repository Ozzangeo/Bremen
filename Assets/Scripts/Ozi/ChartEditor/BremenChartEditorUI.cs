using Ozi.Extension;
using Ozi.Extension.Component;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Ozi.ChartEditor {
    public class BremenChartEditorUI : MonoBehaviour {
        [SerializeField] private BremenChartEditor _editor;

        [Header("UI Components")]
        [SerializeField] private SequenceButton _playSequenceUI;
        [SerializeField] private UIElementField _songFilenameField;
        [SerializeField] private UIElementField _bpmField;
        [SerializeField] private UIElementField _offsetField;
        [SerializeField] private UIElementField _volumeField;
        [SerializeField] private UIElementField _pitchField;

        [SerializeField] private Text _chartTitleText;

        private void Awake() {
            if (_editor == null) {
                _editor = GameObject.FindAnyObjectByType<BremenChartEditor>();
            }

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
                    _editor.Chart.bpm = Mathf.Clamp(bpm, 0.001f, float.MaxValue);
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _offsetField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var offset)) {
                    _editor.Chart.offset = offset;
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _volumeField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var volume)) {
                    _editor.Chart.volume = Mathf.Clamp(volume, 0, 100);
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
            _pitchField.InputField.onEndEdit.AddListener(o => {
                if (int.TryParse(o, out var pitch)) {
                    _editor.Chart.pitch = Mathf.Clamp(pitch, 1, 200);
                    _editor.Dirty = true;

                    UpdateFieldUI();
                }
            });
        }

        private static string GetTextBPM(BremenChart chart) => $"{chart.bpm:###,###,##0.00}";
        private static string GetTextOffset(BremenChart chart) => $"{chart.offset:###,###,##0}";
        private static string GetTextVolume(BremenChart chart) => $"{chart.volume:##0}";
        private static string GetTextPitch(BremenChart chart) => $"{chart.pitch:##0}";

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