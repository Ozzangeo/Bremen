using UnityEngine;

public class MidBossCombatDroneBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("탄환 프리팹")]
  public GameObject bulletPrefab; // 탄환

  [Header("탄환 속도")] public float bulletSpeed = 9f;      // 탄환 속도
  [Header("유지 거리")] public float maintainDistance = 10f; // 유지 거리
  [Header("후퇴 거리")] public float awayDistance = 15f;    // 후퇴 거리
	[Header("공격 쿨타임 거리")] float fireRate = 3f;         // 탄환 발사 쿨타임

  float lastAttackTimePlayer = 0f;  // 마지막 공격 시간 (플레이어)
  float lastAttackTimeBitCore = 0f; // 마지막 공격 시간 (비트코어)

	// 비트 코어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    if(Time.time - lastAttackTimeBitCore >= fireRate)
    {
			ShootBullet(bitCore, monsterStats);
      lastAttackTimeBitCore = Time.time;
    }

    Debug.Log("공격 상태(비트코어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 플레이어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Transform monster)
  {
    if(!isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로가 아니라면 비트코어 타겟팅

    if(Time.time - lastAttackTimePlayer >= fireRate)
    {
			ShootBullet(player, monsterStats);
      lastAttackTimePlayer = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 탄환 발사
  private void ShootBullet(Transform target, MonsterStats monsterStats)
  {
    if(bulletPrefab == null) return;

    Debug.Log("탄환 발사");

    // 탄환 두개 발사
    GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

    Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
    Rigidbody bulletRigidbody2 = bullet2.GetComponent<Rigidbody>();
    if(bulletRigidbody1 != null && bulletRigidbody2 != null)
    {
      // 발사
      Vector3 direction = (target.position - transform.position).normalized;
      Quaternion leftRot = Quaternion.Euler(0, -5, 0);  // 왼쪽으로 5도
      Quaternion rightRot = Quaternion.Euler(0, 5, 0);  // 오른쪽으로 5도


      bulletRigidbody1.AddForce(leftRot * direction * bulletSpeed, ForceMode.VelocityChange);
      bulletRigidbody2.AddForce(rightRot * direction * bulletSpeed, ForceMode.VelocityChange);

      // 공격 사거리만큼 이동 후 파괴
      float destroyTime = monsterStats.attackRange / bulletSpeed;
      Destroy(bullet1, destroyTime);
      Destroy(bullet2, destroyTime);
    }
  }
}
