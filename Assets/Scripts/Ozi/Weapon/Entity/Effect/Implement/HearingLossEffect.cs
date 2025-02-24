using UnityEngine;

namespace Ozi.Weapon.Entity.Effect.Implement {
    public class HearingLossEffect : BasicEffect {
        public const int MAX_STACK = 10;
        public const float BLIND_LEVEL = 0.1f;

        public override string Title => "난청";
        public override string Description => "말로 표현할 수 없는 아름다운 선율에 의해 귀가 멀어버렸습니다.";

        public override float Progress => _progress_time / _max_time;
        public override int Stack { get; protected set; }

        public override bool IsDone => Progress >= 1.0f;

        private float _max_time;
        private float _progress_time;

        public HearingLossEffect(EffectParam param) : base(param) { }

        public override void OnAdded() {
            if (Param.Time is float time) {
                _max_time = time;
            }

            _progress_time = 0.0f;

            if (Stack < MAX_STACK) {
                Stack++;

                AudioManager.GlobalVolume = 1.0f - (BLIND_LEVEL * Stack);
            }
        }

        public override void OnUpdate(float delta_time) {
            _progress_time += delta_time;
        }

        public override void OnRemoved() {
            AudioManager.GlobalVolume = 1.0f;
        }
    }
}