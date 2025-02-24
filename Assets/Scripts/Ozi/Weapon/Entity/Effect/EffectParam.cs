namespace Ozi.Weapon.Entity.Effect {
    public readonly struct EffectParam {
        public readonly BasicEntityBehaviour Caster { get; }
        public readonly BasicEntityBehaviour Target { get; }
        public readonly float? Time { get; }

        public EffectParam(BasicEntityBehaviour caster, BasicEntityBehaviour target, float? time = null) {
            Caster = caster;
            Target = target;

            Time = time;
        }
    }
}