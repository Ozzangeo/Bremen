using UnityEngine;

public interface IBehaviorTreeFactory
{
  // 몬스터의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, float attackRange, float detectionRange, float moveSpeed);
}

