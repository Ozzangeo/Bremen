namespace Ozi.Weapon.Entity.Effect.Implement {
    public class BlindEffect : BasicEffect {
        public const int MAX_STACK = 10;
        public const float BLIND_LEVEL = 0.1f;

        public override string Title => "실명";
        public override string Description => "말로 표현할 수 없는 아름다운 선율에 의해 눈이 멀어버렸습니다.";

        public override float Progress => _progress_time / _max_time;
        public override int Stack { get; protected set; }

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public BlindEffect(EffectParam param) : base(param) {}

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;
            
            // blind logic
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
        }
    }
}