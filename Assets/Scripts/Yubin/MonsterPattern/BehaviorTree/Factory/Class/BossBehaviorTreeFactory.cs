using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorTreeFactory : MonoBehaviour, IBossBehaviorTreeFactory
{
  protected Transform player;         // 가장 가까운 타겟 플레이어
  protected List<Transform> players;  // 플레이어 리스트

  public virtual IBehaviorNode CreateBehaviorTree(Transform monster, List<Transform> players, MonsterStats monsterStats)
  {
    // 가장 가까운 플레이어
    player = ClosestPlayer(players);
    this.players = players;

    // 1. 플레이어 추적
    // 2. 플레이어 공격
    IBehaviorNode chasePlayer = new ActionNode(() => ChasePlayer(monster, monsterStats));           // 추적
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, monsterStats)); // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(monsterStats));                // 공격

    // 공격 시퀸스
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode> { checkAttackRange, performAttack });

    // 추적 -> 공격 범위 확인 -> 가능하다면 공격 아니라면 추적
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { chasePlayer, attackSequence });

    return rootSelector;
  }

  // 가장 가까운 플레이어 찾기
  private Transform ClosestPlayer(List<Transform> players)
  {
    Transform closestPlayer = null;
    float minDistance = float.MaxValue;

    foreach(Transform current in players)
    {
      float temp = Vector3.Distance(transform.position, current.position);
      if(temp < minDistance)
      {
        minDistance = temp;
        closestPlayer = current;
      }
    }

    return closestPlayer;
  }

  // 추적
  public virtual IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, MonsterStats monsterStats)
  {
    float moveSpeed = monsterStats.moveSpeed;      // 이동 속도
    float attackRange = monsterStats.attackRange;  // 공격 범위

    // 공격 범위 내로 들어오면 공격 시퀸스로 전환
    float distanceToPlayer = Vector3.Distance(monster.position, player.position);
    if(distanceToPlayer <= attackRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    Debug.Log("추적 상태");
    monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);
    return IBehaviorNode.EBehaviorNodeState.Running;
  }

  // 공격 범위 확인
  public virtual IBehaviorNode.EBehaviorNodeState CheckAttackRange(Transform monster, MonsterStats monsterStats)
  {
     float attackRange = monsterStats.attackRange; // 공격 범위

    return Vector3.Distance(monster.position, player.position) <= attackRange ? IBehaviorNode.EBehaviorNodeState.Success : IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 공격
  public virtual IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats)
  {
    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }
}
