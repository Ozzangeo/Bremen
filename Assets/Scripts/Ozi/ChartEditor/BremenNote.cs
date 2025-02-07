using System;

namespace Ozi.ChartEditor {
    public enum BremenNoteDirection {
        Left,
        Right,
    }

    [System.Serializable]
    public struct BremenNote : IComparable<BremenNote> {
        public float timing;
        public BremenNoteDirection direction;

        public readonly int CompareTo(BremenNote other) => timing.CompareTo(other.timing);
    }
}