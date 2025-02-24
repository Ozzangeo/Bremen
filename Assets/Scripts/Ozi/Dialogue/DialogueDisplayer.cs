using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Dialogue {
    public class DialogueDisplayer : MonoBehaviour {
        public const KeyCode NEXT_KEY = KeyCode.E;

        [Header("Requires")]
        [SerializeField] private Text _nameText;
        [SerializeField] private Image _profileImage;
        [SerializeField] private Text _speechText;

        private IEnumerator _dialogueCoroutine;

        public void Play(DialogueDataObject dialogue) {
            if (_dialogueCoroutine is not null) {
                StopCoroutine(_dialogueCoroutine);
            }

            _dialogueCoroutine = PlayDialogue(dialogue); 

            StartCoroutine(_dialogueCoroutine);
        }

        private IEnumerator PlayDialogue(DialogueDataObject dialogue) {
            int speech_index = 0;
            while (speech_index < dialogue.Speechs.Count) {
                var speech = dialogue.Speechs[speech_index];
                var profile = dialogue.Profiles[speech.ProfileIndex];

                speech_index++;
            }
            
            yield return null;
        }
    }
}
