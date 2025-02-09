using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class SequenceSelectableUI : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Selectable[] _selectable_uis;
        [SerializeField] private int _current_index = 0;
        
        public int CurrentIndex => _current_index;
        public int NextIndex => _selectable_uis.NextIndex(CurrentIndex);

        private void Awake() {
            _selectable_uis = transform.GetComponentsInChildren<Selectable>(false);

            foreach (var ui in _selectable_uis) {
                ui.Disable();
            }

            if (_selectable_uis.IsVaildIndex(CurrentIndex)) {
                _selectable_uis[CurrentIndex].Enable();
            }
        }

        [ContextMenu("Next")]
        public void Next() {
            if (_selectable_uis.IsVaildIndex(CurrentIndex)) {
                _selectable_uis[CurrentIndex].Disable();
            }

            var next = NextIndex;

            _selectable_uis[next].Enable();

            _current_index = next;
        }
    }
}