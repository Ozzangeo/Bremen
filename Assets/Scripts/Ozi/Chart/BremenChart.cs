using System.Collections.Generic;

namespace Ozi.Chart {
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