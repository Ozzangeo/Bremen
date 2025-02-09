using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class SequenceSelectableUI : MonoBehaviour {
        [field: Header("Debug")]
        [field: SerializeField] public Selectable[] SelectableUIs { get; private set; }
        [field: SerializeField] public int CurrentIndex { get; private set; }

        public int NextIndex => SelectableUIs.NextIndex(CurrentIndex);

        private void Awake() {
            SelectableUIs = transform.GetComponentsInChildren<Selectable>(false);

            foreach (var ui in SelectableUIs) {
                ui.Disable();
            }

            if (SelectableUIs.IsVaildIndex(CurrentIndex)) {
                SelectableUIs[CurrentIndex].Enable();
            }
        }

        [ContextMenu("Next")]
        public void Next() {
            if (SelectableUIs.IsVaildIndex(CurrentIndex)) {
                SelectableUIs[CurrentIndex].Disable();
            }

            var next = NextIndex;

            SelectableUIs[next].Enable();

            CurrentIndex = next;
        }
    }
}