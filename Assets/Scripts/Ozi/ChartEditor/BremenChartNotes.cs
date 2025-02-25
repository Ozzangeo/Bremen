using Ozi.ChartEditor.Tile;
using System.Collections.Generic;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public class BremenChartNotes {
        public List<float> angles = new();

        public List<BremenTileEvent> events = new();

        public bool TryGetEvent(int index, out BremenTileEvent out_event) {
            out_event = null;

            foreach (var @event in events) {
                if (@event.index == index) {
                    out_event = @event;

                    return true;
                }

                if (@event.index > index) {
                    return false;
                }
            }

            return false;
        }
    }
}