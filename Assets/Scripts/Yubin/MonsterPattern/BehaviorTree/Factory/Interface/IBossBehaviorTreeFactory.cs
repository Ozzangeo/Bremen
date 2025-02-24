using System.Collections.Generic;
using UnityEngine;

interface IBossBehaviorTreeFactory
{
  // 보스의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, List<Transform> players, MonsterStats monsterStats);
}
