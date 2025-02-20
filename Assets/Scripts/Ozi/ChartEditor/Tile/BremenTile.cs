using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartEditor.Tile {
    public class BremenTile : MonoBehaviour {
        [field: SerializeField] public BremenTile Previous { get; private set; }
        [field: SerializeField] public BremenTile Next { get; private set; }
        [field: SerializeField] public int Index { get; private set; }

        private float WhenRelativeAngleNotNull {
            get {
                var eular_angle = transform.localEulerAngles.z;
                var next_angle = Next.transform.localEulerAngles.z;

                return (eular_angle - next_angle) + 180.0f;
            }
        }
        public float RelativeAngle {
            get {
                if (Next != null) {
                    return WhenRelativeAngleNotNull;
                }

                var eular_angle = transform.localEulerAngles.z;

                return (eular_angle) + 180.0f;
            }
            set {
                var eular_angle = transform.localEulerAngles;
                eular_angle.z = value - 180.0f;

                transform.localEulerAngles = eular_angle;
            }
        }
        public float RelativeAngleAtPrevious {
            get {
                var angle = transform.localEulerAngles.z;
                var previous_angle = Previous.transform.localEulerAngles.z;

                return previous_angle - angle;
            }
            set {
                var previous_angle = Previous.transform.localEulerAngles.z;

                var angle = transform.localEulerAngles;
                angle.z = previous_angle - (value + 180);

                transform.localEulerAngles = angle;
            }
        }
        public float Angle {
            get => transform.localEulerAngles.z + 180.0f;
            set {
                var angle = transform.localEulerAngles;

                angle.z = value - 180.0f;

                transform.localEulerAngles = angle;
            }
        }

        public float OppositeAngle {
            get => transform.localEulerAngles.z;
        }

        public Vector3 Position => transform.position;

        private Vector3 GetAnglePoint(Vector3 position, float angle_offset = 0.0f) {
            var half_length = transform.localScale.x * 0.5f;

            var angle = transform.localEulerAngles.z + angle_offset;
            var radian = angle * Mathf.Deg2Rad;

            var direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));

            return position + direction * half_length;
        }

        public Vector3 StartPoint {
            get => GetAnglePoint(transform.position, -180.0f);
            set => transform.position = GetAnglePoint(value);
        }
        public Vector3 EndPoint {
            get => GetAnglePoint(transform.position);
            set => transform.position = GetAnglePoint(value);
        }

        public bool Delete() {
            if (Index <= 0) {
                return false;
            }

            Destroy(gameObject);

            if (Previous != null) {
                Previous.Next = Next;
            }
            if (Next != null) {
                Next.Previous = Previous;

                Next.ChainNextDecreseIndex();
            }

            return true;
        }

        private BremenTile CloneTile(Transform parent = null) {
            var clone_tile = Instantiate(this, parent);

            clone_tile.name = "Bremen Tile";

            return clone_tile;
        }
        private static void InsertBackTile(BremenTile target, BremenTile tile) {
            target.Previous = tile;
            target.Next = tile.Next;
            if (tile.Next != null) {
                tile.Next.Previous = target;
            }
            tile.Next = target;

            // Previous is not allocated. because Previous is front tile.
            // This logic like be push_back in linked list structures.
        }

        public BremenTile InsertBack(float angle, Transform parent = null) {
            if (Mathf.Abs(OppositeAngle - angle) <= float.Epsilon) {
                var is_deleted = Delete();
                if (!is_deleted) {
                    return this;
                }

                return Previous;
            }

            var clone_tile = CloneTile(parent);

            InsertBackTile(clone_tile, this);

            clone_tile.Index = Index;
            clone_tile.Angle = angle;
            clone_tile.StartPoint = EndPoint;

            clone_tile.ChainNextIncreseIndex();

            return clone_tile;
        }

        public bool ToAngles(out List<float> angles) {
            angles = new List<float>();
            
            try {
                ChainInsertAngle(ref angles);
            }
            catch (Exception e) {
                Debug.LogException(e);

                return false;
            }

            return true;
        }
        public bool FromAngles(List<float> angles, Transform parent = null) {
            try {
                ChainDestroyTile();

                BremenTile previous = this;
                for (int i = 0; i < angles.Count; i++) {
                    var angle = angles[i];

                    var tile = CloneTile(parent);

                    tile.Previous = previous;
                    previous.Next = tile;

                    tile.Index = i + 1;
                    tile.RelativeAngleAtPrevious = angle;

                    tile.StartPoint = previous.EndPoint;

                    previous = tile;
                }

                // last tile
                previous.Next = null;
            }
            catch (Exception e) {
                Debug.LogException(e);

                return false;
            }
            
            return true;
        }

        private void ChainNextIncreseIndex() {
            Index++;

            if (Next != null) {
                Next.StartPoint = EndPoint;

                Next.ChainNextIncreseIndex();
            }
        }
        private void ChainNextDecreseIndex() {
            Index--;

            if (Next != null) {
                Next.StartPoint = EndPoint;

                Next.ChainNextDecreseIndex();
            }
        }
        
        private void ChainDestroyTile() {
            if (Next != null) {
                Destroy(Next.gameObject);

                Next.ChainDestroyTile();
            }
        }

        private void ChainInsertAngle(ref List<float> angles) {
            if (Next != null) {
                var angle = WhenRelativeAngleNotNull;

                angles.Add(angle);

                Next.ChainInsertAngle(ref angles);
            }
        }
    }
}