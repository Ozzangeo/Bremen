using Ozi.Extension.Component;
using System.Globalization;
using UnityEngine;

namespace Ozi.ChartEditor.Tile {
    public class BremenTileEditorUI : MonoBehaviour {
        private const float BUTTON_DISTANCE = 2.0f;
        private const float BUTTON_SCALE = 0.75f;

        [SerializeField] private BremenTileEditor _editor;

        [Header("Requires")]
        [SerializeField] private StaticRotatableButton _buttonPrefab;
        [SerializeField] private GameObject _keyGuideParent;

        [Header("Debugs")]
        [SerializeField] private StaticRotatableButton[] _buttons;

        private void Awake() {
            if (_editor == null) {
                _editor = GameObject.FindAnyObjectByType<BremenTileEditor>();
            }
        }

        private void Start() {
            _buttons = new StaticRotatableButton[_editor.TileShortKey.Count];

            int i = 0;
            foreach (var short_key in _editor.TileShortKey) {
                var prefab = Instantiate(_buttonPrefab, _keyGuideParent.transform);
                prefab.Text.text = $"{short_key.Key}";
                prefab.Rotate(short_key.Value);

                prefab.Button.onClick.AddListener(
                    () => {
                        if (_editor.CurrentTile != null) {
                            _editor.InsertBackTile(short_key.Value);
                        }
                    });

                var radian = -short_key.Value * Mathf.Deg2Rad;

                var direction = new Vector3(-Mathf.Cos(radian), Mathf.Sin(radian));
                var world_position = direction * BUTTON_DISTANCE;
                world_position.z = -_editor.Camera.transform.position.z;

                var screen_position = _editor.Camera.WorldToScreenPoint(world_position);
                screen_position.x -= Screen.width * 0.5f;
                screen_position.y -= Screen.height * 0.5f;

                prefab.transform.localPosition = screen_position;
                prefab.transform.localScale = Vector3.one * BUTTON_SCALE;
                
                _buttons[i++] = prefab;
            }

            _editor.OnCurrentTileUpdate += OnCurrentTileUpdate;
            _editor.OnZoomCamera += OnZoomCamera;
        }

        private void Update() {
            if (_editor.CurrentTile == null) {
                return;
            }

            var screen_position = _editor.Camera.WorldToScreenPoint(_editor.CurrentTile.transform.position);

            _keyGuideParent.transform.position = screen_position;
        }

        private void OnCurrentTileUpdate(BremenTile tile, bool is_tile_null) {
            _keyGuideParent.SetActive(!is_tile_null);
        }
        private void OnZoomCamera(float zoom_rate) {
            _keyGuideParent.transform.localScale = Vector3.one * zoom_rate;
        }
    }
}