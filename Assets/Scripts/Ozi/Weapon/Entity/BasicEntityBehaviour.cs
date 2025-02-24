using Ozi.Weapon.Entity.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozi.Weapon.Entity {
    public class BasicEntityBehaviour : MonoBehaviour {
        [Header("Recommended Init Status Data")]
        [SerializeField] private EntityStatusObject _initStatusData;
        
        [Header("Init Status Data (For Compatibility)")]
        public BossStats bossStats;
        public MonsterStats monsterStats;
        public CharacterData selectedCharacter;

        [field: Header("Debugs")]
        [field: SerializeField] public EntityStatus Status { get; protected set; }
        [field: SerializeField] public int Team { get; set; } = 0;

        private IEnumerator _onUpdate;

        public event Action<float> OnHit;   // damage: float
        public event Action<float> OnHeal;  // heal: float
        public event Action<EntityStatus> OnStatusChanged;
        public event Action OnDead;

        protected virtual void Awake() {
            if (_initStatusData != null) {
                Status = EntityStatus.Clone(_initStatusData.Status);

                Team = -1;
            }
            else if (bossStats != null) {
                Status = EntityStatus.FromBossStats(bossStats);

                Team = 1;
            }
            else if (monsterStats != null) {
                Status = EntityStatus.FromMonsterStats(monsterStats);

                Team = 1;
            }
            else if (selectedCharacter != null) {
                Status = EntityStatus.FromCharacterData(selectedCharacter);

                Team = 0;
            }
            else {
                Status = new();
            }

            Status.OnStatusNotified += o => OnStatusChanged?.Invoke(o);

            StartCoroutine(_onUpdate = OnUpdate());
        }

        protected virtual void OnDestroy() {
            StopCoroutine(_onUpdate);
        }

        public void Hit(float damage) {
            Status.health -= damage;

            OnHit?.Invoke(damage);

            if (Status.health <= 0) {
                OnDead?.Invoke();
            }

            Status.Notify();
        }
        public void Heal(float heal) {
            Status.health = Mathf.Clamp(Status.health + heal, 0.0f, Status.max_health);

            OnHeal?.Invoke(heal);

            Status.Notify();
        }
        
        public bool IsSameTeam(int team) => Team == team;
        public bool IsSameTeam(BasicEntityBehaviour behaviour) => IsSameTeam(behaviour.Team);
        
        public IEnumerator OnUpdate() {
            while (true) {
                Status.OnUpdate(Time.deltaTime);

                yield return null;
            }
        }
    }
}