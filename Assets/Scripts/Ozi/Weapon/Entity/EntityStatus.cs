using Ozi.Weapon.Entity.Effect;
using Ozi.Weapon.Utility;
using System;
using System.Collections.Generic;

[System.Serializable]
public class EntityStatus : IUpdatable {
    public string name;

    public float attack;
    public float defense;
    public float health;
    public float max_health;
    public float speed;

    public Dictionary<int, BasicEffect> effects = new();
    private readonly List<int> _removes = new();

    public event Action<EntityStatus, BasicEffect> OnAddEffect;
    public event Action<EntityStatus, BasicEffect> OnRemoveEffect;
    public event Action<EntityStatus> OnUpdateEffect;

    public event Action<EntityStatus> OnStatusNotified;

    public void AddEffect<T>(T effect) where T : BasicEffect {
        var hash_code = typeof(T).GetHashCode();

        AddEffect(hash_code, effect);
    }
    public void AddEffect(int hash_code, BasicEffect effect) {
        if (effects.ContainsKey(hash_code)) {
            effect = effects[hash_code];
        }
        else {
            effects[hash_code] = effect;

            effect?.Reset();
        }
        effect?.OnAdded();

        OnAddEffect?.Invoke(this, effect);
        OnUpdateEffect?.Invoke(this);
    }
    public void RemoveEffect<T>() where T : BasicEffect {
        var hash_code = typeof(T).GetHashCode();

        OnEffectDone(hash_code);
    }

    public bool HasEffect<T>() where T : BasicEffect {
        var hash_code = typeof(T).GetHashCode();

        return effects.ContainsKey(hash_code);
    }

    private void OnEffectDone(int hash_code) {
        var effect = effects[hash_code];

        effect?.OnRemoved();

        OnRemoveEffect?.Invoke(this, effect);
        OnUpdateEffect?.Invoke(this);
        
        effects.Remove(hash_code);
    }

    public void OnUpdate(float delta_time) {
        foreach (var pair in effects) {
            var hash_code = pair.Key;
            var effect = pair.Value;

            effect.OnUpdate(delta_time);

            if (effect.IsDone) {
                _removes.Add(hash_code);
            }
        }

        foreach (var hash_code in _removes) {
            OnEffectDone(hash_code);
        }
        _removes.Clear();
    }

    public void Notify() {
        OnStatusNotified?.Invoke(this);
    }

    public static EntityStatus Clone(EntityStatus status) {
        return new EntityStatus() {
            name = status.name,
            attack = status.attack,
            defense = status.defense,
            health = status.health,
            max_health = status.max_health,
            speed = status.speed,
        };
    }
    public static EntityStatus FromMonsterStats(MonsterStats stats) {
        return new EntityStatus() {
            health = stats.health,
            max_health = stats.health,
            attack = stats.attackPower,
            speed = stats.moveSpeed,
        };
    }
    public static EntityStatus FromBossStats(BossStats stats) {
        return new EntityStatus() {
            health = stats.health,
            max_health = stats.health,
        };
    }
    public static EntityStatus FromCharacterData(CharacterData data) {
        return new EntityStatus() {
            health = data.maxHP,
            max_health = data.maxHP,
            speed = data.moveSpeed,
        };
    }
}