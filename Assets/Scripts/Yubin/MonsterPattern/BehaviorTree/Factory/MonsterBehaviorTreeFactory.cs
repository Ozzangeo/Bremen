using System.Collections.Generic;
using UnityEngine;

// 몬스터 행동 트리를 생성하는 팩토리(임시)
public class MonsterBehaviorTreeFactory : IBehaviorTreeFactory
{
  public IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, float attackRange, float detectionRange, float moveSpeed)
  {
    // 개별 액션 노드
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, player, attackRange));      // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(monster));                                 // 공격  
    IBehaviorNode chasePlayer = new ActionNode(() => ChasePlayer(monster, player, detectionRange, moveSpeed));  // 추적

    // 공격 시퀸스 노드
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode>{checkAttackRange, performAttack});

    // 루트 셀렉터 노드
    // 공격이 가능한지 확인 -> 아니라면 플레이어 추적 -> 가능하다면 공격
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode>{attackSequence, chasePlayer});

    return rootSelector;
  }

  // 공격 범위 체크
  private static IBehaviorNode.EBehaviorNodeState CheckAttackRange(Transform monster, Transform player, float attackRange)
  {
    if(Vector3.Distance(monster.position, player.position) <= attackRange) return IBehaviorNode.EBehaviorNodeState.Success;
    else return IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 공격
  private static IBehaviorNode.EBehaviorNodeState PerformAttack(Transform monster) { return IBehaviorNode.EBehaviorNodeState.Success; }

  // 추적
  private static IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, Transform player, float detectionRange, float moveSpeed)
  {
    if(Vector3.Distance(monster.position, player.position) <= detectionRange)
    {
      monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);
      return IBehaviorNode.EBehaviorNodeState.Running;
    }

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }
}