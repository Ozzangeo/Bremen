using UnityEngine;

[RequireComponent(typeof(MidBossBehavior))]
[RequireComponent(typeof(MidBossBehaviorTreeFactory))]
public class MidBossBehaviorDebugger : MonoBehaviour
{
  Color attackColor = Color.red;      // 공격 범위 색상
  Color playerColor = Color.green;      // 플레이어 탐지 범위 색상

  MidBossBehavior midBossBehavior;    // 중간 보스 행동 컴포넌트
  MidBossBehaviorTreeFactory midBossBehaviorTreeFactory;

  private void Start()
  {
    midBossBehavior = GetComponent<MidBossBehavior>();
    midBossBehaviorTreeFactory = GetComponent<MidBossBehaviorTreeFactory>();
  }

  private void OnDrawGizmos()
  {
    if(midBossBehavior == null) midBossBehavior = GetComponent<MidBossBehavior>();
    if(midBossBehaviorTreeFactory == null) midBossBehaviorTreeFactory = GetComponent<MidBossBehaviorTreeFactory>();
    // 공격 범위 (빨간색)
    Gizmos.color = attackColor;
    Gizmos.DrawWireSphere(transform.position, midBossBehavior.monsterStats.attackRange);

    // 플레이어 탐지 범위 (초록색)
    Gizmos.color = playerColor;
    Gizmos.DrawWireSphere(transform.position, midBossBehaviorTreeFactory.playerAway);
  }
}
