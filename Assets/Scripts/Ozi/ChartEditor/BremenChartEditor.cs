using Ookii.Dialogs;
using Ozi.ChartPlayer;
using System;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

namespace Ozi.ChartEditor {
    public class BremenChartEditor : MonoBehaviour {
        private const string EDITOR_TITLE = "[ Bremen Chart Editor ]";
        private const string EDITOR_EXTENSION = ".bremen";

        private string DataPath => Path.Combine(UnityEngine.Application.persistentDataPath, "editor_data.json");

        [Header("Require")]
        [SerializeField] private BremenChartPlayer _chart_player;

        [Header("Debug")]
        [SerializeField] private BremenChartEditorData _data;
        [SerializeField] private BremenChart _chart;
        [SerializeField] private AudioClip _song_clip;
        [SerializeField] private bool _dirty;

        private VistaOpenFileDialog _chart_load_dialog;
        private VistaSaveFileDialog _chart_save_dialog;
        private VistaOpenFileDialog _song_load_dialog;

        public BremenChartEditorData Data => _data;
        public bool Dirty => _dirty;

        private void Awake() {
            string json = "";
            if (File.Exists(DataPath)) {
                json = File.ReadAllText(DataPath);
            }

            _data = (json != "")
                ? JsonUtility.FromJson<BremenChartEditorData>(json)
                : new BremenChartEditorData();

            ResetChart();

            _chart_load_dialog = new VistaOpenFileDialog {
                Title = $"{EDITOR_TITLE} Chart Load",
                Filter = $"bremen files (*{EDITOR_EXTENSION})|*{EDITOR_EXTENSION}",
                FilterIndex = 1,
            };
            _chart_save_dialog = new VistaSaveFileDialog {
                Title = $"{EDITOR_TITLE} Chart Save",
                Filter = $"bremen files (*{EDITOR_EXTENSION})|*{EDITOR_EXTENSION}",
                FilterIndex = 1,
            };
            _song_load_dialog = new VistaOpenFileDialog {
                Title = $"{EDITOR_TITLE} Song Load",
                Filter = "mp3 files (*.mp3)|*.mp3|ogg files (*.ogg)|*.ogg",
                FilterIndex = 1,
            };
        }

        private void OnEnable() {
            if (_chart_player == null) {
                _chart_player = GameObject.FindAnyObjectByType<BremenChartPlayer>();
            }
        }
        private void OnDestroy() {
            var json = JsonUtility.ToJson(_data, true);
            
            File.WriteAllText(DataPath, json);
        }

        public bool PlayChart(float time = 0.0f) {
            if (_chart_player == null) {
                return false;
            }

            _chart_player.LoadChart(_chart, _song_clip);
            _chart_player.Play(time);

            return true;
        }

        public bool ResetChart() {
            _chart = BremenChart.Generate();
            _data.work_space_path = null;
            _dirty = false;

            if (!File.Exists(_data.last_open_file_path)) {
                _data.last_open_file_path = null;
            }

            return true;
        }
        public bool LoadSong() {
            if (_song_load_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _song_load_dialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            string song_filename = Path.GetFileName(_song_load_dialog.FileName);

            try {
                var path = Path.Combine(_data.work_space_path, song_filename);

                if (!File.Exists(path)) {
                    File.Copy(_song_load_dialog.FileName, path);
                }
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
            finally {
                _chart.song_filename = song_filename;
                _dirty = true;
            }
            
            _song_clip = BremenChartAudioLoader.LoadAudioClip(_song_load_dialog.FileName);
            if (_song_clip == null) {
                return false;
            }

            return true;
        }

        public bool Save() => SaveAs(_data.last_open_file_path);
        public bool SaveAs(string path) {
            var json = JsonUtility.ToJson(_chart, true);
            
            File.WriteAllText(path, json);

            _data.work_space_path = Path.GetDirectoryName(path);
            _data.last_open_file_path = path;

            _dirty = false;

            return true;
        }
        public bool SaveAs() {
            if (_chart_save_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var path = Path.ChangeExtension(_chart_save_dialog.FileName, EDITOR_EXTENSION);

            return SaveAs(path);
        }

        public bool LoadAs(string path) => LoadAs(path, out _);
        public bool LoadAs(string path, out string exception) {
            exception = "";

            if (!File.Exists(path)) {
                return false;
            }

            var json = File.ReadAllText(path);

            BremenChart chart;
            try {
                chart = JsonUtility.FromJson<BremenChart>(json);
            }
            catch (System.Exception e) {
                Debug.LogException(e);

                exception = e.Message;

                return false;
            }

            _chart = chart;

            _data.work_space_path = Path.GetDirectoryName(path);
            _data.last_open_file_path = path;
            _dirty = false;

            if (_chart.song_filename is not null
                && _chart.song_filename != "") {
                var song_path = Path.Combine(_data.work_space_path, _chart.song_filename);

                _song_clip = BremenChartAudioLoader.LoadAudioClip(song_path);
            }

            return true;
        }
        public bool LoadAs() => LoadAs(out _);
        public bool LoadAs(out string exception) {
            exception = "";

            if (_chart_load_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _chart_load_dialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            return LoadAs(_chart_load_dialog.FileName, out exception);
        }
        
        public bool LoadLastOpened() => LoadAs(_data.last_open_file_path);
        public bool LoadLastOpened(out string exception) => LoadAs(_data.last_open_file_path, out exception);
    }
}