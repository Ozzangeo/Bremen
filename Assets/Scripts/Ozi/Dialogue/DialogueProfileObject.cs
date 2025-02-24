using UnityEngine;

namespace Ozi.Dialogue {
    [CreateAssetMenu(fileName = "DialogueProfile", menuName = "Dialogue/DialogueProfile")]
    public class DialogueProfileObject : ScriptableObject {
        [field: SerializeField] public DialogueProfile Data { get; private set; }
    }
}