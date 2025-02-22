using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MidBossEyeBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("돌진 속도")] public float dashSpeed = 16f;    // 돌진 속도
  [Header("공격 쿨타임")] public float attackRate = 2f;  // 공격 쿨타임
  
  float lastAttackTimePlayer = 0f;  // 마지막 공격 시간 (플레이어)
  float lastAttackTimeBitCore = 0f; // 마지막 공격 시간 (비트코어)

  // 비트 코어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    if(Time.time - lastAttackTimeBitCore >= attackRate)
    {
      StartCoroutine(Dash(bitCore, monsterStats));
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
     StartCoroutine(Dash(player, monsterStats));
      lastAttackTimePlayer = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 돌진 코루틴
  private IEnumerator Dash(Transform player, MonsterStats monsterStats)
  {
    while(Vector3.Distance(transform.position, player.position) > 0.5f)
    {
      Debug.Log("돌진");
      transform.position = Vector3.MoveTowards(transform.position, player.position, dashSpeed * Time.deltaTime);
      yield return null;
    }
  }
}
