using UnityEngine;

public class BossCombatDroneBehaviorTreeFactory : BossBehaviorTreeFactory
{
  [Header("탄환 프리팹")]
  public GameObject bulletPrefab; // 탄환

  [Header("탄환 속도")] public float bulletSpeed = 9f;      // 탄환 속도
  [Header("유지 거리")] public float maintainDistance = 10f; // 유지 거리
  [Header("후퇴 거리")] public float awayDistance = 15f;    // 후퇴 거리

  float fireRate = 3f;         // 탄환 발사 쿨타임
  float lastFireTime = 0f;     // 마지막 발사 시간

  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats)
  {
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

    // 탄환 두개 발사
    GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

    Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
    Rigidbody bulletRigidbody2 = bullet2.GetComponent<Rigidbody>();

    MonsterAttackPlayer monsterAttackPlayer1 = bullet1.GetComponent<MonsterAttackPlayer>();
    MonsterAttackPlayer monsterAttackPlayer2 = bullet2.GetComponent<MonsterAttackPlayer>();

    monsterAttackPlayer1.Initialize(monsterStats.attackPower);
    monsterAttackPlayer2.Initialize(monsterStats.attackPower);
    
    if(bulletRigidbody1 != null && bulletRigidbody2 != null)
    {
      // 발사
      Vector3 direction = (player.position - transform.position).normalized;
      Quaternion leftRot = Quaternion.Euler(0, -2.5f, 0);  // 왼쪽으로 5도
      Quaternion rightRot = Quaternion.Euler(0, 2.5f, 0);  // 오른쪽으로 5도


      bulletRigidbody1.AddForce(leftRot * direction * bulletSpeed, ForceMode.VelocityChange);
      bulletRigidbody2.AddForce(rightRot * direction * bulletSpeed, ForceMode.VelocityChange);

      // 공격 사거리만큼 이동 후 파괴
      float destroyTime = monsterStats.attackRange / bulletSpeed;
      Destroy(bullet1, destroyTime);
      Destroy(bullet2, destroyTime);
    }
  }

  // 추적 재정의
  public override IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, MonsterStats monsterStats)
  {
    float moveSpeed = monsterStats.moveSpeed;      // 이동 속도
    float attackRange = monsterStats.attackRange;  // 공격 범위

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
