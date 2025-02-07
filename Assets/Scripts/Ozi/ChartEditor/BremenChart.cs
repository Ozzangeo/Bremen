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
    }
}