using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Weapon.Utility {
    public class ScreenBlinder : MonoBehaviour {
        private static ScreenBlinder Instance {
            get {
                if (!_isAllocated
                    && _instance == null) {
                    var prefab = Resources.Load<ScreenBlinder>("Utility/Screen Blinder");

                    _instance = GameObject.Instantiate(prefab);

                    DontDestroyOnLoad(_instance.gameObject);

                    _isAllocated = true;
                }

                return _instance;
            }
        }

        private static ScreenBlinder _instance;
        private static bool _isAllocated = false;

        [SerializeField] private Image _image;

        private void Awake() {
            if (_isAllocated) {
                Destroy(gameObject);

                return;
            }

            _instance = this;

            _isAllocated = true;
        }

        public static void Show(float alpha) {
            var color = Instance._image.color;
            color.a = alpha;

            Instance._image.color = color;
        }
    }
}
