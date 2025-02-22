using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozi.ChartPlayer {
    [System.Serializable]
    public class BremenNotePool {
        public IObjectPool<BremenNote> Pool { get; private set; }

        [field: Header("Settings")]
        [field: SerializeField] public BremenChartAudioPlayer AudioPlayer { get; private set; }
        [field: SerializeField] public BremenNote Prefab { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
        [field: SerializeField] public Color Color { get; set; } = Color.white;

        [field: Header("Debugs")]
        [field: SerializeField] public List<BremenNote> Using { get; private set; } = new();
        [field: SerializeField] public int UsingIndex { get; private set; } = 0;
        [field: SerializeField] public int ReleasedIndex { get; private set; } = 0;

        public BremenNotePool(BremenChartAudioPlayer audio_player, BremenNote prefab, Transform parent = null) {
            AudioPlayer = audio_player;
            Prefab = prefab;
            Parent = parent;

            Pool = new ObjectPool<BremenNote>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);
        }

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

            UsingIndex = 0;
            ReleasedIndex = 0;
        }

        public void Focus(int index) {
            if (index < Using.Count) {
                Using[index].Image.color = Color.green;
            }
        }
        public BremenNote Generate(float timing, float visualize_timing) {
            var note = Pool.Get();
            Debug.Log($"Generate: {timing}");

            note.Timing = timing;
            note.VisualizeTiming = visualize_timing;

            Using.Add(note);

            return note;
        }

        private BremenNote OnCreateObject() => GameObject.Instantiate(Prefab, Parent);
        private void OnGetObject(BremenNote note) {
            note.gameObject.SetActive(true);

            note.Image.color = Color;
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