namespace Ozi.Weapon.Entity.Effect.Implement {
    public class DecreaseSpeedEffect : BasicEffect {
        public const int MAX_STACK = 3;
        public const float SPEED_LEVEL = 2;

        public override string Title => "감속의 선율";
        public override string Description => "신비로운 선율에 의해 이동속도가 감소했습니다.";

        public override float Progress => _progress_time / _max_time;
        public override int Stack { get; protected set; }

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public DecreaseSpeedEffect(EffectParam param) : base(param) { }

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;

            if (Stack < MAX_STACK) {
                Stack++;

                Target.Status.speed -= SPEED_LEVEL;

                Target.Status.Notify();
            }
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
        }

        public override void OnRemoved() {
            Target.Status.speed += SPEED_LEVEL * Stack;

            Target.Status.Notify();
        }
    }
}