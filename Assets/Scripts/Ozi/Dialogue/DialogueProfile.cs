using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueProfile {
        [field: SerializeField] public Sprite Profile { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public float TalkSpeed { get; private set; }
        [field: SerializeField] public AudioClip TalkAudio { get; private set; }
    }
}