using System.Collections.Generic;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public struct BremenChart {
        public string song_filename;
            
        public float bpm;
        public int offset;
        public int volume;

        public List<BremenNote> notes;

        public void SetBPM(float bpm) => this.bpm = bpm;
        public void SetOffset(int offset) => this.offset = offset;
        public void SetVolume(int volume) => this.volume = volume;

        public readonly void Sort() {
            notes.Sort();
        }

        public static BremenChart Generate() {
            return new BremenChart() {
                bpm = 100.0f,
                offset = 0,
                volume = 100
            };
        }
    }
}