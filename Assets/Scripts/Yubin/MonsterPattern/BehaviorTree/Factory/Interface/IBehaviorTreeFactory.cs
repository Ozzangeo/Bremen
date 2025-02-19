using UnityEngine;

public interface IBehaviorTreeFactory
{
  // 몬스터의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition);
}

