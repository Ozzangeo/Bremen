﻿using Ozi.Weapon.Entity;
using Ozi.Weapon.Entity.Effect;
using Ozi.Weapon.Entity.Effect.Implement;
using Ozi.Weapon.Utility;
using System.Linq;
using UnityEngine;

namespace Ozi.Weapon {
    public class DoubleBassWeapon : BasicWeapon {
        private const float NORMAL_ATTACK_RADIUS = 1.5f;
        private const float NORMAL_ATTACK_KNOCKBACK = 0.3f;
        private const float NORMAL_ATTACK_KNOCKBACK_TIME = 1.0f;
        private const float NORMAL_ATTACK_DAMAGE = 85.0f;
        private const float NORMAL_ATTACK_MAX_DAMAGE = 110.0f;

        private const float PLAY_ATTACK_PROVOCATE_RADIUS = 10.0f;
        private const float PLAY_ATTACK_RADIUS = 4.0f;
        private const float PLAY_ATTACK_KNOCKBACK = 0.6f;
        private const float PLAY_ATTACK_KNOCKBACK_TIME = 1.0f;
        private const float PLAY_ATTACK_DAMAGE = 11.0f;
        private const float PLAY_ATTACK_MAX_DAMAGE = 33.0f;

        private readonly ElementCooldown<BasicEntityBehaviour> AttackKnockbackCooldown = new();

        protected override void OnUpdate() {
            AttackKnockbackCooldown.Update(Time.deltaTime);
        }

        protected override void NormalAttack() {
            var position = Owner.transform.position;
            var entities = 
                FindEntities(position, NORMAL_ATTACK_RADIUS)
                .Where(o => !o.IsSameTeam(Owner));

            // knockback logic
            var knockback_entities = entities.Where(o => !AttackKnockbackCooldown.HasCooldown(o));
            
            EntitiesKnockback(knockback_entities, position, NORMAL_ATTACK_KNOCKBACK);

            AttackKnockbackCooldown.AddElements(knockback_entities, NORMAL_ATTACK_KNOCKBACK_TIME);

            // damage logic
            var damage = Mathf.Clamp(NORMAL_ATTACK_DAMAGE + Owner.Status.attack, 0.0f, NORMAL_ATTACK_MAX_DAMAGE);

            EntitiesHit(entities, damage);
        }
        protected override void PlayAttack() {
            var position = Owner.transform.position;
            var entities =
                FindEntities(position, PLAY_ATTACK_RADIUS)
                .Where(o => o.transform != Owner.transform)
                .Where(o => !o.IsSameTeam(Owner));

            // knockback logic
            var knockback_entities = entities.Where(o => !AttackKnockbackCooldown.HasCooldown(o));
            
            EntitiesKnockback(knockback_entities, position, PLAY_ATTACK_KNOCKBACK);

            AttackKnockbackCooldown.AddElements(knockback_entities, PLAY_ATTACK_KNOCKBACK_TIME);

            // provocate logic
            var provocate_entities =
                FindEntities(position, PLAY_ATTACK_PROVOCATE_RADIUS)
                .Where(o => !o.IsSameTeam(Owner));

            EntitiesAddEffect(provocate_entities, o => new ProvocateEffect(new EffectParam(Owner, o, 0.5f)));

            // damage logic
            var damage = Mathf.Clamp(PLAY_ATTACK_DAMAGE + Owner.Status.attack, 0.0f, PLAY_ATTACK_MAX_DAMAGE);

            EntitiesHit(entities, damage);
        }
        protected override void SpecialAttack() {
            // why not exsit special attack in plan document?
        }
    }
}