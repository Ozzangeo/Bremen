using Ozi.Weapon.Entity;
using Ozi.Weapon.Entity.Effect;
using Ozi.Weapon.Entity.Effect.Implement;
using UnityEngine;

namespace Ozi {
    public class TestEntity : BasicEntityBehaviour {
        [ContextMenu("Blind")]
        public void Blind() {
            Status.AddEffect(new BlindEffect(new EffectParam(this, this, 10.0f)));
        }
    }
}