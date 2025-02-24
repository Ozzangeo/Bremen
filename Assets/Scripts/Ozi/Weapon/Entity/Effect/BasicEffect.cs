using Ozi.Weapon.Utility;
using UnityEngine;

namespace Ozi.Weapon.Entity.Effect {
    public class BasicEffect : IUpdatable {
        public virtual Sprite Profile { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Description { get; protected set; }
        
        public virtual float Progress { get; protected set; } = 0.0f;
        public virtual int Stack { get; protected set; } = 0;

        public virtual bool IsDone { get; protected set; } = false;

        public EffectParam Param { get; protected set; }
        public BasicEntityBehaviour Caster => Param.Caster;
        public BasicEntityBehaviour Target => Param.Target;

        public BasicEffect(EffectParam param) {
            Param = param;
        }

        public virtual void OnAdded() { }
        public virtual void OnRemoved() { }
        public virtual void OnReset() { }
        public void Reset() {
            Progress = 0.0f;
            Stack = 0;

            OnReset();
        }

        public virtual void OnUpdate(float delta_time) { }
    }
}