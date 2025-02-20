using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class SequenceButton : MonoBehaviour {
        [field: Header("Debug")]
        [field: SerializeField] public Button[] ButtonUis { get; private set; }
        [field: SerializeField] public int CurrentIndex { get; private set; }

        public int NextIndex => ButtonUis.NextIndex(CurrentIndex);

        private void Awake() {
            ButtonUis = transform.GetComponentsInChildren<Button>(false);

            foreach (var button in ButtonUis) {
                button.Disable();
            }

            if (ButtonUis.IsVaildIndex(CurrentIndex)) {
                ButtonUis[CurrentIndex].Enable();
            }
        }

        [ContextMenu("Next")]
        public void Next() {
            if (ButtonUis.IsVaildIndex(CurrentIndex)) {
                ButtonUis[CurrentIndex].Disable();
            }

            var next = NextIndex;

            ButtonUis[next].Enable();

            CurrentIndex = next;
        }
    }
}