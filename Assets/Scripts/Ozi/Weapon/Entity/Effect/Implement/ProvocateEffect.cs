namespace Ozi.Weapon.Entity.Effect.Implement {
    public class ProvocateEffect : BasicEffect {
        public override string Title => "도발";
        public override string Description => "\"저 사람이 제게 옷걸이를 던졌다고요!\"";

        public override float Progress => _progress_time / _max_time;
        public override int Stack { get; protected set; }

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public ProvocateEffect(EffectParam param) : base(param) { }

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;

            Target.transform.LookAt(Caster.transform);

            Target.transform.position += Target.transform.forward * (Target.Status.speed * delta_time);
        }
    }
}