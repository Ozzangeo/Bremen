using System;
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
        }

        public void Hit(float damage) {
            Status.health -= damage;

            OnHit?.Invoke(damage);

            if (Status.health <= 0) {
                OnDead?.Invoke();
            }

            NotifyStatusChanged();
        }
        public void Heal(float heal) {
            Status.health = Mathf.Clamp(Status.health + heal, 0.0f, Status.max_health);

            OnHeal?.Invoke(heal);

            NotifyStatusChanged();
        }
        
        public void NotifyStatusChanged() {
            OnStatusChanged?.Invoke(Status);
        }

        public bool IsSameTeam(int team) => Team == team;
        public bool IsSameTeam(BasicEntityBehaviour behaviour) => IsSameTeam(behaviour.Team);
    }
}