using UnityEngine;
using UnityEngine.UI;

namespace Ozi.ChartPlayer {
    public class BremenNote : MonoBehaviour {
        [field: Header("Requires")]
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public BremenChartAudioPlayer AudioPlayer { get; set; }

        [field: Header("Settings")]
        [field: SerializeField] public float Speed { get; set; } = 5.0f;
        [field: SerializeField] public float Height { get; set; } = 100.0f;
        
        [field: Header("Debugs")]
        [field: SerializeField] public float Timing { get; set; }
        [field: SerializeField] public float VisualizeTiming { get; set; }

        public RectTransform RectTransform => transform as RectTransform;
        public float Progress => (Timing - AudioPlayer.RealTime) / VisualizeTiming;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _distance;

        private void Awake() {
            var start_position = Vector3.zero;
            start_position.x = -(RectTransform.rect.width * 0.5f);

            var end_position = Vector3.zero;
            end_position.x = (Screen.width * 0.5f);

            _startPosition = start_position;
            _endPosition = end_position;

            _distance = _endPosition - _startPosition;
        }

        private void Update() {
            var position = _endPosition - (Progress * Speed * _distance);
            position.y = Height;

            RectTransform.position = position;
        }
    }
}