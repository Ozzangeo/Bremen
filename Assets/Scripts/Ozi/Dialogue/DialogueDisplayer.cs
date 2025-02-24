using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ozi.Dialogue {
    public class DialogueDisplayer : MonoBehaviour {
        public const KeyCode INTERACTION_KEY = KeyCode.E;

        [Header("Requires")]
        [SerializeField] private Text _nameText;
        [SerializeField] private Image _profileImage;
        [SerializeField] private Text _speechText;

        [Header("Debugs")]
        [SerializeField] private DialogueDataObject _init;
        [SerializeField] private bool _isSkip = false;

        private IEnumerator _dialogueCoroutine;

        private void Awake() {
            // Hide();

            Show();

            Play(_init);
        }

        public void Play(DialogueDataObject dialogue) {
            Stop();

            _dialogueCoroutine = PlayDialogue(dialogue); 

            StartCoroutine(_dialogueCoroutine);
        }
        public void Stop() {
            if (_dialogueCoroutine is not null) {
                StopCoroutine(_dialogueCoroutine);
            }

            _isSkip = false;
        }

        public void Show() {
            gameObject.SetActive(true);
        }
        public void Hide() {
            gameObject.SetActive(false);
        }

        private IEnumerator PlayDialogue(DialogueDataObject dialogue) {
            if (dialogue == null) {
                yield break;
            }

            int speech_index = 0;
            while (speech_index < dialogue.Speechs.Count) {
                var speech = dialogue.Speechs[speech_index];
                var profile = dialogue.Profiles[speech.ProfileIndex].Data;

                _nameText.text = profile.Name;
                _profileImage.sprite = profile.Profile;
                _speechText.text = "";

                var seconds_per_word = 1.0f / profile.TalkSpeed;

                string foward_sentence = "";
                int sentence_index = 0;
                while (sentence_index < speech.Sentences.Count) {
                    var sentence = speech.Sentences[sentence_index];

                    yield return PlaySentence(sentence, seconds_per_word, foward_sentence);

                    foward_sentence = _speechText.text;

                    sentence_index++;

                    if (_isSkip) {
                        _speechText.text = speech.SetenceAllText;

                        _isSkip = false;

                        yield return null;

                        break;
                    }
                }

                while (!Input.GetKeyDown(INTERACTION_KEY)) {
                    yield return null;
                }

                yield return null;

                speech_index++;
            }
            
            yield return null;
        }
        private IEnumerator PlaySentence(DialogueSentence sentence, float seconds_per_word, string forward_sentence) {
            float time = 0.0f;

            var text_length = sentence.Text.Length;
            for (int i = 0; i < text_length && !_isSkip; i++) {
                while (time < seconds_per_word) {
                    time += Time.deltaTime;

                    if (Input.GetKeyDown(INTERACTION_KEY)) {
                        _isSkip = true;

                        yield return null;

                        break;
                    }

                    yield return null;
                }
                time -= seconds_per_word;

                _speechText.text = $"{forward_sentence}{sentence.Text[..i]}";
            }

            _speechText.text = $"{forward_sentence}{sentence.Text}";

            if (!_isSkip) {
                time = sentence.Delay;
                while(time > 0.0f) {
                    time -= Time.deltaTime;
                    
                    if (Input.GetKeyDown(INTERACTION_KEY)) {
                        _isSkip = true;

                        yield return null;

                        break;
                    }

                    yield return null;
                }
            }
        }
    }
}
