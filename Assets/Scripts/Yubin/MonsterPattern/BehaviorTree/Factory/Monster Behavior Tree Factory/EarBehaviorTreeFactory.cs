using UnityEngine;

// 귀 행동 트리를 생성하는 팩토리
// 2박자에 1번이 정확히 뭔지 몰라서 2초에 한번 발사하는거로 임시 구현
public class EarBehaviorTreeFactory : BehaviorTreeFactory
{
  float attackRate = 2f;      // 공격 쿨타임
  float lastAttackTime = 0f;  // 마지막 공격 시간 
  
  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange; // 순찰 범위

    // 플레이어가 순찰 범위 내에 있으면 공격 상태로 전환, 없다면 순찰 상태로 전환
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 쿨타임마다 공격
    if(Time.time - lastAttackTime >= attackRate)
    {
      Attack(player, monsterStats);
      lastAttackTime = Time.time;
    }
    
    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 근접 공격
  public void Attack(Transform player, MonsterStats monsterStats)
  {
    Debug.Log("공격");
  }
}
