using System.Collections.Generic;
using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueSpeech {
        [field: SerializeField] public int ProfileIndex { get; private set; }
        [field: SerializeField] public List<DialogueSentence> Sentences { get; private set; } = new();
    }
}