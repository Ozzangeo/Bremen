using UnityEngine;

namespace Ozi.Weapon.Entity.Effect.Implement {
    public class ElectricGuitarTeamIncreaseAttackEffect : BasicEffect {
        public const float ATTACK_LEVEL = 3;
        public const float EFFECT_RADIUS = 5.0f;

        public override string Title => "열기";
        public override string Description => "일렉기타의 뜨거운 열정에 몸이 달아올라 공격력이 증가했습니다.";

        public override float Progress => 1.0f;
        public override int Stack { get; protected set; }

        public override bool IsDone => Vector3.Distance(Caster.transform.position, Target.transform.position) > EFFECT_RADIUS;

        public ElectricGuitarTeamIncreaseAttackEffect(EffectParam param) : base(param) { }

        public override void OnAdded() {
            Stack++;

            Target.Status.attack += ATTACK_LEVEL;

            Target.Status.Notify();
        }

        public override void OnRemoved() {
            Target.Status.defense -= ATTACK_LEVEL * Stack;

            Target.Status.Notify();
        }
    }
}