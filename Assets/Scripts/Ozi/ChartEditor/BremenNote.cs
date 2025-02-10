using System;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public struct BremenNote : IComparable<BremenNote> {
        public float timing;

        public readonly int CompareTo(BremenNote other) => timing.CompareTo(other.timing);
    }
}