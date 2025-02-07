using UnityEngine;

namespace Ozi.Chart {
    public class BremenChartEditorUI : MonoBehaviour {
        [SerializeField] private BremenChartEditor _editor;

        private void OnEnable() {
            if (_editor == null) {
                _editor = GameObject.FindAnyObjectByType<BremenChartEditor>();
            }
        }
    }
}