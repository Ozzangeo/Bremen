using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.ChartEditor.Tile {
    public class BremenTile : MonoBehaviour {
        [field: SerializeField] public BremenTile Previous { get; private set; }
        [field: SerializeField] public BremenTile Next { get; private set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        
        public int Index { 
            get => _index; 
            private set {
                _index = value;

                SpriteRenderer.sortingOrder = -_index;
            } 
        }
        [SerializeField] private int _index;

        public float RelativeAngleAtPrevious {
            get {
                var previous_angle = Previous.transform.localEulerAngles.z;
                var angle = transform.localEulerAngles.z;

                return previous_angle - (angle - 180.0f);
            }
            set {
                var previous_angle = Previous.transform.localEulerAngles.z;

                var angle = transform.localEulerAngles;
                angle.z = previous_angle - (value - 180.0f);

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

        public float OppositeAngle => transform.localEulerAngles.z;

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

            //clone_tile.name = "Bremen Tile";
            clone_tile.SpriteRenderer.color = Color.white;

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
            angle %= 360.0f;

            if (Previous != null) {
                if (Mathf.Abs(OppositeAngle - angle) <= float.Epsilon) {
                    if (!Delete()) {
                        return this;
                    }

                    return Previous;
                }
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
                if (Next != null) {
                    Next.ChainInsertAngle(angles);
                }
            }
            catch (Exception e) {
                Debug.LogException(e);

                return false;
            }

            return true;
        }
        public bool FromAngles(List<float> angles, Transform parent = null) {
            try {
                if (Next != null) {
                    Next.ChainDestroyTile();
                }

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

        public void Select() {
            SpriteRenderer.color = Color.green;
        }
        public void Unselect() {
            SpriteRenderer.color = Color.white;
        }

        public void Select(BremenTile end) {
            if (end == null) {
                Select();

                return;
            }

            if (Index > end.Index) {
                ChainActioner(
                    this,
                    o => {
                        o.Select();

                        if (o.Index == end.Index) {
                            return null;
                        }

                        return o.Previous;
                    }
                    );
            } 
            else {
                ChainActioner(
                    this,
                    o => {
                        o.Select();

                        if (o.Index == end.Index) {
                            return null;
                        }

                        return o.Next;
                    }
                    );
            }
        }
        public void Unselect(BremenTile end) {
            if (end == null) {
                Select();

                return;
            }

            if (Index > end.Index) {
                ChainActioner(
                    this,
                    o => {
                        o.Unselect();

                        if (o.Index == end.Index) {
                            return null;
                        }

                        return o.Previous;
                    }
                    );
            }
            else {
                ChainActioner(
                    this,
                    o => {
                        o.Unselect();

                        if (o.Index == end.Index) {
                            return null;
                        }

                        return o.Next;
                    }
                    );
            }
        }

        private static void ChainActioner(BremenTile instance, Func<BremenTile, BremenTile> on_chain) {
            var chain = on_chain.Invoke(instance);

            if (chain != null) {
                ChainActioner(chain, on_chain);
            }
        }
        private static void ChainActioner(BremenTile instance, Func<BremenTile, BremenTile> on_chain, Action<BremenTile, BremenTile> on_select_chain) {
            var chain = on_chain.Invoke(instance);

            if (chain != null) {
                on_select_chain?.Invoke(instance, chain);

                ChainActioner(chain, on_chain, on_select_chain);
            }
        }

        private void ChainNextIncreseIndex() => 
            ChainActioner(
                this, 
                o => {
                    o.Index++;

                    o.StartPoint = o.Previous.EndPoint;

                    return o.Next;
                }
                );
        private void ChainNextDecreseIndex() =>
            ChainActioner(
                this,
                o => {
                    o.Index--;

                    o.StartPoint = o.Previous.EndPoint;

                    return o.Next;
                }
                );

        private void ChainDestroyTile() =>
            ChainActioner(
                this,
                o => {
                    Destroy(o.gameObject);

                    return o.Next;
                }
                );

        private void ChainInsertAngle(List<float> angles) {
            ChainActioner(
                this,
                o => {
                    var angle = o.RelativeAngleAtPrevious;

                    angles.Add(angle);

                    return o.Next;
                }
                );
        }
    }
}