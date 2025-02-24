using Ozi.Weapon.Entity;
using Ozi.Weapon.Entity.Effect;
using Ozi.Weapon.Entity.Effect.Implement;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace Ozi.Weapon {
    public class ElectricGuitarWeapon : BasicWeapon {
        private const float NORMAL_ATTACK_STANDARD_COMBO = 3.0f;
        private const float NORMAL_ATTACK_RADIUS = 0.25f;
        private const float NORMAL_ATTACK_DAMAGE = 115.0f;
        private const float NORMAL_ATTACK_MAX_DAMAGE = 230.0f;

        private const float PLAY_ATTACK_INCREASE_ATTACK = 3.0f;
        private const float PLAY_ATTACK_STANDARD_COMBO = 8.0f;
        private const float PLAY_ATTACK_RADIUS = 1.0f;
        private const float PLAY_ATTACK_BUFF_RADIUS = 5.0f;
        private const float PLAY_ATTACK_DAMAGE = 30.0f;
        private const float PLAY_ATTACK_MAX_DAMAGE = 78.0f;

        private Dictionary<BasicEntityBehaviour, float> _playIncreasedAttackByEntity;
        private HashSet<BasicEntityBehaviour> _buffedEntities;

        private void ClearPlayIncreased() {
            foreach (var pair in _playIncreasedAttackByEntity) {
                var entity = pair.Key;
                var increased = pair.Value;

                entity.Status.attack -= increased;

                entity.Status.Notify();
            }
            _playIncreasedAttackByEntity.Clear();
        }

        protected override void OnInitialize() {
            _playIncreasedAttackByEntity = new();

            OnComboAdd +=
                () => {
                    // Normal
                    if (ChartPlayer.Combo % NORMAL_ATTACK_STANDARD_COMBO == 0) {
                        Owner.Status.AddEffect(new ElectricGuitarIncreaseAttackEffect(new EffectParam(Owner, Owner)));
                    }

                    // Play
                    if (Input.GetKey(PLAY_ATTACK_KEY)) {
                        PlayAttack();
                    }
                };
            OnComboReset +=
                () => {
                    // Normal
                    Owner.Status.RemoveEffect<ElectricGuitarIncreaseAttackEffect>();

                    // Play
                    foreach (var entity in _buffedEntities) {
                        entity.Status.RemoveEffect<ElectricGuitarTeamIncreaseAttackEffect>();
                    }
                };
        }

        protected override void NormalAttack() {
            var position = Owner.transform.position;
            var forward = Owner.transform.forward;

            var entities =
                FindEntities(position + forward, NORMAL_ATTACK_RADIUS)
                .Where(o => o.IsSameTeam(Owner));

            var damage = Mathf.Clamp(NORMAL_ATTACK_DAMAGE + Owner.Status.attack, 0.0f, NORMAL_ATTACK_MAX_DAMAGE);

            EntitiesHit(entities, damage);
        }
        protected override void PlayAttack() {
            var position = Owner.transform.position;
            var entities = FindEntities(position, PLAY_ATTACK_RADIUS);

            var team_entities =
                FindEntities(position, PLAY_ATTACK_BUFF_RADIUS)
                .Where(o => o.IsSameTeam(Owner));
            var other_team_entities = entities.Where(o => !o.IsSameTeam(Owner));

            if (ChartPlayer.Combo % PLAY_ATTACK_STANDARD_COMBO == 0) {
                EntitiesAddEffect(team_entities, o => new ElectricGuitarTeamIncreaseAttackEffect(new EffectParam(Owner, o)));

                foreach (var entity in team_entities) {
                    _buffedEntities.Add(entity);
                }
            }

            var damage = Mathf.Clamp(PLAY_ATTACK_DAMAGE + Owner.Status.attack, 0.0f, PLAY_ATTACK_MAX_DAMAGE);

            EntitiesHit(other_team_entities, damage);
        }
        protected override void SpecialAttack() {
            // why not exsit special attack in plan document?
        }
    }
}