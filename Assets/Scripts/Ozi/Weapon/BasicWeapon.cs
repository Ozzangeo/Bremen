using Ozi.ChartPlayer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.Weapon {
    public class BasicWeapon : MonoBehaviour {
        public const int MAX_PHYSICS_COLLIDER_COUNT = 20;

        public const KeyCode PLAY_ATTACK_KEY = KeyCode.Q;
        public const KeyCode SPECIAL_ATTACK_KEY = KeyCode.R;
        private readonly static Collider[] PhysicsColliders = new Collider[MAX_PHYSICS_COLLIDER_COUNT];

        [field: Header("Requires")]
        [field: SerializeField] public BremenChartPlayer ChartPlayer { get; protected set; }
        
        [field: Header("Settings")]
        [field: SerializeField] public BasicEntityBehaviour Owner { get; set; }
        
        [field: Header("Debugs")]
        [field: SerializeField] public int CumulativeCombo { get; protected set; } = 0;

        public virtual int NeedComboSpecialAttack => 10;

        public event Action OnUseNormalAttack;
        public event Action OnUsePlayAttack;
        // [Obsolete] public event Action OnUseSpecialAttack;

        public event Action OnUseFailedNormalAttack;
        public event Action OnUseFailedPlayAttack;
        // [Obsolete] public event Action OnUseFailedSpecialAttack;

        public event Action OnComboAdd;
        public event Action OnComboReset;

        protected virtual void NormalAttack() => Debug.Log("Default Normal Attack");
        protected virtual void PlayAttack() => Debug.Log("Default Play Attack");
        protected virtual void SpecialAttack() => Debug.Log("Default Special Attack");

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate() { }

        protected virtual void Start() {
            if (ChartPlayer == null) {
                ChartPlayer = GameObject.FindAnyObjectByType<BremenChartPlayer>();
            }

            if (ChartPlayer == null) {
                Debug.LogError($"언놈이 박자에 맞춘 무기 사용하는데 차트 플레이어를 씬에 안뒀어?");

                return;
            }

            ChartPlayer.OnComboAdd += 
                o => {
                    CumulativeCombo++;

                    OnComboAdd?.Invoke();
                };
            ChartPlayer.OnComboReset += 
                () => {
                    OnComboReset?.Invoke();
                };

            OnInitialize();
        }

        protected virtual void Update() {
            // if onwer has haking effect -> return

            #region DeprecatedLogic
            // Normal attack cancelable
            //if (Input.GetKeyDown(SPECIAL_ATTACK_KEY)) {
            //    if (CumulativeCombo >= NeedComboSpecialAttack) {
            //        SpecialAttack();

            //        CumulativeCombo = 0;

            //        OnUseSpecialAttack?.Invoke();
            //    } else {
            //        OnUseFailedSpecialAttack?.Invoke();
            //    }
            //}
            //else
            #endregion

            if (Input.GetMouseButtonDown(0)) {
                var result = ChartPlayer.TryProcessNote();

                if (Input.GetKey(PLAY_ATTACK_KEY)) {
                    switch (result) {
                        case BremenNoteResult.Perfect:
                            PlayAttack();

                            OnUsePlayAttack?.Invoke();

                            break;
                        case BremenNoteResult.Miss:
                            OnUseFailedPlayAttack?.Invoke();

                            break;
                    }
                } else {
                    switch (result) {
                        case BremenNoteResult.Perfect:
                            NormalAttack();

                            OnUseNormalAttack?.Invoke();

                            break;
                        case BremenNoteResult.Miss:
                            OnUseFailedNormalAttack?.Invoke();

                            break;
                    }
                }
            }

            OnUpdate();
        }

        protected List<BasicEntityBehaviour> FindEntities(Vector3 origin, float radius, int layer_mask = -1) {
            var entities = new List<BasicEntityBehaviour>();

            int count = Physics.OverlapSphereNonAlloc(origin, radius, PhysicsColliders, layer_mask);
            for (int i = 0; i < count; i++) {
                var collider = PhysicsColliders[i];

                if (collider.TryGetComponent<BasicEntityBehaviour>(out var entity)) {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        protected void EntitiesKnockback(IEnumerable<BasicEntityBehaviour> entities, Vector3 origin, float power, ForceMode mode = ForceMode.VelocityChange)
            => EntitiesKnockback(entities, origin, power, o => 1.0f, mode);
        protected void EntitiesKnockback(IEnumerable<BasicEntityBehaviour> entities, Vector3 origin, float power, Func<float, float> power_graph, ForceMode mode = ForceMode.VelocityChange) {
            foreach (var entity in entities) {
                if (entity.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    var position = entity.transform.position;

                    var direction = (position - origin).normalized;

                    var distance = Vector3.Distance(position, origin);
                    var power_rate = power_graph.Invoke(distance);

                    rigidbody.AddForce(direction * (power * power_rate), mode);
                }
            }
        }
        
        protected void EntitiesHit(IEnumerable<BasicEntityBehaviour> entities, float damage) {
            foreach (var entity in entities) {
                var apply_damage = damage - entity.Status.defense;

                entity.GetDamage(Math.Clamp(apply_damage, 0.0f, float.MaxValue));
            }
        }
        protected void EntitiesHeal(IEnumerable<BasicEntityBehaviour> entities, float heal) {
            foreach (var entity in entities) {
                var apply_heal = heal;

                entity.Status.health += Math.Clamp(apply_heal, 0.0f, float.MaxValue);
            }
        }
    }
}