using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class ShortcutableUI : MonoBehaviour {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public KeyCode[] SubShortcutKeys { get; private set; }
        [field: SerializeField] public KeyCode ShortcutKey { get; private set; }
        
        public bool IsShortcut {
            get {
                foreach (var sub_key in SubShortcutKeys) {
                    if (!Input.GetKey(sub_key)) {
                        return false;
                    }
                }

                return Input.GetKeyDown(ShortcutKey);
            }
        }

        private void Update() {
            TryExecute();
        }

        public void Execute() => Button.onClick.Invoke();

        public void TryExecute() {
            if (IsShortcut) {
                Execute();
            }
        }
    }
}