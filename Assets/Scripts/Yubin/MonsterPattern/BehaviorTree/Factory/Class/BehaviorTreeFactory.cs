using System.Collections.Generic;
using UnityEngine;

// 행동 트리 가상 클래스
public class BehaviorTreeFactory : MonoBehaviour, IBehaviorTreeFactory
{
  public virtual IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    // 개별 액션 노드
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, player, monsterStats));       // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(player, monsterStats, spawnPosition));       // 공격
    IBehaviorNode chasePlayer = new ActionNode(() => ChasePlayer(monster, player, monsterStats, spawnPosition));  // 추적
    IBehaviorNode patrolArea = new ActionNode(() => Patrol(monster, player, monsterStats, spawnPosition));        // 순찰
    IBehaviorNode returnToSpawn = new ActionNode(() => ReturnToSpawn(monster, spawnPosition, monsterStats));      // 복귀

    // 공격 시퀸스 노드
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode> { checkAttackRange, performAttack });

    // 순찰 루틴 (공격 범위에 없다면 순찰 or 복귀)
    IBehaviorNode patrolSelector = new SelectorNode(new List<IBehaviorNode> { patrolArea, returnToSpawn });

    //공격 가능하면 공격 → 아니면 플레이어 추적 → 순찰 또는 복귀
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { chasePlayer, attackSequence, patrolSelector });

    return rootSelector;
  }

  // 공격 범위 체크
  public virtual IBehaviorNode.EBehaviorNodeState CheckAttackRange(Transform monster, Transform player, MonsterStats monsterStats)
  {
    float attackRange = monsterStats.attackRange; // 공격 범위

    return Vector3.Distance(monster.position, player.position) <= attackRange ? IBehaviorNode.EBehaviorNodeState.Success : IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 공격 실행
  public virtual IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange; // 순찰 범위

    // 플레이어가 순찰 범위 내에 있으면 공격 상태로 전환, 없다면 순찰 상태로 전환
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange) return IBehaviorNode.EBehaviorNodeState.Failure;
    
    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 추적
  public virtual IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange;  // 순찰 범위
    float moveSpeed = monsterStats.moveSpeed;      // 이동 속도
    float attackRange = monsterStats.attackRange;  // 공격 범위

    // 플레이어가 순찰 범위를 벗어나면 추적 중단, 순찰 상태로 변경
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange)
    {
      Debug.Log("순찰 상태 전환");
      return IBehaviorNode.EBehaviorNodeState.Failure;
    }

    // 공격 범위 내에 들어왔으면 공격 상태로 전환
    float distanceToPlayer = Vector3.Distance(monster.position, player.position);
    if(distanceToPlayer <= attackRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 플레이어가 탐지 범위 내에 있으면 계속 추적
    Debug.Log("추적 상태");
    monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);
    return IBehaviorNode.EBehaviorNodeState.Running;
  }

  // 순찰
  [HideInInspector] public Vector3 patrolTarget;          // 순찰 목적지
  [HideInInspector] public bool hasPatrolTarget = false;  // 타겟 유무

  public virtual IBehaviorNode.EBehaviorNodeState Patrol(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
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

  // 스폰 복귀
  public virtual IBehaviorNode.EBehaviorNodeState ReturnToSpawn(Transform monster, Vector3 spawnPosition, MonsterStats monsterStats)
  {
    float moveSpeed = monsterStats.moveSpeed; // 이동 속도

    if(Vector3.Distance(monster.position, spawnPosition) > 0.5f)
    {
      monster.position = Vector3.MoveTowards(monster.position, spawnPosition, moveSpeed * Time.deltaTime);
      Debug.Log("스폰 복귀");
      return IBehaviorNode.EBehaviorNodeState.Running;
    }

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }
}
