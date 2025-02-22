using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozi.ChartPlayer {
    [System.Serializable]
    public class BremenNotePool {
        public IObjectPool<BremenNote> Pool { get; private set; }

        [field: Header("Settings")]
        [field: SerializeField] public Transform Parent { get; private set; }
        [field: SerializeField] public BremenNote Prefab { get; private set; }
        [field: SerializeField] public BremenChartAudioPlayer AudioPlayer { get; set; }

        [field: Header("Debugs")]
        [field: SerializeField] public List<BremenNote> Using { get; private set; } = new();
        [field: SerializeField] public int UsingIndex { get; private set; } = 0;

        public BremenNotePool() {
            Pool = new ObjectPool<BremenNote>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);
        }

        public BremenNote At(int index) => Using[index];
        public void ReleaseSome(int limit) {
            for (int i = UsingIndex; i < limit; i++) {
                var using_note = Using[i];

                Pool.Release(using_note);

                UsingIndex++;
            }
        }
        public void Clear() {
            ReleaseSome(Using.Count);

            Using.Clear();
        }

        private BremenNote OnCreateObject() => GameObject.Instantiate(Prefab, Parent);
        private void OnGetObject(BremenNote note) {
            note.gameObject.SetActive(true);

            note.Image.color = Color.white;
            note.AudioPlayer = AudioPlayer;
        }
        private void OnReleaseObject(BremenNote note) {
            note.gameObject.SetActive(false);
        }
        private void OnDestroyObject(BremenNote note) {
            GameObject.Destroy(note.gameObject);
        }
    }
}