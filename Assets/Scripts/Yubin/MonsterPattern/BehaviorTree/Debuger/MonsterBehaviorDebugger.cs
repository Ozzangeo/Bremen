using UnityEngine;

[RequireComponent(typeof(MonsterBehavior))]
public class MonsterDebugger : MonoBehaviour
{
  Color detectionColor = Color.green; // 탐지 범위 색상
  Color attackColor = Color.red;      // 공격 범위 색상
  MonsterBehavior monsterBehavior;    // 몬스터 행동 컴포넌트
  Vector3 spawnPosition;              // 스폰 위치 저장

  private void Start()
  {
    if(monsterBehavior == null) monsterBehavior = GetComponent<MonsterBehavior>();

    spawnPosition = transform.position;
  }

  private void OnDrawGizmos()
  {
    if(monsterBehavior == null) monsterBehavior = GetComponent<MonsterBehavior>();

    // 탐지 범위 (초록색)
    Gizmos.color = detectionColor;
    Gizmos.DrawWireSphere(spawnPosition, monsterBehavior.monsterStats.patrolRange);

    // 공격 범위 (빨간색)
    Gizmos.color = attackColor;
    Gizmos.DrawWireSphere(transform.position, monsterBehavior.monsterStats.attackRange);
  }
}
