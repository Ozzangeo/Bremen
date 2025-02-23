using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ozi.Weapon {
    public class ElectricGuitarWeapon : BasicWeapon {
        private const float NORMAL_ATTACK_INCREASE_ATTACK = 5.0f;
        private const float NORMAL_ATTACK_STANDARD_COMBO = 3.0f;
        private const float NORMAL_ATTACK_RADIUS = 0.25f;
        private const float NORMAL_ATTACK_DAMAGE = 115.0f;
        private const float NORMAL_ATTACK_MAX_DAMAGE = 230.0f;

        private const float PLAY_ATTACK_INCREASE_ATTACK = 3.0f;
        private const float PLAY_ATTACK_STANDARD_COMBO = 8.0f;
        private const float PLAY_ATTACK_RADIUS = 3.0f;
        private const float PLAY_ATTACK_DAMAGE = 30.0f;
        private const float PLAY_ATTACK_MAX_DAMAGE = 78.0f;

        [SerializeField] private float _normalIncreasedAttack = 0.0f;
        private Dictionary<TestEntity, float> _playIncreasedAttackByEntity;
        private List<TestEntity> _playRemoveEntities;

        private void ClearPlayIncreased() {
            foreach (var pair in _playIncreasedAttackByEntity) {
                var entity = pair.Key;
                var increased = pair.Value;

                entity.status.attack -= increased;
            }
            _playIncreasedAttackByEntity.Clear();
        }

        protected override void OnInitialize() {
            _playIncreasedAttackByEntity = new();
            _playRemoveEntities = new();

            OnComboAdd +=
                () => {
                    // Normal
                    var before_increased = _normalIncreasedAttack;

                    int normal_combo = (int)(ChartPlayer.Combo / NORMAL_ATTACK_STANDARD_COMBO);
                    _normalIncreasedAttack = normal_combo * NORMAL_ATTACK_INCREASE_ATTACK;

                    Owner.status.attack += _normalIncreasedAttack - before_increased;

                    // Play
                    if (Input.GetKey(PLAY_ATTACK_KEY)) {
                        PlayAttack();
                    } else {
                        ClearPlayIncreased();
                    }
                };
            OnComboReset +=
                () => {
                    // Normal
                    Owner.status.attack -= _normalIncreasedAttack;

                    _normalIncreasedAttack = 0.0f;

                    // Play
                    ClearPlayIncreased();
                };
        }

        protected override void NormalAttack() {
            var position = Owner.transform.position;
            var forward = Owner.transform.forward;

            var entities =
                FindEntities(position + forward, NORMAL_ATTACK_RADIUS)
                .Where(o => o.IsSameTeam(Owner.team));

            var damage = Mathf.Clamp(Owner.status.attack, NORMAL_ATTACK_DAMAGE, NORMAL_ATTACK_MAX_DAMAGE);

            EntitiesHit(entities, damage);
        }
        protected override void PlayAttack() {
            var position = Owner.transform.position;
            var entities = FindEntities(position, PLAY_ATTACK_RADIUS);

            var team_entities = entities.Where(o => o.IsSameTeam(Owner.team));
            var other_team_entities = entities.Where(o => !o.IsSameTeam(Owner.team));

            int play_combo = (int)(ChartPlayer.Combo / PLAY_ATTACK_STANDARD_COMBO);
            var play_attack = play_combo * PLAY_ATTACK_INCREASE_ATTACK;

            var inner_entities = _playIncreasedAttackByEntity.Where(o => entities.Contains(o.Key));
            
            ClearPlayIncreased();

            _playIncreasedAttackByEntity = inner_entities.ToDictionary(o => o.Key, o => o.Value);

            foreach (var pair in _playIncreasedAttackByEntity) {
                var entity = pair.Key;
                var increased = pair.Value;

                entity.status.attack += play_attack;
                _playIncreasedAttackByEntity[entity] = play_attack;
            }

            var damage = Mathf.Clamp(Owner.status.attack, PLAY_ATTACK_DAMAGE, PLAY_ATTACK_MAX_DAMAGE);

            EntitiesHit(other_team_entities, damage);
        }
        protected override void SpecialAttack() {
            // why not exsit special attack in plan document?
        }
    }
}