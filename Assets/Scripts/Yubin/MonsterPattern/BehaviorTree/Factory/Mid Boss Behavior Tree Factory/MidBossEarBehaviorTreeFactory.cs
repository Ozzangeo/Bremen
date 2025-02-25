using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 귀(중간 보스) 행동 트리를 생성하는 팩토리
// 2박자에 1번이 정확히 뭔지 몰라서 2초에 한번 발사하는거로 임시 구현
public class MidBossEarBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("공격 쿨타임")] public float attackRate = 2f;      // 공격 쿨타임
  
  float lastAttackTimePlayer = 0f;  // 마지막 공격 시간 (플레이어)
  float lastAttackTimeBitCore = 0f; // 마지막 공격 시간 (비트코어)

  Animator animator;

  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // 비트 코어 접근
  public override IBehaviorNode.EBehaviorNodeState AccessBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    float moveSpeed = monsterStats.moveSpeed;     // 이동 속도
    float attackRange = monsterStats.attackRange; // 공격 범위

    float bitCoreDistance = Vector3.Distance(monster.position, bitCore.position);
    if(bitCoreDistance <= attackRange)
    {
      Debug.Log("공격 상태 전환(비트코어)");
      return IBehaviorNode.EBehaviorNodeState.Success;
    }

    // 비트 코어에 접근
    Debug.Log("비트코어 접근");
    monster.position = Vector3.MoveTowards(monster.position, bitCore.position, moveSpeed * Time.deltaTime);
    animator.SetBool("IsAttack", false);

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 플레이어 추적
  public override IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform player, MonsterStats monsterStats, Transform monster)
  {
    if(!isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로가 아니라면 비트코어 타겟팅

    float moveSpeed = monsterStats.moveSpeed;     // 이동 속도
    float attackRange = monsterStats.attackRange; // 공격 범위

    float playerDistance = Vector3.Distance(monster.position, player.position);
    if(playerDistance >= playerAway)
    {
      Debug.Log("어그로 해제");
      isPlayerTarget = false;
      return IBehaviorNode.EBehaviorNodeState.Failure;
    }
    
    if(playerDistance <= attackRange)
    {
      Debug.Log("공격 상태 전환(플레이어)");
      return IBehaviorNode.EBehaviorNodeState.Success;
    }
  
    // 플레이어 추적
    monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);    
    animator.SetBool("IsAttack", false);

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 비트 코어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    if(Time.time - lastAttackTimeBitCore >= attackRate)
    {
      StartCoroutine(Attack(monsterStats));
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
      StartCoroutine(Attack(monsterStats));
      lastAttackTimePlayer = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 공격
  private IEnumerator Attack(MonsterStats monsterStats)
  {
    Debug.Log("공격");

    animator.SetBool("IsAttack", true);

    yield return new WaitForSeconds(2f);
  }
}
