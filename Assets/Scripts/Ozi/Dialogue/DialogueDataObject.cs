using System.Collections.Generic;
using UnityEngine;

namespace Ozi.Dialogue {
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
    public class DialogueDataObject : ScriptableObject {
        [field: SerializeField] public List<DialogueProfileObject> Profiles { get; private set; } = new();
        [field: SerializeField] public List<DialogueSpeech> Speechs { get; private set; } = new();
    }
}