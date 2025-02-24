using Ozi.Weapon.Entity;
using Ozi.Weapon.Entity.Effect;
using Ozi.Weapon.Entity.Effect.Implement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozi.Weapon {
    public class KeyboardWeapon : BasicWeapon {
        private const float NORMAL_ATTACK_BUFF_RADIUS = 10.0f;
        private const float NORMAL_ATTACK_DEBUFF_RADIUS = 10.0f;
        private const float NORMAL_ATTACK_DAMAGE = 70.0f;
        private const float NORMAL_ATTACK_BUFF_TIME = 7.0f;
        private const float NORMAL_ATTACK_DEBUFF_TIME = 5.0f;

        private const float PLAY_ATTACK_HEAL_RADIUS = 4.0f;

        private const float PLAY_ATTACK_HEAL = 3.0f;

        private static int GetHashCode<T>() => typeof(T).GetHashCode(); 
        private Dictionary<int, Func<BasicEntityBehaviour, BasicEffect>> PositiveEffects;
        private Dictionary<int, Func<BasicEntityBehaviour, BasicEffect>> NegativeEffects;

        public KeyValuePair<int, Func<BasicEntityBehaviour, BasicEffect>> GetRandomPositiveEffect() {
            var random = UnityEngine.Random.Range(0, PositiveEffects.Count);

            return PositiveEffects.ElementAt(random);
        }
        public KeyValuePair<int, Func<BasicEntityBehaviour, BasicEffect>> GetRandomNegativeEffect() {
            var random = UnityEngine.Random.Range(0, NegativeEffects.Count);

            return NegativeEffects.ElementAt(random);
        }

        private void Awake() {
            PositiveEffects = new() {
                { GetHashCode<RegenerateEffect>()       , o => new RegenerateEffect     (new EffectParam(Owner, o, NORMAL_ATTACK_BUFF_TIME)) },
                { GetHashCode<IncreaseSpeedEffect>()    , o => new IncreaseSpeedEffect  (new EffectParam(Owner, o, NORMAL_ATTACK_BUFF_TIME)) },
                { GetHashCode<IncreaseDefenseEffect>()  , o => new IncreaseDefenseEffect(new EffectParam(Owner, o, NORMAL_ATTACK_BUFF_TIME)) },
            };

            NegativeEffects = new() {
                { GetHashCode<HackingEffect>()          , o => new HackingEffect        (new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
                { GetHashCode<PoisonEffect>()           , o => new PoisonEffect         (new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
                { GetHashCode<DecreaseSpeedEffect>()    , o => new DecreaseSpeedEffect  (new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
                { GetHashCode<DecreaseDefenseEffect>()  , o => new DecreaseDefenseEffect(new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
                { GetHashCode<HearingLossEffect>()      , o => new HearingLossEffect    (new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
                { GetHashCode<BlindEffect>()            , o => new BlindEffect          (new EffectParam(Owner, o, NORMAL_ATTACK_DEBUFF_TIME)) },
            };
        }

        protected override void NormalAttack() {
            var position = Owner.transform.position;

            // owner included
            var team_entites = 
                FindEntities(position, NORMAL_ATTACK_BUFF_RADIUS)
                .Where(o => o.IsSameTeam(Owner));
            var other_team_entites =
                FindEntities(position, NORMAL_ATTACK_DEBUFF_RADIUS)
                .Where(o => !o.IsSameTeam(Owner));

            // buff team entities logic
            var positive_effect = GetRandomPositiveEffect();
            EntitiesAddEffect(team_entites, positive_effect.Key, positive_effect.Value);

            // debuff other team entities logic
            var negative_effect = GetRandomNegativeEffect();
            EntitiesAddEffect(other_team_entites, negative_effect.Key, negative_effect.Value);

            EntitiesHit(other_team_entites, NORMAL_ATTACK_DAMAGE + Owner.Status.attack);
        }
        protected override void PlayAttack() {
            var position = Owner.transform.position;
            var entities = FindEntities(position, PLAY_ATTACK_HEAL_RADIUS);

            // owner included
            var team_entites = entities.Where(o => o.IsSameTeam(Owner));

            var combo = ChartPlayer.Combo;

            EntitiesHeal(team_entites, PLAY_ATTACK_HEAL * combo);
        }
        protected override void SpecialAttack() {
            // why not exsit special attack in plan document?
        }
    }
}