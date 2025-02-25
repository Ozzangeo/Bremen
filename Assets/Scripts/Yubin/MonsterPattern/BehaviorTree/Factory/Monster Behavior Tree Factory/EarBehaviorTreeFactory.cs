using System.Collections.Generic;
using System.Collections;
using Ozi.Weapon.Entity;
using UnityEngine;

// 귀 행동 트리를 생성하는 팩토리
// 2박자에 1번이 정확히 뭔지 몰라서 2초에 한번 발사하는거로 임시 구현
public class EarBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("공격 쿨타임")] public float attackRate = 2f;      // 공격 쿨타임
  float lastAttackTime = 0f;  // 마지막 공격 시간 
  
  Animator animator;

  void Start()
  {
    animator = GetComponent<Animator>();
  }
  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange; // 순찰 범위

    // 플레이어가 순찰 범위 내에 있으면 공격 상태로 전환, 없다면 순찰 상태로 전환
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 쿨타임마다 공격
    if(Time.time - lastAttackTime >= attackRate)
    {
      StartCoroutine(Attack(monsterStats));
      lastAttackTime = Time.time;
    }
    Debug.Log("공격 상태");

    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 공격
  private IEnumerator Attack(MonsterStats monsterStats)
  {
    Debug.Log("공격");
    animator.SetBool("IsAttack", true);
    BasicEntityBehaviour basicEntityBehaviour = player.GetComponent<BasicEntityBehaviour>();
    basicEntityBehaviour.Hit(monsterStats.attackPower);

    yield return null;
  }

  public override IBehaviorNode.EBehaviorNodeState Patrol(Transform monster, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    animator.SetBool("IsAttack", false);
    
    player = ClosestPlayer(monster, players, monsterStats.patrolRange, spawnPosition);
    float patrolRange = monsterStats.patrolRange; // 순찰 범위
    float moveSpeed = monsterStats.moveSpeed;     // 이동 속도

    // 순찰 범위 내에 플레이어가 있다면 추적 상태로 변경
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn < patrolRange)
    {
      Debug.Log("추적 상태 전환");
      return IBehaviorNode.EBehaviorNodeState.Failure;
    }
    
    // 목적지 설정
    if(!hasPatrolTarget || Vector3.Distance(monster.position, patrolTarget) < 0.5f)
    {
      patrolTarget = spawnPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
      hasPatrolTarget = true;
      Debug.Log("순찰 상태");
    }

    // 목적지로 이동
    monster.position = Vector3.MoveTowards(monster.position, patrolTarget, moveSpeed * Time.deltaTime);
    return IBehaviorNode.EBehaviorNodeState.Running;
  }
}
