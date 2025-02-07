namespace Ozi.ChartEditor {
    [System.Serializable]
    public struct BremenChartEditorData {
        public string last_open_file_path;
        public string work_space_path;

        public readonly bool IsExistWorkSpace => work_space_path is not null;
    }
}