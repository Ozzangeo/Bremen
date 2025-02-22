using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class StaticRotatableButton : MonoBehaviour {
        [field: SerializeField] public Text Text { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }

        public float Angle => Button.transform.localEulerAngles.z;

        public void Rotate(float angle) {
            var eular_angle = Button.transform.localEulerAngles;

            eular_angle.z = angle;

            Button.transform.localEulerAngles = eular_angle;
        }
    }
}