namespace Ozi.Weapon.Entity.Effect.Implement {
    public class DecreaseDefenseEffect : BasicEffect {
        public const int MAX_STACK = 3;
        public const float DEFENSE_LEVEL = 5;

        public override string Title => "취약의 선율";
        public override string Description => "신비로운 선율에 의해 방어력이 감소했습니다.";

        public override float Progress => _progress_time / _max_time;
        public override int Stack { get; protected set; }

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public DecreaseDefenseEffect(EffectParam param) : base(param) { }

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;

            if (Stack < MAX_STACK) {
                Stack++;

                Target.Status.defense -= DEFENSE_LEVEL;

                Target.Status.Notify();
            }
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
        }

        public override void OnRemoved() {
            Target.Status.defense += DEFENSE_LEVEL * Stack;

            Target.Status.Notify();
        }

        public override void OnReset() {
            _progress_time = 0.0f;
        }
    }
}