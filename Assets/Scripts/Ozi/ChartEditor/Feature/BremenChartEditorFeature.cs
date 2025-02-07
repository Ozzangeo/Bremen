using UnityEngine;

namespace Ozi.ChartEditor.Feature {
    public class BremenChartEditorFeature : MonoBehaviour {
        [SerializeField] protected BremenChartEditor _editor;

        protected virtual void OnEnable() {
            if (_editor == null) {
                _editor = GameObject.FindAnyObjectByType<BremenChartEditor>();
            }
        }
    }
}