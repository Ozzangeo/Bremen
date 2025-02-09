using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Extension.Component {
    public class ElementFieldUI : MonoBehaviour {
        [field: Header("Components")]
        [field: SerializeField] public Text TitleText { get; private set; }
        [field: SerializeField] public InputField InputField { get; private set; }
        [field: SerializeField] public Button InteractionButton { get; private set; }
        [field: SerializeField] public Text UnitText { get; private set; }
    }
}