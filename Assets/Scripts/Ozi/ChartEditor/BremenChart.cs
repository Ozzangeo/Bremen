using System.Collections.Generic;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public struct BremenChart {
        public string song_filename;
            
        public float bpm;
        public float offset;
        public float volume;

        public List<BremenNote> notes;

        public readonly void Sort() {
            notes.Sort();
        }

        public static BremenChart Generate() {
            return new BremenChart() {
                bpm = 100.0f,
                offset = 0.0f,
                volume = 1.0f
            };
        }
    }
}