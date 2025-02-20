using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 드론 행동 트리를 생성하는 팩토리
// 4박자에 1개가 정확히 뭔지 몰라서 4초에 한번 발사하는거로 임시 구현
[RequireComponent(typeof(MonsterBehavior))]
public class DroneBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("탄환 프리팹")]
  public GameObject bulletPrefab; // 탄환

  [Header("탄환 속도")] public float bulletSpeed = 6f;      // 탄환 속도
  [Header("유지 거리")] public float maintainDistance = 5f; // 유지 거리
  [Header("후퇴 거리")] public float awayDistance = 10f;    // 후퇴 거리

  float fireRate = 4f;         // 탄환 발사 쿨타임
  float lastFireTime = 0f;     // 마지막 발사 시간

  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange; // 순찰 범위

    // 플레이어가 순찰 범위 내에 있으면 공격 상태로 전환, 없다면 순찰 상태로 전환
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 쿨타임마다 발사
    if(Time.time - lastFireTime >= fireRate)
    {
      ShootBullet(player, monsterStats);
      lastFireTime = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 탄환 발사
  private void ShootBullet(Transform player, MonsterStats monsterStats)
  {
    if(bulletPrefab == null) return;

    Debug.Log("탄환 발사");

    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
    if (bulletRigidbody != null)
    {
      // 발사
      Vector3 direction = (player.position - transform.position).normalized;
      bulletRigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);

      // 공격 사거리만큼 이동 후 파괴
      float destroyTime = monsterStats.attackRange / bulletSpeed;
      Destroy(bullet, destroyTime);
    }
  }

  // 추적 재정의
  public override IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
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

    // 유지 거리 이하라면 후퇴
    float distanceToPlayer = Vector3.Distance(monster.position, player.position);
    if(distanceToPlayer <= maintainDistance)
    {
      Vector3 awayDirection = (monster.position - player.position).normalized;
      Vector3 targetPosition = monster.position + awayDirection * awayDistance;

      monster.position = Vector3.MoveTowards(monster.position, targetPosition, moveSpeed * Time.deltaTime);
      return IBehaviorNode.EBehaviorNodeState.Running;
    }

    // 공격 범위 내에 들어왔으면 공격 상태로 전환
    if(distanceToPlayer <= attackRange)
    {
      Debug.Log("공격 상태 전환");
      return IBehaviorNode.EBehaviorNodeState.Failure;
    }

    // 플레이어가 탐지 범위 내에 있으면 계속 추적
    if(distanceToPlayer > maintainDistance)
    {
      Debug.Log("추적 상태");
      monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);
      return IBehaviorNode.EBehaviorNodeState.Running;
    }

    return IBehaviorNode.EBehaviorNodeState.Running;
  }
}
