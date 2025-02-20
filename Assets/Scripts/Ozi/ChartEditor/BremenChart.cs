using System.Collections.Generic;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public class BremenChart {
        public string songFilename = string.Empty;
        public float bpm = 100.0f;
        public int offset = 0;
        public int volume = 100;
        public int pitch = 100;
        
        public List<float> angles = new();

        public void Sort() {
            angles.Sort();
        }

        public List<float> ToTimings() {
            var timings = new List<float>();

            float total_timing = 0.0f;
            foreach (var angle in angles) {
                var timing = total_timing + angle;

                timings.Add(timing);

                total_timing += timing;
            }

            return timings;
        }
    }
}