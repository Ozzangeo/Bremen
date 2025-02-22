using UnityEngine;
using System;

// 보스의 유틸리티 AI를 생성하는 팩토리 (임시)
public class BossUtilityFactory : IUtilityFactory
{
  private readonly Transform boss;    // 보스 자신
  private readonly Transform player;  // 플레이어

  private float health;
  private float maxHealth;
  private float attackRange;
  private float fleeDistance;

  public BossUtilityFactory(Transform boss, Transform player, float health, float maxHealth, float attackRange, float fleeDistance)
  {
    this.boss = boss;
    this.player = player;
    this.health = health;
    this.maxHealth = maxHealth;
    this.attackRange = attackRange;
    this.fleeDistance = fleeDistance;
  }

  public Action GetAction(string actionName)
  {
    switch(actionName)
    {
      case "Heal": return Heal;
      case "Attack": return Attack;
      case "Chase": return Chase;
      case "Flee": return Flee;
      default: return null;
    }
  }

  private void Heal()
  {
    Debug.Log("힐링중");
    health = Mathf.Min(maxHealth, health + 10f * Time.deltaTime);
  }

  private void Attack()
  {
    Debug.Log("공격중");
  }

  private void Chase()
  {
    Debug.Log("추적중");
    boss.position = Vector3.MoveTowards(boss.position, player.position, 3f * Time.deltaTime);
  }

  private void Flee()
  {
    Debug.Log("도망중");
    if(Vector3.Distance(boss.position, player.position) < fleeDistance) boss.position += (boss.position - player.position).normalized * 5f * Time.deltaTime;
  }
}
