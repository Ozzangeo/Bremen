using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public class BremenChart {
        [field: SerializeField] public string SongFilename { get; set; } = string.Empty;

        [field: SerializeField] public float BPM { get; set; } = 100.0f;
        [field: SerializeField] public int Offset { get; set; } = 0;
        [field: SerializeField] public int Volume { get; set; } = 100;
        [field: SerializeField] public int Pitch { get; set; } = 100;

        [field: SerializeField] public List<BremenNote> Notes { get; set; } = new();

        public void Sort() {
            Notes.Sort();
        }
    }
}