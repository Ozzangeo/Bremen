namespace Ozi.Weapon.Entity.Effect.Implement {
    public class HackingEffect : BasicEffect {
        public override string Title => "해킹";
        public override string Description => "해킹에 당해 공격 및 연주를 할 수 없습니다.";

        public override float Progress => _progress_time / _max_time;

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public HackingEffect(EffectParam param) : base(param) {}

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
        }
    }
}