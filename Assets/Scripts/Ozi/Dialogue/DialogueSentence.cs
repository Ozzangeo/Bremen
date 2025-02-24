using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueSentence {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public float Delay { get; private set; } = 0.0f;
        [field: SerializeField] public float TalkSpeedRate { get; private set; } = 1.0f;
    }
}