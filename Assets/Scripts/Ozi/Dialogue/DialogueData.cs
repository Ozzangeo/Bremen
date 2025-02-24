using System.Collections.Generic;
using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueData {
        [field: SerializeField] public List<DialogueProfile> Profiles { get; private set; } = new();
        [field: SerializeField] public List<DialogueSpeech> Speechs { get; private set; } = new();
    }
}