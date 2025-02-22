using System;
using UnityEngine;

// 귀(중간 보스) 행동 트리를 생성하는 팩토리
// 2박자에 1번이 정확히 뭔지 몰라서 2초에 한번 발사하는거로 임시 구현
public class MidBossEarBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("공격 쿨타임")] public float attackRate = 2f;      // 공격 쿨타임
  
  float lastAttackTimePlayer = 0f;  // 마지막 공격 시간 (플레이어)
  float lastAttackTimeBitCore = 0f; // 마지막 공격 시간 (비트코어)

  // 비트 코어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    if(Time.time - lastAttackTimeBitCore >= attackRate)
    {
      Attack();
      lastAttackTimeBitCore = Time.time;
    }

    Debug.Log("공격 상태(비트코어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 플레이어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Transform monster)
  {
    if(!isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로가 아니라면 비트코어 타겟팅

    if(Time.time - lastAttackTimePlayer >= attackRate)
    {
      Attack();
      lastAttackTimePlayer = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 공격
  private void Attack()
  {
    Debug.Log("공격");
  }
}
