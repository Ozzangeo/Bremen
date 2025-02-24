using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueSpeech {
        [field: SerializeField] public int ProfileIndex { get; private set; }
        [field: SerializeField] public List<DialogueSentence> Sentences { get; private set; } = new();

        public string SetenceAllText {
            get {
                var builder = new StringBuilder();

                foreach (var sentence in Sentences) {
                    builder.Append(sentence.Text);
                }

                return builder.ToString();
            }
        }
    }
}