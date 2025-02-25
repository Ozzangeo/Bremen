using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ozi.ChartEditor.Tile {
    public class BremenTileEditor : MonoBehaviour {
        private const float CAMERA_MOVE_SPEED = 10.0f;
        private const float CAMERA_SCROLL_SPEED = 50.0f;

        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public BremenTile StartTile { get; private set; }
        [field: SerializeField] public BremenTile CurrentTile { get; private set; }
        [field: SerializeField] public BremenTile NextTile { get; private set; }

        [field: Header("Debug")]
        [field: SerializeField] public Vector3 PreviousInput { get; private set; }
        [field: SerializeField] public Vector3 PreviousCameraPosition { get; private set; }
        [field: SerializeField] public Vector3 PreviousInputToWorld { get; private set; }
        [field: SerializeField] public Ray ClickTileRay { get; private set; }
        [field: SerializeField] public Vector3? CameraMoveTargetPosition { get; private set; }

        public Dictionary<KeyCode, float> TileShortKey { get; private set; }

        public Vector3 InputOrigin {
            get {
                var input = Input.mousePosition;
                
                input.z = -Camera.transform.position.z;

                return input;
            }
        }

        public event Action<BremenTile, bool> OnCurrentTileUpdate; // current: BremenTile, is_current_null: bool
        public event Action<float> OnZoomCamera;    // zoom_rate: float
        public event Action OnTileUpdated;

        public void InsertBackTile(float angle) {
            var tile = CurrentTile.InsertBack(angle, transform);
            
            SelectTile(tile);

            OnTileUpdated?.Invoke();
        }

        private void Awake() {
            if (Camera == null) {
                Camera = Camera.main;
            }

            TileShortKey = new() {
                { KeyCode.Q, 315 },
                { KeyCode.W, 270 },
                { KeyCode.E, 225 },
                { KeyCode.D, 180 },
                { KeyCode.C, 135 },
                { KeyCode.X,  90 },
                { KeyCode.Z,  45 },
                { KeyCode.A, 360 },
            };

            SelectTile(StartTile);
        }

        private void Update() {
            var input = Input.mousePosition;
            var scroll = Input.mouseScrollDelta;

            ZoomCamera(scroll.y);
            MoveCamera(input);

            RaycastTile();

            ControlTile();

            if (CameraMoveTargetPosition is Vector3 position) {
                var move_tick = CAMERA_MOVE_SPEED * Time.deltaTime;

                var move = Vector3.Lerp(Camera.transform.position, position, move_tick);
                move.z = Camera.transform.position.z;

                Camera.transform.position = move;
            }
        }

        private bool IsMouseMoved() => Vector3.Distance(PreviousInput, Input.mousePosition) >= 1.0f;

        private void ZoomCamera(float sensitivity) {
            if (Mathf.Abs(sensitivity) > 0.0f) {
                var position = Camera.transform.position;

                float zoom = sensitivity * Time.deltaTime * CAMERA_SCROLL_SPEED;

                position.z = Mathf.Clamp(position.z + zoom, -100.0f, -0.1f);

                Camera.transform.position = position;

                var zoom_rate = 7.0f / Mathf.Abs(position.z);   // (Default Depth) / (Current Depth) 
                OnZoomCamera?.Invoke(zoom_rate);
            }
        }
        private void MoveCamera(Vector2 input) {
            if (Input.GetMouseButtonDown(0)) {
                PreviousCameraPosition = Camera.transform.position;

                PreviousInput = input;
                PreviousInputToWorld = Camera.ScreenToWorldPoint(InputOrigin);
            }
            else if (Input.GetMouseButton(0) && IsMouseMoved()) {
                var input_to_world = Camera.ScreenToWorldPoint(InputOrigin);

                var move_position = PreviousInputToWorld - input_to_world;
                CameraMoveTargetPosition = PreviousCameraPosition + move_position;
            }
        }

        private void SelectTile(BremenTile tile) {
            if (CurrentTile != null) {
                CurrentTile.Unselect();
            }

            CurrentTile = tile;
            
            if (tile != null) {
                CurrentTile.Select();

                CameraMoveTargetPosition = CurrentTile.Position;
            }
        }
        private void ControlTile() {
            if (CurrentTile == null) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (CurrentTile.Next != null) {
                    SelectTile(CurrentTile.Next);
                }
            } 
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (CurrentTile.Previous != null) {
                    SelectTile(CurrentTile.Previous);
                }
            }

            foreach (var short_key in TileShortKey) {
                if (Input.GetKeyDown(short_key.Key)) {
                    InsertBackTile(short_key.Value);
                }
            }
        }
        private void RaycastTile() {
            if (!Input.GetMouseButtonUp(0) || IsMouseMoved()) {
                return;
            }

            // Is raycast UI
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            var distance = PreviousInputToWorld - PreviousCameraPosition;
            var direction = distance.normalized;

            ClickTileRay = new Ray(PreviousCameraPosition, direction);

            bool is_raycasted = Physics.Raycast(ClickTileRay, out var hit);
            BremenTile raycasted_tile = null;
            if (is_raycasted) {
                if (hit.collider.TryGetComponent(out raycasted_tile)) {
                    // Debug anything
                }
            }

            SelectTile(raycasted_tile);

            OnCurrentTileUpdate?.Invoke(CurrentTile, !is_raycasted);
        }

        public BremenChartNotes ToNotes() {
            // Must be StartTile is not null in logic
            if (StartTile.ToNotes(out var notes)) {
                // Succeed
            } else {
                // Failed
            }

            return notes;
        }
        public void FromNotes(BremenChartNotes notes) {
            if (StartTile.FromNotes(notes, transform)) {
                // Succeed
            } else {
                // Failed
            }

            CurrentTile = StartTile;
        }

        private void OnDrawGizmos() {
            var before = Gizmos.color;
            Gizmos.color = Color.red;

            Gizmos.DrawRay(ClickTileRay.origin, ClickTileRay.direction);

            Gizmos.color = before;
        }
    }
}