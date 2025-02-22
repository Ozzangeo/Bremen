using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public class BremenChart {
        public string songFilename = string.Empty;
        public float bpm = 100.0f;
        public int offset = 0;
        public int volume = 100;
        public int pitch = 100;
        
        public List<float> angles = new();

        public float SecondsPerBeat => 60.0f / bpm;

        public void Sort() {
            angles.Sort();
        }

        public List<float> ToTimings(float offset_seconds = 0.0f) {
            var timings = new List<float>();

            float seconds_per_beat = SecondsPerBeat;
            float total_timing = offset * 0.001f + offset_seconds;
            foreach (var angle in angles) {
                var timing = (angle / 180.0f) * seconds_per_beat;

                total_timing += timing;
                
                timings.Add(total_timing);
            }

            return timings;
        }
        public float GetSecondsFromTileIndex(int index) {
            if (index <= 0) {
                return 0.0f;
            }

            var min = Mathf.Min(index, angles.Count);

            float seconds_per_beat = SecondsPerBeat;
            float total_timing = offset * 0.001f;
            for (int i = 0; i < min; i++) {
                var angle = angles[i];
                var timing = (angle / 180.0f) * seconds_per_beat;

                total_timing += timing;
            }

            return total_timing;
        }
    }
}