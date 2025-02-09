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

        [field: Header("Debug")]
        [field: SerializeField] public BremenChartEditorData Data;
        [field: SerializeField] public BremenChart Chart;
        [field: SerializeField] public AudioClip SongClip { get; private set; }
        [field: SerializeField] public float SongTime { get; set; }
        [field: SerializeField] public bool Dirty { get; set; }

        private VistaOpenFileDialog _chart_load_dialog;
        private VistaSaveFileDialog _chart_save_dialog;
        private VistaOpenFileDialog _song_load_dialog;

        public event Action OnChartReseted;
        public event Action<string, AudioClip> OnSongLoaded;    // path, clip

        private void Awake() {
            string json = "";
            if (File.Exists(DataPath)) {
                json = File.ReadAllText(DataPath);
            }

            Data = (json != "")
                ? JsonUtility.FromJson<BremenChartEditorData>(json)
                : new BremenChartEditorData();

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

        private void Start() {
            ResetChart();
        }

        public bool PlayChart(float time = 0.0f) {
            if (_chart_player == null) {
                return false;
            }

            _chart_player.LoadChart(Chart, SongClip);
            _chart_player.Play(time);

            return true;
        }
        public bool StopChart() {
            if (_chart_player == null) {
                return false;
            }

            _chart_player.Stop();

            return true;
        }

        public bool ResetChart() {
            Chart = BremenChart.Generate();
            Data.work_space_path = null;
            Dirty = false;

            SongClip = null;
            SongTime = 0.0f;

            if (!File.Exists(Data.last_open_file_path)) {
                Data.last_open_file_path = null;
            }

            OnChartReseted?.Invoke();

            return true;
        }

        public bool LoadSong(string path) {
            if (!File.Exists(path)) {
                return false;
            }

            var clip = BremenChartAudioLoader.LoadAudioClip(path);
            if (clip == null) {
                return false;
            }

            SongClip = clip;
            Chart.song_filename = Path.GetFileName(path);
            Dirty = true;

            OnSongLoaded?.Invoke(path, SongClip);

            return true;
        }

        public bool Save() {
            if (Data.IsExistWorkSpace) {
                return SaveAs(Data.last_open_file_path);
            }
            
            return SaveWithDialog();
        }
        public bool SaveAs(string path) {
            var json = JsonUtility.ToJson(Chart, true);
            
            File.WriteAllText(path, json);

            Data.work_space_path = Path.GetDirectoryName(path);
            Data.last_open_file_path = path;

            Dirty = false;

            return true;
        }

        public bool Load(string path) {
            if (!File.Exists(path)) {
                return false;
            }

            var json = File.ReadAllText(path);

            BremenChart chart;
            try {
                chart = JsonUtility.FromJson<BremenChart>(json);
            }
            catch (Exception e) {
                Debug.LogException(e);

                return false;
            }

            Chart = chart;

            Data.work_space_path = Path.GetDirectoryName(path);
            Data.last_open_file_path = path;

            var song_path = Path.Combine(Data.work_space_path, Chart.song_filename);
            LoadSong(song_path);

            Dirty = false;

            return true;
        }
        public bool LoadLastOpened() => Load(Data.last_open_file_path);

        // With Dialog Functions
        public bool LoadSongWithDialog() {
            if (_song_load_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _song_load_dialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            var song_filename = Path.GetFileName(_song_load_dialog.FileName);
            var path = Path.Combine(Data.work_space_path, song_filename);

            try {
                if (!File.Exists(path)) {
                    File.Copy(_song_load_dialog.FileName, path);
                }
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
            finally {
                LoadSong(path);
            }

            return true;
        }
        public bool SaveWithDialog() {
            if (_chart_save_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var path = Path.ChangeExtension(_chart_save_dialog.FileName, EDITOR_EXTENSION);

            return SaveAs(path);
        }
        public bool LoadWithDialog() {
            if (_chart_load_dialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _chart_load_dialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            return Load(_chart_load_dialog.FileName);
        }

        private void OnEnable() {
            if (_chart_player == null) {
                _chart_player = GameObject.FindAnyObjectByType<BremenChartPlayer>();
            }
        }
        private void OnDestroy() {
            var json = JsonUtility.ToJson(Data, true);
            
            File.WriteAllText(DataPath, json);
        }
    }
}