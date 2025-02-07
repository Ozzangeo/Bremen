using System.IO;
using UnityEngine;

namespace Ozi.Chart {
    public class BremenChartEditor : MonoBehaviour {
        [SerializeField] private BremenChartEditorData _data;

        [SerializeField] private BremenChart _chart;

        public BremenChartEditorData Data => _data;

        private void Awake() {
            
        }

        public void Save() {
            
        }

        public bool Load() => LoadAs(_data.last_open_file_path);
        public bool Load(out string exception) => LoadAs(_data.last_open_file_path, out exception);
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

            return true;
        }
    }
}