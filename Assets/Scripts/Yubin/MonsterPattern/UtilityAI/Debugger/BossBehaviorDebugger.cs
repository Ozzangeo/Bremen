using UnityEngine;

[RequireComponent(typeof(BossUtilityAI))]
public class BossBehaviorDebugger : MonoBehaviour
{
  Color attackColor = Color.red;  // 공격 범위 색상
  Color fleeColor = Color.green;  // 도망 범위 색상
  BossUtilityAI bossUtilityAI;

  private void OnDrawGizmos()
  {
    if(bossUtilityAI == null) bossUtilityAI = GetComponent<BossUtilityAI>();

    // 공격 범위 그리기
    Gizmos.color = attackColor;
    Gizmos.DrawWireSphere(transform.position, bossUtilityAI.attackRange);

    // 도망 범위 그리기
    Gizmos.color = fleeColor;
    Gizmos.DrawWireSphere(transform.position, bossUtilityAI.fleeDistance);
  }
}
