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

        public event Action OnHit;
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

        public void GetDamage(float damage) {
            Status.health -= damage;

            OnHit?.Invoke();

            if (Status.health <= 0) {
                OnDead?.Invoke();
            }
        }

        public bool IsSameTeam(int team) => Team == team;
        public bool IsSameTeam(BasicEntityBehaviour behaviour) => IsSameTeam(behaviour.Team);
    }
}