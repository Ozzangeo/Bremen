using UnityEngine;

[System.Serializable]
public class EntityStatus {
    public string name;

    public float attack;
    public float defense;
    public float health;
    public float max_health;
    public float speed;

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