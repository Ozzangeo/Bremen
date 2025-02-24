namespace Ozi.Weapon.Entity.Effect.Implement {
    public class PoisonEffect : BasicEffect {
        public const float DAMAGE_COOLTIME = 1.0f;
        public const float DAMAGE_LEVEL = 5.0f;

        public override string Title => "중독";
        public override string Description => "치명적인 독에 중독되었습니다. 천천히 체력이 감소합니다.";

        public override float Progress => _progress_time / _max_time;

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;
        private float _time;

        public PoisonEffect(EffectParam param) : base(param) {}

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
            _time += delta_time;

            if (_time >= DAMAGE_COOLTIME) {
                Target.Hit(DAMAGE_LEVEL);

                _time -= DAMAGE_COOLTIME;
            }
        }

        public override void OnReset() {
            _time = 0.0f;
        }
    }
}