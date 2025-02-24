namespace Ozi.Weapon.Entity.Effect.Implement {
    public class ElectricGuitarIncreaseAttackEffect : BasicEffect {
        public const float ATTACK_LEVEL = 5;

        public override string Title => "열정";
        public override string Description => "자신의 연주에 심취하여 공격력이 증가했습니다.";

        public override float Progress => 1.0f;
        public override int Stack { get; protected set; }

        public override bool IsDone => false;

        public ElectricGuitarIncreaseAttackEffect(EffectParam param) : base(param) { }

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