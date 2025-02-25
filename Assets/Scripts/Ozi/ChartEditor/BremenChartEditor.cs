using Ookii.Dialogs;
using Ozi.ChartEditor.Tile;
using Ozi.ChartPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

namespace Ozi.ChartEditor {
    public class BremenChartEditor : MonoBehaviour {
        private const string EDITOR_TITLE = "[ Bremen Chart Editor ]";
        private const string EDITOR_EXTENSION = ".bremen";

        private string DataPath => Path.Combine(UnityEngine.Application.persistentDataPath, "editor_data.json");

        [Header("Require")]
        [SerializeField] private BremenChartPlayer _chartPlayer;
        [SerializeField] private BremenTileEditor _tileEditor;

        [Header("Settings")]
        [SerializeField] private List<GameObject> _editorUis;

        [field: Header("Debug")]
        [field: SerializeField] public BremenChartEditorData Data { get; private set; }
        [field: SerializeField] public BremenChart Chart { get; private set; }
        [field: SerializeField] public AudioClip SongClip { get; private set; }
        [field: SerializeField] public float SongTime { get; set; }
        [field: SerializeField] public bool Dirty { get; set; }
        [field: SerializeField] public string OpenFilePath { get; private set; }

        private VistaOpenFileDialog _chartLoadDialog;
        private VistaSaveFileDialog _chartSaveDialog;
        private VistaOpenFileDialog _songLoadDialog;

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

            _chartLoadDialog = new VistaOpenFileDialog {
                Title = $"{EDITOR_TITLE} Chart Load",
                Filter = $"bremen files (*{EDITOR_EXTENSION})|*{EDITOR_EXTENSION}",
                FilterIndex = 1,
            };
            _chartSaveDialog = new VistaSaveFileDialog {
                Title = $"{EDITOR_TITLE} Chart Save",
                Filter = $"bremen files (*{EDITOR_EXTENSION})|*{EDITOR_EXTENSION}",
                FilterIndex = 1,
            };
            _songLoadDialog = new VistaOpenFileDialog {
                Title = $"{EDITOR_TITLE} Song Load",
                Filter = "mp3 files (*.mp3)|*.mp3|ogg files (*.ogg)|*.ogg",
                FilterIndex = 1,
            };

            _tileEditor.OnTileUpdated += OnTileUpdated;
        }

        private void Start() {
            ResetChart();
        }
        private void OnTileUpdated() {
            Chart.notes = _tileEditor.ToNotes();
        }

        public bool PlayChart() {
            if (OpenFilePath is null) {
                return false;
            }

            foreach (var ui in _editorUis) {
                ui.SetActive(false);
            }

            _chartPlayer.gameObject.SetActive(true);
            
            _chartPlayer.LoadChart(Chart, SongClip);

            float time = 0.0f;
            if (_tileEditor.CurrentTile != null) {
                var index = _tileEditor.CurrentTile.Index;

                time = Chart.GetSecondsFromTileIndex(index);
            }

            _chartPlayer.Play(time, true);

            return true;
        }
        public bool StopChart() {
            foreach (var ui in _editorUis) {
                ui.SetActive(true);
            }

            _chartPlayer.gameObject.SetActive(false);

            _chartPlayer.Stop();

            return true;
        }

        public bool ResetChart() {
            Chart = new();
            Data.WorkSpacePath = default;
            OpenFilePath = default;
            Dirty = false;

            SongClip = null;
            SongTime = 0.0f;

            if (!File.Exists(Data.LastOpenedFilePath)) {
                Data.LastOpenedFilePath = default;
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
            Chart.songFilename = Path.GetFileName(path);
            Dirty = true;

            OnSongLoaded?.Invoke(path, SongClip);

            return true;
        }

        public bool Save() {
            if (Data.IsExistWorkSpace) {
                return SaveAs(Data.LastOpenedFilePath);
            }
            
            return SaveWithDialog();
        }
        public bool SaveAs(string path) {
            var json = JsonUtility.ToJson(Chart, true);
            
            File.WriteAllText(path, json);

            Data.WorkSpacePath = Path.GetDirectoryName(path);
            Data.LastOpenedFilePath = path;

            OpenFilePath = path;

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

            _tileEditor.FromNotes(Chart.notes);

            Data.WorkSpacePath = Path.GetDirectoryName(path);
            Data.LastOpenedFilePath = path;
            OpenFilePath = path;

            var song_path = Path.Combine(Data.WorkSpacePath, Chart.songFilename);
            LoadSong(song_path);

            Dirty = false;

            return true;
        }
        public bool LoadLastOpened() => Load(Data.LastOpenedFilePath);

        // With Dialog Functions
        public bool LoadSongWithDialog() {
            if (_songLoadDialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _songLoadDialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            var song_filename = Path.GetFileName(_songLoadDialog.FileName);
            var path = Path.Combine(Data.WorkSpacePath, song_filename);

            try {
                if (!File.Exists(path)) {
                    File.Copy(_songLoadDialog.FileName, path);
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
            if (_chartSaveDialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var path = Path.ChangeExtension(_chartSaveDialog.FileName, EDITOR_EXTENSION);

            return SaveAs(path);
        }
        public bool LoadWithDialog() {
            if (_chartLoadDialog.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var stream = _chartLoadDialog.OpenFile();
            if (stream is null) {
                return false;
            }

            stream.Close();

            return Load(_chartLoadDialog.FileName);
        }

        private void OnEnable() {
            if (_chartPlayer == null) {
                _chartPlayer = GameObject.FindAnyObjectByType<BremenChartPlayer>();
            }
        }
        private void OnDestroy() {
            var json = JsonUtility.ToJson(Data, true);
            
            File.WriteAllText(DataPath, json);
        }
    }
}