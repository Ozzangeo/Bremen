using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class ShortcutableUI : MonoBehaviour {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Text TitleText { get; private set; }
        [field: SerializeField] public Text ShortcutText { get; private set; }

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

        private void Awake() {
            var shortcut_builder = new StringBuilder("( ");

            foreach (var sub_key in SubShortcutKeys) {
                shortcut_builder.Append($"{sub_key} + ");
            }

            shortcut_builder.Append($"{ShortcutKey} )");
            
            ShortcutText.text = $"{shortcut_builder}";
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