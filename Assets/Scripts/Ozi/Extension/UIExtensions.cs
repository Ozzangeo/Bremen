using UnityEngine.EventSystems;

namespace Ozi.Extension {
    public static class UIExtensions {
        public static void Enable(this UIBehaviour ui_behaviour) => ui_behaviour.gameObject.SetActive(true);
        public static void Disable(this UIBehaviour ui_behaviour) => ui_behaviour.gameObject.SetActive(false);
    }
}