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

        public BremenChartNotes notes = new();

        public float SecondsPerBeat => 60.0f / bpm;

        public float ToTimings(out List<float> timings, int length = -1, float offset_seconds = 0.0f) {
            timings = new List<float>();

            if (length < 0) {
                length = notes.angles.Count;
            }

            length = Mathf.Min(length, notes.angles.Count);

            float seconds_per_beat = SecondsPerBeat;
            float total_timing = offset * 0.001f + offset_seconds;
            
            float speed = 1.0f;
            bool is_twirl = false;

            for (int i = 0; i < length; i++) {
                var is_event_allocated = notes.TryGetEvent(i, out var @event);


                float angle = notes.angles[i];
                if (is_twirl) {
                    angle = 360.0f - angle;
                }

                var timing = (angle / 180.0f) * seconds_per_beat / speed;

                total_timing += timing;
                
                timings.Add(total_timing);

                if (is_event_allocated) {
                    speed *= @event.speedRate;
                }
                if (is_event_allocated) {
                    if (@event.isTwirl) {
                        is_twirl = !is_twirl;
                    }
                }
            }

            return total_timing;
        }

        public float GetSecondsFromTileIndex(int index) => ToTimings(out _, index);
    }
}