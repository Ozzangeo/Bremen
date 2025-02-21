using UnityEngine;

// 중간 보스 드론 행동 트리를 생성하는 팩토리
// 4박자에 1개가 정확히 뭔지 몰라서 4초에 한번 발사하는거로 임시 구현
public class MidBossDroneBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
	[Header("탄환 프리팹")]
  public GameObject bulletPrefab; // 탄환

  [Header("탄환 속도")] public float bulletSpeed = 6f;      // 탄환 속도
  [Header("유지 거리")] public float maintainDistance = 5f; // 유지 거리
  [Header("후퇴 거리")] public float awayDistance = 10f;    // 후퇴 거리

	float fireRate = 4f;         // 탄환 발사 쿨타임
  float lastFireTime = 0f;     // 마지막 발사 시간

	
}
