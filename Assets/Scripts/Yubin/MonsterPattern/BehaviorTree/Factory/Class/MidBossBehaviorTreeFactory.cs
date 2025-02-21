using UnityEngine;
using System.Collections.Generic;

// 중간 보스 행동 트리 가상 클래스
public class MidBossBehaviorTreeFactory : MonoBehaviour, IMidBossBehaviorTreeFactory
{
  [Header("플레이어 어그로 표시")] public bool isPlayerTarget = false;  // 플레이어 어그로 표시
  [Header("플레이어 어그로 해제 거리")] public float playerAway = 5f;   // 플레이어 어그로 해제 거리

  public virtual IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, Transform bitCore, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    // 1. 비트 코어 타겟팅
    // 2. 플레이어에게 피격 시 어그로
    // 3. 플레이어가 도망(5)가면 1로 이동

    IBehaviorNode accessBitCore = new ActionNode(() => AccessBitCore(bitCore, monsterStats, monster));  // 비트 코어 접근
    IBehaviorNode attackBitCore = new ActionNode(() => AttackBitCore(bitCore, monsterStats, monster));  // 비트 코어 공격
    IBehaviorNode chasePlayer = new ActionNode(() => ChasePlayer(player, monsterStats, monster));       // 플레이어 추적
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(player, monsterStats, monster));  // 플레이어 공격

    // 비트 코어 타겟 시퀸스 노드
    IBehaviorNode bitCoreSequence = new SequenceNode(new List<IBehaviorNode> { accessBitCore, attackBitCore});

    // 플레이어 타겟 시퀸스 노드
    IBehaviorNode playerSequence = new SequenceNode(new List<IBehaviorNode> { chasePlayer, performAttack});

    // 비트코어 타겟팅 -> 피격 시 플레이어 타겟팅 -> 플레이어가 멀어지면 다시 비트코어 타겟팅
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { playerSequence, bitCoreSequence});

    return rootSelector;
  }

  // 비트 코어 접근
  public virtual IBehaviorNode.EBehaviorNodeState AccessBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
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

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 비트 코어 공격
  public virtual IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    Debug.Log("공격 상태(비트코어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 플레이어 추적
  public virtual IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform player, MonsterStats monsterStats, Transform monster)
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

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }

  // 플레이어 공격
  public virtual IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Transform monster)
  {
    if(!isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로가 아니라면 비트코어 타겟팅

    Debug.Log("공격 상태(플레이어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 피격
  public void GetDamage()
  {
    Debug.Log("플레이어 어그로 지정");
    isPlayerTarget = true;
  }
}
