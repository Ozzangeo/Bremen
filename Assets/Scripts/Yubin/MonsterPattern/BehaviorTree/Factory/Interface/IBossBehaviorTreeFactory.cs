using UnityEngine;

interface IBossBehaviorTreeFactory
{
  // 보스의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, MonsterStats monsterStats);
}
