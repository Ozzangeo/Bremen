using Ookii.Dialogs;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

namespace Ozi.ChartEditor {
    public class BremenChartEditor : MonoBehaviour {
        private const string EDITOR_TITLE = "[ Bremen Chart Editor ]";
        private const string EDITOR_EXTENSION = ".bremen";

        private string DataPath => Path.Combine(UnityEngine.Application.persistentDataPath, "editor_data.json");
        
        [SerializeField] private BremenChartEditorData _data;
        [SerializeField] private BremenChart _chart;

        [SerializeField] private bool _dirty;
        [SerializeField] private AudioSource _source;

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
                FilterIndex = 2,
            };
        }

        private void OnDestroy() {
            var json = JsonUtility.ToJson(_data, true);
            
            File.WriteAllText(DataPath, json);
        }

        public bool ResetChart() {
            _chart = new();
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

            File.Copy(_song_load_dialog.FileName, Path.Combine(_data.work_space_path, song_filename), true);

            _chart.song_filename = song_filename;
            _dirty = true;

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

            _data.last_open_file_path = path;
            _data.work_space_path = path;

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