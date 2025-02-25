using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Ozi.Weapon.Utility {
    public class ScreenBlinder : MonoBehaviour {
        public const float SHOW_TIME = 1.0f;

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

        [SerializeField] private Volume _volume;
        private IEnumerator _show;

        private void Awake() {
            if (_isAllocated) {
                Destroy(gameObject);

                return;
            }

            _instance = this;

            _isAllocated = true;
        }

        public static void Show(float alpha) {
            var instance = Instance;

            if (instance._show is not null) {
                instance.StopCoroutine(instance._show);
            }

            var profile = instance._volume.profile;
            if (!profile.TryGet(out Vignette vignette)) {
                return;
            }

            instance._show = Show(vignette.intensity.value, alpha, SHOW_TIME, o => vignette.intensity.value = o);
            instance.StartCoroutine(instance._show);
        }

        private static IEnumerator Show(float start, float end, float time, Action<float> setter) {
            float cur_time = 0.0f;

            while (cur_time < time) {
                float t = cur_time / time;

                var lerp = Mathf.Lerp(start, end, t);
                setter?.Invoke(lerp);

                cur_time += Time.deltaTime;

                yield return null;
            }

            setter?.Invoke(end);
        }
    }
}
