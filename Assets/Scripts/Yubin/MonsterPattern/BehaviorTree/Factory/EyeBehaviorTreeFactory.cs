using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 눈 행동 트리를 생성하는 팩토리
// 4박자가 정확히 뭔지 몰라서 4초에 한번 발사하는거로 임시 구현
public class EyeBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("돌진 속도")]
  public float dashSpeed = 16f;      // 돌진 속도

  float attackRate = 2f;      // 공격 쿨타임
  float lastAttackTime = 0f;  // 마지막 공격 시간

  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange; // 순찰 범위

    // 플레이어가 순찰 범위 내에 있으면 공격 상태로 전환, 없다면 순찰 상태로 전환
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 돌진 이후 쿨마다 돌진
    if(Time.time - lastAttackTime >= attackRate)
    {
      StartCoroutine(Dash(player, monsterStats));
      lastAttackTime = Time.time;
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