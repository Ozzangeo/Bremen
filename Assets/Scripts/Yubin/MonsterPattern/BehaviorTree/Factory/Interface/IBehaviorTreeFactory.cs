using UnityEngine;
using System.Collections.Generic;

public interface IBehaviorTreeFactory
{
  // 몬스터의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, List<Transform> players, MonsterStats monsterStats, Vector3 spawnPosition);
}

