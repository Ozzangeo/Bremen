namespace Ozi.Weapon.Entity.Effect.Implement {
    public class RegenerateEffect : BasicEffect {
        public const float HEAL_COOLTIME = 1.0f;
        public const float HEAL_LEVEL = 6.0f;

        public override string Title => "재생의 선율";
        public override string Description => "신비로운 선율에 의해 체력이 천천히 재생됩니다.";

        public override float Progress => _progress_time / _max_time;

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;
        private float _time;

        public RegenerateEffect(EffectParam param) : base(param) {}

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
            _time += delta_time;

            if (_time >= HEAL_COOLTIME) {
                Target.Heal(HEAL_LEVEL);

                _time -= HEAL_COOLTIME;
            }
        }

        public override void OnReset() {
            _time = 0.0f;
        }
    }
}