using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStats", menuName = "ScriptableObjects/MonsterStats", order = 1)]
public class MonsterStats : ScriptableObject
{
  [Header("몬스터 능력치")]
  public float health;      // 체력
  public float attackPower; // 공격력
  public float moveSpeed;   // 이동 속도
  public float attackRange; // 공격 범위
  public float patrolRange; // 순찰 범위

    public EntityStats ToEntityStats() {
        var stats = new EntityStats() {
            health = health,
            attack = attackPower,
            speed = moveSpeed,
        };

        return stats;
    }
}
