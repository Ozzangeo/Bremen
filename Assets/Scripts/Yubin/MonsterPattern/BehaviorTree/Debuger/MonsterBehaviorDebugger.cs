using UnityEngine;

[RequireComponent(typeof(MonsterBehavior))]
public class MonsterDebugger : MonoBehaviour
{
  Color detectionColor = Color.green; // 탐지 범위 색상
  Color attackColor = Color.red;      // 공격 범위 색상
  MonsterBehavior monsterBehavior;    // 몬스터 행동 컴포넌트

  private void OnDrawGizmos()
  {
    if(monsterBehavior == null) monsterBehavior = GetComponent<MonsterBehavior>();

    // 탐지 범위 초록
    Gizmos.color = detectionColor;
    Gizmos.DrawWireSphere(transform.position, monsterBehavior.detectionRange);

    // 공격 범위 빨간색
    Gizmos.color = attackColor;
    Gizmos.DrawWireSphere(transform.position, monsterBehavior.attackRange);
  }
}
