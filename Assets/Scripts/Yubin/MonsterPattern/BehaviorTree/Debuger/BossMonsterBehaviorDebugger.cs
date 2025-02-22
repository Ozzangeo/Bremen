using UnityEngine;

public class BossMonsterBehaviorDebugger : MonoBehaviour
{
  Color attackColor = Color.red;      // 공격 범위 색상
  BossBehavior bossBehavior;    // 몬스터 행동 컴포넌트

  private void Start()
  {
    if(bossBehavior == null) bossBehavior = GetComponent<BossBehavior>();
  }

  private void OnDrawGizmos()
  {
    if(bossBehavior == null) bossBehavior = GetComponent<BossBehavior>();

    // 공격 범위 (빨간색)
    Gizmos.color = attackColor;
    Gizmos.DrawWireSphere(transform.position, bossBehavior.monsterStats.attackRange);
  }
}
