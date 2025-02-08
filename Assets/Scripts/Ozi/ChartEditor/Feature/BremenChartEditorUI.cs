using UnityEngine;

namespace Ozi.ChartEditor.Feature {
    public class BremenChartEditorUI : BremenChartEditorFeature {
        public void GenerateNewChart() {
            _editor.ResetChart();
        }

        public void LoadSong() {
            if (_editor.Data.IsExistWorkSpace) {
                _editor.LoadSong();
            } else {
                // display logic with exception
            }
        }
        public void PlayChart() {
            _editor.PlayChart();
        }

        public void Save() {
            if (_editor.Data.IsExistWorkSpace) {
                _editor.Save();
            } else {
                _editor.SaveAs();
            }
        }
        public void Load() {
            _editor.LoadAs();
        }
    }
}