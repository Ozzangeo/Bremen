using UnityEngine;

namespace Ozi.ChartEditor {
    [System.Serializable]
    public class BremenChartEditorData {
        [field: SerializeField] public string LastOpenedFilePath { get; set; } = string.Empty;
        [field: SerializeField] public string WorkSpacePath { get; set; } = string.Empty;

        public bool IsExistWorkSpace => WorkSpacePath is not null;
    }
}