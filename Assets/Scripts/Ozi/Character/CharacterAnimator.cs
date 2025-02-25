using System;
using UnityEngine;

namespace Ozi.Character {
    public class CharacterAnimator : MonoBehaviour {
        public const string SPEED_VALUE_NAME = "Speed";
        public const string JUMP_VALUE_NAME = "Jumping";

        [field: Header("Requires")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public Func<float> SpeedSelector;
        public Func<bool> JumpSelector;

        private void Awake() {
            JumpSelector ??= () => Rigidbody.linearVelocity.y > 0.1f;
            SpeedSelector ??= () => Rigidbody.linearVelocity.magnitude;
        }

        private void Update() {
            if (JumpSelector?.Invoke() is bool jump) {
                Animator.SetBool(JUMP_VALUE_NAME, jump);
            }

            if (SpeedSelector?.Invoke() is float speed) {
                Animator.SetFloat(SPEED_VALUE_NAME, speed);
            }
        }
    }
}